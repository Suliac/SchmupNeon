using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ItemManager Description
/// </summary>
public class ItemManager : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Object In World"), Tooltip("canvas des players"), SerializeField]
    private List<Image> canvasPlayer;
    public List<Image> CanvasPlayer { get { return canvasPlayer; } }

    [FoldoutGroup("Gameplay")][Tooltip("Sprite affiché dans l'UI quand le joueur n'a pas d'objet")][SerializeField]
    private Sprite defaultObjectSpriteInUI;
    public Sprite DefaultObjectSpriteInUI { get { return defaultObjectSpriteInUI; } }

    #endregion

    #region Initialization
    private void Start()
    {
        SetupItemImages();
    }
    #endregion

    #region Core
    private void SetupItemImages()
    {
        for (int i = 0; i < canvasPlayer.Count; i++)
        {
            canvasPlayer[i].sprite = defaultObjectSpriteInUI;
        }
    }

    public void SetItem(int idPlayer, Sprite itemSprite)
    {
        canvasPlayer[idPlayer].sprite = itemSprite;
    }

    public void ResetItem(int idPlayer)
    {
        canvasPlayer[idPlayer].sprite = defaultObjectSpriteInUI;
    }

    public void ResetAll()
    {
        for (int i = 0; i < canvasPlayer.Count; i++)
        {
            canvasPlayer[i].sprite = defaultObjectSpriteInUI;
        }
    }
    #endregion

    #region Unity ending functions


    #endregion
}
