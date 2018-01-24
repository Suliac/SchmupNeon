using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


/// <summary>
/// BaseEnemy Description
/// </summary>
public abstract class BaseEnemy : MonoBehaviour, IKillable
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [Header("Attributs ennemis généraux")]
    [FoldoutGroup("GamePlay"), Tooltip("Vitesse des ennemis"), SerializeField]
    protected float moveSpeed = 1.0f;

    [FoldoutGroup("GamePlay"), Tooltip("Ennemi activé par défaut"), SerializeField]
    protected bool wantToEnable = true;         // Etat actuel de l'ennemi
    public bool WantToEnable { get { return wantToEnable; } set { wantToEnable = value; } }

    [FoldoutGroup("GamePlay"), Tooltip("ref du prefabs de la particule de mort"), SerializeField]
    private string prefabsDeathTag;

    protected bool wantToDisable = false;         // Etat actuel de l'ennemi
    public bool WantToDisable { get { return wantToDisable; } set { wantToDisable = value; } }

    //[FoldoutGroup("GamePlay"), Tooltip("Durée du blink"), SerializeField]
    //private float blinkDuration = 1.0f;
    //[FoldoutGroup("GamePlay"), Tooltip("Fréquence du blink"), SerializeField]
    //private float blinkFrequency = 0.2f;
    //private float preShotTimer = 0.0f;
    //private bool coroutineStarted = false;

    private bool coroutinePrepareToMoveStarted = false;

    [FoldoutGroup("GamePlay"), Tooltip("Temps avant de relancer le mouvement"), SerializeField]
    private float timeBeforeMoving = 1.0f;

    protected bool enableEnemy = false;         // Etat actuel de l'ennemi
    protected IsOnCamera isOnCamera;
    protected Rigidbody body;             //ref du rigidbody
    protected Collider collider;             //ref du collider
    //private Renderer renderer;
    protected Animator animator;

    [FoldoutGroup("GamePlay"), Tooltip("Ennemi en train de tirer"), SerializeField]
    protected bool isShooting = true;

    [FoldoutGroup("GamePlay"), Tooltip("Ennemi en train de bouger"), SerializeField]
    protected bool isMoving = false;

    [FoldoutGroup("GamePlay"), Tooltip("Temps avant que l'ennemi n ebouge plus dans la caméra (-1 si il bouge tout le temps)"), SerializeField]
    protected float timeBeforeStop = 1f;

    protected bool lastFrameMoving = false;
    private bool readyToMove = false;

    [FoldoutGroup("GamePlay"), Tooltip("Ennemi mort"), SerializeField]
    protected bool isDead = false;
    #endregion

    #region Initialization
    protected void Awake()
    {
        isOnCamera = GetComponent<IsOnCamera>();
        body = GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        collider = GetComponent<Collider>();

        readyToMove = isMoving;
    }

    #endregion

    #region Core
    abstract protected void Move();
    abstract protected void Shoot();
    abstract protected void OnBeforeKill();
    abstract protected void OnEnableInCamera();

    //protected void OnPreShotPhase()
    //{
    //    if (!coroutineStarted)
    //        StartCoroutine(Blink(blinkDuration, blinkFrequency));
    //}

    protected void OnEnemyEnable()
    {
        //print("enable : " + name);
        enableEnemy = true;
        collider.enabled = true;
        OnEnableInCamera(); //lors de l'activation, appeler cette fonction
        transform.SetParent(GameManager.GetSingleton.EnemyGroup);   //set l'ennemy dans la moving platform
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

    IEnumerator ReadyToMove(float time)
    {
        coroutinePrepareToMoveStarted = true;
        yield return new WaitForSeconds(time);
        coroutinePrepareToMoveStarted = false;
        readyToMove = true;
        yield return null;
    }

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        if (!isDead)
        {
            SoundManager.GetSingularity.PlayDeadEnemySound();

            isDead = true;
            enableEnemy = false;
            body.velocity = Vector3.zero;
            transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);
            collider.enabled = false;

            OnBeforeKill();
            CreateDeathObject();

            if (animator)
                animator.SetTrigger("enemyIsDead");
            else
                DeathAnimationFinised();
        }
    }

    public void DeathAnimationFinised()
    {
        GameManager.GetSingleton.WinManager.NewEnemyKill();
        Destroy(gameObject);
    }
    #endregion

    #region Unity ending functions

    protected void Update()
    {
        if (!isDead)
        {
            if (((isOnCamera && isOnCamera.isOnScreen) || (!isOnCamera && wantToEnable)) && !enableEnemy) // Si l'ennemi vient d'apparaitre & n'a pas déja été spawn
                OnEnemyEnable();

            if (!enableEnemy)
                return;

            if (isMoving)
            {
                if (!coroutinePrepareToMoveStarted && !readyToMove)
                    StartCoroutine(ReadyToMove(timeBeforeMoving));

                if (readyToMove)
                    Move();
            }
            else
            {
                readyToMove = false;
            }

            if (isShooting)
                Shoot();

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

            lastFrameMoving = isMoving;
        }
    }

    #endregion
}
