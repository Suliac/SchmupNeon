﻿using UnityEngine;

/// <summary>
/// SimpleShootingEnemy Description
/// </summary>
public class SimpleShootingEnemy : ShootingEnemy
{
    #region Attributes

    #endregion

    #region Initialization
    

    #endregion

    #region Core

    /// <summary>
    /// Fonction appelée à chaque frame depuis la classe <see cref="BaseEnemy"/>
    /// </summary>
    protected override void Shoot()
    {
        if (weaponHandle)
            if (weaponHandle.UseWeapon())// Shoot every time it can
                currentState = EnemyState.Moving;
    }

    protected override void Move()
    {
        currentState = EnemyState.Preshot;
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }
    #endregion


}
