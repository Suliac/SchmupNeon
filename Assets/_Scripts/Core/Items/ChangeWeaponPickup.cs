using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// ChangeWeaponPickup Description
/// </summary>
public class ChangeWeaponPickup : Pickup
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Gameplay"), Tooltip("ref sur HandablePickup"), SerializeField]
    private int NewWeaponId = 0;

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
        print("BITCH ?");
        if (currentHandler)
        {
            print("BITCH ARE YOU HERE ?");
            WeaponHandler handler = currentHandler.PlayerController.GetComponent<WeaponHandler>();
            if (handler)
            {
                print("BITCH");
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
