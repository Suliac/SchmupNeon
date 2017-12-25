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

    [FoldoutGroup("GamePlay"), Tooltip("prefabs quand on prend le pickup"), SerializeField]
    protected string prefabsTakePickup;

    [FoldoutGroup("GamePlay"), Tooltip("prefabs quand on active le pickup"), SerializeField]
    protected string prefabsUsePickup;

    [FoldoutGroup("GamePlay"), Tooltip("Destruction du pickup juste après utilisation"), SerializeField]
    private bool killOnUse = false;

    #endregion

    #region Core
    /// <summary>
    /// Applique l'effet de l'objet
    /// </summary>
    protected abstract void Use();

    /// <summary>
    /// ici est appelé lorsque l'on prend le pickup
    /// </summary>
    protected void CreateTakeObject(Transform refObject, string tagParticle, PickupHandler handler = null)
    {
        GameObject deathTake = ObjectsPooler.GetSingleton.GetPooledObject(tagParticle, false);
        if (!deathTake)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du player");
            return;
        }
        deathTake.transform.position = refObject.position;
        deathTake.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);

        if (handler != null)
        {
            SpriteRenderer renderer = deathTake.GetComponent<SpriteRenderer>();
            PlayerController controller = handler.GetComponent<PlayerController>();

            if (renderer && controller)
                renderer.color = controller.ColorPlayer; 
        }

        deathTake.SetActive(true);
    }

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
