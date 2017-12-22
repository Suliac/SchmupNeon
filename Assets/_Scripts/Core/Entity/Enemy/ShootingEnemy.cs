using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingEnemy : BaseEnemy {
    #region Attributes

    protected WeaponHandler weaponHandle; // Lien vers le script de gestion du port d'arme

    #endregion

    #region Initialization

    protected new void Awake()
    {
        base.Awake();
        weaponHandle = GetComponent<WeaponHandler>();
    }
    

    #endregion
}
