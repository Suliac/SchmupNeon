using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// class abstraite des weapons,
/// contient un cooldown, et une reference au player
/// </summary>
public abstract class Projectile : MonoBehaviour, IKillable
{
    #region Attributes
    [FoldoutGroup("GamePlay")] [Tooltip("ref du prefabs de la particule de mort"), SerializeField] protected string prefabsDeathTag;
    //public string PrefabsDeathTag { get { return prefabsDeathTag; } }

    [FoldoutGroup("GamePlay"), Tooltip("Distance maximale de la balle (0 = pas de limitation)"), SerializeField]
    private float maxDistance = 0.0f; // NB : max distance == 0 -> pas de limitation de distance



    [FoldoutGroup("Debug")] [Tooltip("ref sur playerController")] protected PlayerController playerController;
    public PlayerController PlayerController { set { playerController = value; } get { return playerController; } }

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private SpriteRenderer objectSprite;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private Color colorBullet;
    public Color ColorBullet
    {
        //get { return colorBullet; }
        set
        {
            colorBullet = value;
            objectSprite.color = colorBullet;
        }
    }
    protected Rigidbody bodyBullet;   //ref du rigidbody
    protected IsOnCamera isOnCamera;  //ref du IsOnCamera pour savoir si le bullet est hors champs
    protected bool enabledBullet = false; //le bullet, au démarage ne fait pas de dégat !
    protected Vector3 startPosition;
    #endregion

    #region Initialization
    protected void Awake()
    {
        bodyBullet = GetComponent<Rigidbody>();
        isOnCamera = GetComponent<IsOnCamera>();
    }
    #endregion
    
    #region Core
    public void Setup(PlayerController refPlayer, float addSpeed, Vector3 orientation)
    {
        startPosition = transform.position;
        SetUpBullet(refPlayer, addSpeed, orientation);
    }

    abstract protected void SetUpBullet(PlayerController refPlayer, float addSpeed, Vector3 orientation);
    abstract protected void MoveProjectile();
    abstract protected void OnProjectileTooFar();
    public virtual void Kill() { }
    public virtual void Kill(bool createBullet) { }
    #endregion

    #region Unity ending functions
    private void Update()
    {
        if (!enabledBullet) //si le bullet est désactivé, ne pas effectuer de test...
            return;

        MoveProjectile();

        if(maxDistance > 0)
        {
            Vector3 distance = transform.position - startPosition;
            if(distance.magnitude > maxDistance)
            {
                OnProjectileTooFar();
            }
        }

        if (updateTimer.Ready())
        {
            if (!isOnCamera.enabled)
                isOnCamera.enabled = true;
            if (!isOnCamera.isOnScreen)
                Kill();
        }
    }
    #endregion
}
