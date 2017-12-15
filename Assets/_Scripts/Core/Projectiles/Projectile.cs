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
    [FoldoutGroup("Debug")] [Tooltip("ref sur playerController")] private PlayerController playerController;
    public PlayerController PlayerController { set { playerController = value; } get { return playerController; } }

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    protected Rigidbody bodyBullet;   //ref du rigidbody
    protected IsOnCamera isOnCamera;  //ref du IsOnCamera pour savoir si le bullet est hors champs
    protected bool enabledBullet = false; //le bullet, au démarage ne fait pas de dégat !

    #endregion

    #region Initialization
    protected void Awake()
    {
        bodyBullet = GetComponent<Rigidbody>();
        isOnCamera = GetComponent<IsOnCamera>();
    }
    #endregion

    #region Core

    abstract public void SetUpBullet(PlayerController refPlayer, float addSpeed, BulletOrientation orientation);
    abstract protected void MoveProjectile();
    public virtual void Kill() { }
    #endregion

    #region Unity ending functions
    private void Update()
    {
        if (!enabledBullet) //si le bullet est désactivé, ne pas effectuer de test...
            return;

        MoveProjectile();

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
