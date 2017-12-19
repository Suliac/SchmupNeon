using Sirenix.OdinInspector;
using UnityEngine;

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

    protected bool enableEnemy = false;         // Etat actuel de l'ennemi
    protected IsOnCamera isOnCamera;
    protected Rigidbody body;             //ref du rigidbody


    #endregion

    #region Initialization
    protected void Awake()
    {
        isOnCamera = GetComponent<IsOnCamera>();
        body = GetComponent<Rigidbody>();
    }

    #endregion

    #region Core
    abstract protected void Move();
    abstract protected void Shoot();
    abstract protected void OnBeforeKill();


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

        Move();
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
    }

    #endregion
}
