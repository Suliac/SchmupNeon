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
    
    #endregion

    #region Initialization
    private void Start()
    {
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
        canvasPlayer[idPlayer].sprite = null;
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
