using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// BigFootEnemy Description
/// </summary>
public class BigFootEnemy : ShootingEnemy
{
    #region Attributes
    [Header("Attributs bigfoot")]

    [FoldoutGroup("GamePlay"), Tooltip("Tir toutes les X secondes"), SerializeField]
    private FrequencyTimer shootFrequency;

    [FoldoutGroup("GamePlay"), Tooltip("Nombre de tir durant les phases de tir"), SerializeField]
    private int NumberOfShoots = 3;

    private Vector3 topPosition;
    private Vector3 botPosition;

    private int currentNumberOfShoots = 0;

    private bool goingTop = true;
    #endregion

    #region Initialization
    protected new void Awake()
    {
        base.Awake();
        topPosition = transform.GetChild(1).GetChild(0).position; // crado !
        botPosition = transform.GetChild(1).GetChild(1).position; // crado !
    }


    private void Start()
    {
        // Start function
    }
    #endregion

    #region Core
    protected override void Move()
    {
        Vector3 nextCheckPoint = goingTop ? topPosition : botPosition;

        Vector3 dir = nextCheckPoint - transform.position;
        body.velocity = dir.normalized * moveSpeed;

        if ((goingTop && transform.position.y >= topPosition.y) || (!goingTop && transform.position.y <= botPosition.y))
            goingTop = !goingTop;
    }

    protected override void Shoot()
    {
        if (shootFrequency.Ready())
        {
            if (currentNumberOfShoots < NumberOfShoots)
            {
                if (weaponHandle.UseWeapon())
                    currentNumberOfShoots++;
            }
            else
            {
                currentNumberOfShoots = 0;
            } 
        }
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }

    /// <summary>
    /// lors de l'activation de l'objet quand il entre dans la caméra
    /// </summary>
    protected override void OnEnableInCamera()
    {
        // Nothing to do
    }
    
    #endregion

    #region Unity ending functions

    #endregion
}
