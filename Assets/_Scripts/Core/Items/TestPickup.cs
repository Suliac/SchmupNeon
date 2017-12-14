using UnityEngine;

/// <summary>
/// TestPickup Description
/// </summary>
public class TestPickup : HandablePickup
{
    #region Attributes

    #endregion

    #region Initialization
    #endregion

    #region Core
    protected override void Use()
    {
        IKillable killableEntity = currentHandler.gameObject.GetComponent<IKillable>();
        if (killableEntity != null)
        {
            killableEntity.Kill();
        }
    }
    #endregion

    #region Unity ending functions

    #endregion

}
