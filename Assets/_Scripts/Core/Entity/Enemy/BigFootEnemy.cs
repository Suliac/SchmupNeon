using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// BigFootEnemy Description
/// </summary>
public class BigFootEnemy : ShootingEnemy
{
    #region Attributes
    [FoldoutGroup("Gameplay"), Tooltip("Tir toutes les X secondes"), SerializeField]
    private float timeShooting = 1.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Nombre de tir durant les phases de tir"), SerializeField]
    private int NumberOfShoots = 3;

    private Vector3 topPosition;
    private Vector3 botPosition;

    private int currentNumberOfShoots = 0;
    private float currentTimeShooting = 0.0f;

    private bool goingTop = true;
    private bool isShooting = false;
    #endregion

    #region Initialization
    protected void Awake()
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
        currentTimeShooting += Time.deltaTime;

        Vector3 nextCheckPoint = goingTop ? topPosition : botPosition;

        Vector3 dir = nextCheckPoint - transform.position;
        body.velocity = dir.normalized * speed;

        if ((goingTop && transform.position.y >= topPosition.y) || (!goingTop && transform.position.y <= botPosition.y))
            goingTop = !goingTop;

        if (currentTimeShooting >= timeShooting)
        {
            currentState = EnemyState.Preshot;
            currentTimeShooting = 0.0f;
        }
    }

    protected override void Shoot()
    {
        body.velocity = Vector3.zero;
        if (weaponHandle.UseWeapon())
            currentNumberOfShoots++;

        if (currentNumberOfShoots >= NumberOfShoots)
        {
            isShooting = false;
            currentState = EnemyState.Moving;

            currentNumberOfShoots = 0;
        }
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }
    #endregion

    #region Unity ending functions

    #endregion
}
