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
    
    private Sprite[] defaultObjectSpriteInUI;

    #endregion

    #region Initialization
    private void Start()
    {
        defaultObjectSpriteInUI = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            defaultObjectSpriteInUI[i] = canvasPlayer[i].sprite;
        }

        ResetAll();
    }
    #endregion

    #region Core
    private void SetupItemImages()
    {
        ResetAll();
    }

    public void SetItem(int idPlayer, Sprite itemSprite)
    {
        canvasPlayer[idPlayer].color = new Color(255f, 255f, 255f, 1.0f);
        canvasPlayer[idPlayer].sprite = itemSprite;
    }

    public void ResetItem(int idPlayer)
    {
        canvasPlayer[idPlayer].color = new Color(255f, 255f, 255f, 0f);
        canvasPlayer[idPlayer].sprite = defaultObjectSpriteInUI[idPlayer];
    }

    public void ResetAll()
    {
        for (int i = 0; i < canvasPlayer.Count; i++)
        {
            ResetItem(i);
        }
    }
    #endregion

    #region Unity ending functions


    #endregion
}
