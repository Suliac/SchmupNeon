using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : BaseEnemy
{
    #region Attributes
    [FoldoutGroup("Gameplay"), Tooltip("Orientation du trajet de l'ennemi"), SerializeField]
    private float angle = 180.0f; 
    #endregion

    #region Core
    protected override void Move()
    {
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed, 0);
        body.velocity = dir;

    }

    protected override void Shoot()
    {
        // Nothing to do
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }
    #endregion
}
