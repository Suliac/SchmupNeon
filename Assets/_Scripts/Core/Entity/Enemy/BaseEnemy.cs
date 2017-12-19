using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Moving,
    Preshot,
    Shooting
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

    private float blinkDuration = 1.0f;
    private float blinkRedDuration = 0.2f;
    private float preShotTimer = 0.0f;
    private bool coroutineStarted = false;
    private Renderer renderer;

    protected bool enableEnemy = false;         // Etat actuel de l'ennemi
    protected IsOnCamera isOnCamera;
    protected Rigidbody body;             //ref du rigidbody

    
    protected EnemyState currentState = EnemyState.Moving;
    #endregion

    #region Initialization
    protected void Awake()
    {
        isOnCamera = GetComponent<IsOnCamera>();
        body = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
    }

    #endregion

    #region Core
    abstract protected void Move();
    abstract protected void Shoot();
    abstract protected void OnBeforeKill();

    protected void OnPreShotPhase()
    {
        if (!coroutineStarted)
            StartCoroutine(Blink(blinkDuration, blinkRedDuration));
    }

    protected void OnEnemyEnable()
    {
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
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du player");
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

            if(myTimer > oneColorDuration)
            {
                isRed = !isRed;
                myTimer = 0.0f;
            }
            
            if (isRed)
            {
                renderer.material.color = Color.red;
            }
            else
            {
                renderer.material.color = Color.white;
            }

            yield return null;
        }

        print("End blink");
        preShotTimer = 0.0f;
        coroutineStarted = false;
        currentState = EnemyState.Shooting;
        yield return null;
    }

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        //Debug.Log("Dead");
        OnBeforeKill();
        CreateDeathObject();
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

        switch (currentState)
        {
            case EnemyState.Moving:
                Move();
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
    }

    #endregion
}
