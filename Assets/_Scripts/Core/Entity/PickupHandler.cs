using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// PickupHandler Description
/// </summary>
public class PickupHandler : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Debug"), Tooltip("ref sur HandablePickup")]
    private HandablePickup currentPickup;

    [FoldoutGroup("Debug"), Tooltip("ref sur PlayerController")]
    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } }
    #endregion

    #region Initialization
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Init()
    {
        if (currentPickup)
            currentPickup.Kill();

        currentPickup = null;

        if (playerController)
            GameManager.GetSingleton.ItemManager.ResetItem(playerController.IdPlayer);

    }

    #endregion

    #region Core

    public void UseItem()
    {
        if (currentPickup)
        {
            currentPickup.TryUse();
            currentPickup = null;

            if(PlayerController)
            {
                GameObject pickGO = playerController.AnimPick;
                if (pickGO)
                {
                    if (pickGO.activeInHierarchy)
                    {
                        pickGO.SetActive(false);
                        SpriteRenderer renderer = pickGO.GetComponent<SpriteRenderer>();
                        if(renderer)
                        {
                            renderer.sprite = null;
                        }
                    }
                    
                }
            }

            if (playerController)
                GameManager.GetSingleton.ItemManager.ResetItem(playerController.IdPlayer);
        }
    }
    #endregion

    #region Unity ending functions

    private void OnTriggerEnter(Collider col)
    {
        if (StateManager.GetSingleton.State >= StateManager.GameState.GameOver)
            return;

        HandablePickup handablePickup = col.GetComponent<HandablePickup>();
        if (handablePickup && !handablePickup.IsAlreadyPicked)
        {
            currentPickup = handablePickup;
            handablePickup.Pick(this);

            if (playerController)
            {
                GameObject pickGO = playerController.AnimPick;
                if (pickGO)
                {
                    if (pickGO.activeInHierarchy)
                        pickGO.SetActive(false);

                    pickGO.SetActive(true);

                    Animator anim = pickGO.GetComponent<Animator>();
                    if (anim)
                    {
                        anim.SetTrigger("Pick");
                    }
                }

                GameManager.GetSingleton.ItemManager.SetItem(playerController.IdPlayer, handablePickup.ObjectSpriteInUI);
                SoundManager.GetSingularity.PlayPickupSound(playerController.IdPlayer);
            }
        }
        else
        {
            Pickup instantPickup = col.GetComponent<Pickup>(); // Si pikcup n'est pas Handable mais un pickup quand meme -> on l'utilise de suite

            if (instantPickup != null)
            {
                if (playerController)
                    SoundManager.GetSingularity.PlayPickupSound(playerController.IdPlayer);

                instantPickup.CurrentHandler = this;
                instantPickup.TryUse();
            }
        }

    }

    #endregion
}
