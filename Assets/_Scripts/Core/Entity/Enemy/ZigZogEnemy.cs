using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZogEnemy : ShootingEnemy
{
    #region Attributes
    [FoldoutGroup("Gameplay"), Tooltip("Orientation du trajet de l'ennemi lors du déplacement vers le haut"), SerializeField]
    private float angleTop = 135.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Orientation du trajet de l'ennemi lors du déplacement vers le haut"), SerializeField]
    private float angleBot = 225.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Durée des phases de déplacement"), SerializeField]
    private float timeMoving = 1.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Nombre de tir durant les phases de tir"), SerializeField]
    private int NumberOfShoots = 3;

    private int currentNumberOfShoots = 0;
    private float currentTimeMoving = 0.0f;

    private bool goingTop = true;
    #endregion

    #region Core
    protected override void Move()
    {
        currentTimeMoving += Time.deltaTime;
        float angle = goingTop ? angleTop : angleBot;
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed, 0);
        body.velocity = dir;

        if (currentTimeMoving >= timeMoving)
        {
            body.velocity = Vector3.zero;
            currentState = EnemyState.Preshot;
            currentTimeMoving = 0.0f;
            goingTop = !goingTop;
        }
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }

    protected override void Shoot()
    {
        body.velocity = Vector3.zero;
        if (weaponHandle.UseWeapon())
            currentNumberOfShoots++;

        if (currentNumberOfShoots >= NumberOfShoots)
        {
            currentNumberOfShoots = 0;
            currentState = EnemyState.Moving;
        }
    }
    #endregion
}
