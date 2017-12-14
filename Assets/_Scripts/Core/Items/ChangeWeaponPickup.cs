using UnityEngine;

/// <summary>
/// ChangeWeaponPickup Description
/// </summary>
public class ChangeWeaponPickup : Pickup
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    

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
        throw new System.NotImplementedException(); // TODO : changer l'arme du currentHandler
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
