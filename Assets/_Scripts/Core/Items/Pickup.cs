using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// IPickup Description
/// </summary>
public abstract class Pickup : MonoBehaviour, IKillable
{
    #region Attributes
    [FoldoutGroup("Debug")]
    [Tooltip("ref sur PickupHandler")]
    protected PickupHandler currentHandler;
    public PickupHandler CurrentHandler { get { return currentHandler; } set { currentHandler = value; } }

    [FoldoutGroup("Gameplay"), Tooltip("Destruction du pickup juste après utilisation"), SerializeField]
    private bool killOnUse = false;

    #endregion

    #region Core
    /// <summary>
    /// Applique l'effet de l'objet
    /// </summary>
    protected abstract void Use();

    public void TryUse()
    {
        Use();
        if (killOnUse)
            Stop();
    }

    protected void Stop()
    {
        Kill();
        currentHandler = null;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    #endregion

}
