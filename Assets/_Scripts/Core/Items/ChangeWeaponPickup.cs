using Sirenix.OdinInspector;
using UnityEngine;

public enum Weapon
{
    SimpleRifle = 0,
    LaserBeam = 1,
    Shotgun = 2,
    Wavegun = 3
}

/// <summary>
/// ChangeWeaponPickup Description
/// </summary>
public class ChangeWeaponPickup : Pickup
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Gameplay"), Tooltip("ref sur HandablePickup"), SerializeField]
    private Weapon NewWeaponId = Weapon.LaserBeam;

    #endregion

    #region Initialization

    private void Start()
    {
        // Start function
    }
    #endregion

    #region Core
    protected override void Use()
    {
        if (currentHandler)
        {
            WeaponHandler handler = currentHandler.PlayerController.GetComponent<WeaponHandler>();
            if (handler)
            {
                handler.IdWeapon = NewWeaponId;
            }
        }

    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {

        }
    }

    #endregion
}
