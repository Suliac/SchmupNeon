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

    [FoldoutGroup("Gameplay"), Tooltip("Vitesse des ennemis"), SerializeField]
    protected float speed = 1.0f;

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

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        Debug.Log("Dead");
        Destroy(gameObject);
    }
    #endregion

    #region Unity ending functions

    protected void Update()
    {
        if (isOnCamera.isOnScreen && !enableEnemy) // Si l'ennemi vient d'apparaitre & n'a pas déja été spawn
            enableEnemy = true;

        if (!enableEnemy)
            return;

        Move();
        Shoot();

        //optimisation des fps
        if (updateTimer.Ready())
        {
            if (!isOnCamera.enabled)
                isOnCamera.enabled = true;
            if (!isOnCamera.isOnScreen)
                Kill(); // Kill si sort de l'écran
        }
    }

    #endregion
}
