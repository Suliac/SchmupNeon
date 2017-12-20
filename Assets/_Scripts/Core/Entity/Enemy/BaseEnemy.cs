using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public enum EnemyState
{
    Moving,
    Preshot,
    Shooting,
    Death
}

/// <summary>
/// BaseEnemy Description
/// </summary>
public abstract class BaseEnemy : MonoBehaviour, IKillable
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("GamePlay"), Tooltip("Vitesse des ennemis"), SerializeField]
    protected float speed = 1.0f;

    [FoldoutGroup("GamePlay"), Tooltip("Ennemi activé par défaut"), SerializeField]
    protected bool wantToEnable = true;         // Etat actuel de l'ennemi
    public bool WantToEnable { get { return wantToEnable; } set { wantToEnable = value; } }

    [FoldoutGroup("GamePlay"), Tooltip("ref du prefabs de la particule de mort"), SerializeField]
    private string prefabsDeathTag;

    protected bool wantToDisable = false;         // Etat actuel de l'ennemi
    public bool WantToDisable { get { return wantToDisable; } set { wantToDisable = value; } }

    [FoldoutGroup("GamePlay"), Tooltip("Durée du blink"), SerializeField]
    private float blinkDuration = 1.0f;
    [FoldoutGroup("GamePlay"), Tooltip("Fréquence du blink"), SerializeField]
    private float blinkFrequency = 0.2f;
    private float preShotTimer = 0.0f;
    private bool coroutineStarted = false;
    private bool coroutinePrepareToMoveStarted = false;
    private bool readyToMove = true;

    private Renderer renderer;
    protected Animator animator;

    [FoldoutGroup("GamePlay"), Tooltip("Temps avant de relancer le mouvement"), SerializeField]
    private float timeBeforeMoving = 1.0f;

    protected bool enableEnemy = false;         // Etat actuel de l'ennemi
    protected IsOnCamera isOnCamera;
    protected Rigidbody body;             //ref du rigidbody


    protected EnemyState currentState = EnemyState.Moving;
    protected EnemyState previousState;
    #endregion

    #region Initialization
    protected void Awake()
    {
        isOnCamera = GetComponent<IsOnCamera>();
        body = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    #endregion

    #region Core
    abstract protected void Move();
    abstract protected void Shoot();
    abstract protected void OnBeforeKill();

    protected void OnPreShotPhase()
    {
        if (!coroutineStarted)
            StartCoroutine(Blink(blinkDuration, blinkFrequency));
    }

    protected void OnEnemyEnable()
    {
        animator.SetBool("IsDead", false);
        enableEnemy = true;
    }

    protected void OnEnemyDisable()
    {
        enableEnemy = true;
    }

    private void CreateDeathObject()
    {
        GameObject deathEnemy = ObjectsPooler.GetSingleton.GetPooledObject(prefabsDeathTag, false);
        if (!deathEnemy)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du player tag:" + prefabsDeathTag + " | " + name);
            return;
        }
        deathEnemy.transform.position = transform.position;
        deathEnemy.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);
        deathEnemy.SetActive(true);
    }

    IEnumerator Blink(float totalDuration, float oneColorDuration)
    {
        coroutineStarted = true;
        bool isRed = false;
        float myTimer = 0.0f;

        while (preShotTimer < totalDuration)
        {
            preShotTimer += Time.deltaTime;
            myTimer += Time.deltaTime;

            if (myTimer > oneColorDuration)
            {
                isRed = !isRed;
                myTimer = 0.0f;
            }

            if (isRed)
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.white;

            yield return null;
        }
        renderer.material.color = Color.white;

        preShotTimer = 0.0f;
        coroutineStarted = false;
        currentState = EnemyState.Shooting;
        yield return null;
    }

    IEnumerator ReadyToMove(float time)
    {
        coroutinePrepareToMoveStarted = true;
        yield return new WaitForSeconds(time);
        readyToMove = true;
        coroutinePrepareToMoveStarted = false;
        yield return null;
    }

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        //Debug.Log("Dead");
        if (currentState != EnemyState.Death)
        {
            currentState = EnemyState.Death;
            OnBeforeKill();
            CreateDeathObject();

            if (animator)
            {
                animator.SetBool("isDead", true);
            }
            else
            {
                Destroy(gameObject);
            }

            enableEnemy = false;
        }
    }

    public void DeathAnimationFinised()
    {
        print("real destroy");
        Destroy(gameObject);
    }
    #endregion

    #region Unity ending functions

    protected void Update()
    {
        if (((isOnCamera && isOnCamera.isOnScreen) || (!isOnCamera && wantToEnable)) && !enableEnemy) // Si l'ennemi vient d'apparaitre & n'a pas déja été spawn
            OnEnemyEnable();

        if (!enableEnemy)
            return;
        if (previousState != currentState)
        {
            readyToMove = false;

        }
        switch (currentState)
        {
            case EnemyState.Moving:

                if (readyToMove)
                {
                    Move();
                }
                else
                {
                    if (!coroutinePrepareToMoveStarted)
                        StartCoroutine(ReadyToMove(timeBeforeMoving));
                }
                break;
            case EnemyState.Preshot:
                OnPreShotPhase();
                break;
            case EnemyState.Shooting:
                Shoot();
                break;
            default:
                break;
        }

        //optimisation des fps
        if (updateTimer.Ready())
        {
            if (isOnCamera)
            {
                if (!isOnCamera.enabled)
                    isOnCamera.enabled = true;
                if (!isOnCamera.isOnScreen)
                    wantToDisable = true; // Kill si sort de l'écran 
            }

            if (wantToDisable) // Sert pour gérer le cas ou l'ennemi est activé depuis l'exterieur
            {
                wantToEnable = false;
                Kill();
            }
        }


        previousState = currentState;
    }

    #endregion
}
