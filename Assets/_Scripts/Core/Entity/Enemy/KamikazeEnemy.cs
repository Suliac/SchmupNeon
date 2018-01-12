using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : BaseEnemy
{
    #region Attributes
    [Header("Attributs kamikaze")]
    [FoldoutGroup("GamePlay"), Tooltip("Orientation du trajet de l'ennemi"), SerializeField]
    private float angle = 180.0f; 
    #endregion

    #region Core
    protected override void Move()
    {
        Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * moveSpeed, Mathf.Sin(Mathf.Deg2Rad * angle) * moveSpeed, 0);
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

    /// <summary>
    /// lors de l'activation de l'objet quand il entre dans la caméra
    /// </summary>
    protected override void OnEnableInCamera()
    {
        // Nothing to do
    }
    #endregion
}
