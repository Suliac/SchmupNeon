﻿using UnityEngine;

/// <summary>
/// ShootOnDeathEnemy Description
/// </summary>
public class ShootOnDeathEnemy : ShootingEnemy
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
    }

    protected override void Move()
    {
    }

    protected override void OnBeforeKill()
    {
        if (weaponHandle)
            weaponHandle.UseWeapon();
    }

    #endregion
}
