using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HandablePickup Description
/// </summary>
public abstract class HandablePickup : Pickup
{
    #region Attributes
    [FoldoutGroup("Gameplay")][Tooltip("Sprite affiché dans l'UI quand le joueur récupère l'objet")][SerializeField]
    private Sprite objectSpriteInUI;
    public Sprite ObjectSpriteInUI { get { return objectSpriteInUI; } }

    [FoldoutGroup("Debug")][Tooltip("Est ce que ce pickup a été récupéré")]
    private bool isAlreadyPicked = false;
    public bool IsAlreadyPicked {  get { return isAlreadyPicked; } }

    private Renderer renderer;
    #endregion

    #region Initialization
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        isAlreadyPicked = false;
        renderer.enabled = true;
    }

    #endregion

    #region Core

    /// <summary>
    /// Cache le pickup après l'avoir récupéré. 
    /// ATTENTION : L'OBJET N'EST PAS SUPPRIME POUR POUVOIR ACCEDER A SON USE()
    /// </summary>
    protected void Hide()
    {
        renderer.enabled = false;
    }

    public void Pick(PickupHandler handler)
    {
        Hide();
        currentHandler = handler;
    }
    
    #endregion

}
