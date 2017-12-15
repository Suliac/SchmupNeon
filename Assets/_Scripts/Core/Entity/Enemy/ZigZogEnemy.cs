using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZogEnemy : ShootingEnemy {
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
    private bool isShooting = false;
    #endregion

    #region Core
    protected override void Move()
    {
        if (!isShooting)
        {
            currentTimeMoving += Time.deltaTime;
            float angle = goingTop ? angleTop : angleBot;
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed, 0);
            body.velocity = dir; 
            
            if(currentTimeMoving >= timeMoving)
            {
                isShooting = true;
                currentTimeMoving = 0.0f;
                goingTop = !goingTop;
            }
        }
        else
        {
            body.velocity = Vector3.zero;
        }

    }

    protected override void Shoot()
    {
        if(isShooting)
        {
            body.velocity = Vector3.zero;
            if (weaponHandle.UseWeapon())
                currentNumberOfShoots++;
            
            if (currentNumberOfShoots >= NumberOfShoots)
            {
                currentNumberOfShoots = 0;
                isShooting = false;
            }
        }
    } 
    #endregion
}
