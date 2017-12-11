using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// ScoreManager Description
/// </summary>
public class ScoreManager : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("Object In World"), Tooltip("canvas des players"), SerializeField]
    private List<TextMeshProUGUI> canvasPlayer;
    public List<TextMeshProUGUI> CanvasPlayer { get { return canvasPlayer; } }

    [FoldoutGroup("Debug"), Tooltip("Sauvegarde du joueur"), SerializeField]
    private PlayerData data = new PlayerData();
    public PlayerData Data { get { return data; } }
    #endregion

    #region Initialization
    /// <summary>
    /// lors du start: load les données, sinon, sauvegarder !
    /// </summary>
    private void Start()
    {
        if (!load())
            resetAll();
        SetupTextScore();
    }
    #endregion

    #region Core
    /// <summary>
    /// setup els score dans le canvas
    /// </summary>
    private void SetupTextScore()
    {
        for (int i = 0; i < canvasPlayer.Count; i++)
        {
            canvasPlayer[i].text = data.scorePlayer[i].ToString();
        }
    }

    /// <summary>
    /// set le score à un joueur, et actualise
    /// </summary>
    public void setScore(int idPlayer, int score)
    {
        data.scorePlayer[idPlayer] = score;
        canvasPlayer[idPlayer].text = score.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    private void resetAll()
    {
        Debug.Log("reset les stats du joueurs");
        if (data == null)
            data = new PlayerData();
        data.SetDefault();

        save();
    }


    /// <summary>
    /// renvoi VRAI si ça à loadé !
    /// </summary>
    /// <returns></returns>
    [FoldoutGroup("Debug"), Button("load")]
    public bool load()
    {
        data = DataSaver.Load<PlayerData>("playerData.dat");
        if (data == null)
            return (false);
        return (!(data == null));
    }


    [FoldoutGroup("Debug"), Button("save")]
    public void save()
    {
        DataSaver.Save(data);
    }

    [FoldoutGroup("Debug"), Button("delete")]
    public void delete()
    {
        DataSaver.DeleteSave("playerData.dat");
    }
    #endregion

    #region Unity ending functions

    #endregion
}
