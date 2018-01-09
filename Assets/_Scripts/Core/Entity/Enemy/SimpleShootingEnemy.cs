using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// SimpleShootingEnemy Description
/// </summary>
public class SimpleShootingEnemy : ShootingEnemy
{
    #region Attributes
    [Header("Attributs simpleshootingenemy")]
    [FoldoutGroup("GamePlay"), Tooltip("Tir toutes les X secondes"), SerializeField]
    private FrequencyTimer shootFrequency;
    #endregion

    #region Initialization


    #endregion

    #region Core

    /// <summary>
    /// Fonction appelée à chaque frame depuis la classe <see cref="BaseEnemy"/>
    /// </summary>
    protected override void Shoot()
    {
        if (shootFrequency.Ready())
        {
            if (weaponHandle)
                weaponHandle.UseWeapon(); 
        }
    }

    protected override void Move()
    {
        
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }
    #endregion


}
