using UnityEngine;

/// <summary>
/// SimpleShootingEnemy Description
/// </summary>
public class SimpleShootingEnemy : BaseEnemy
{
    #region Attributes

    private WeaponHandler weaponHandle; // Lien vers le script de gestion du port d'arme

    #endregion

    #region Initialization

    private void Awake()
    {
        weaponHandle = GetComponent<WeaponHandler>();
    }

    #endregion

    #region Core

    /// <summary>
    /// Fonction appelée à chaque frame depuis la classe <see cref="BaseEnemy"/>
    /// </summary>
    protected override void Behavior()
    {
        if (weaponHandle)
            weaponHandle.UseWeapon(); // Shoot every time it cans
    }

    #endregion


}
