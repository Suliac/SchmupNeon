using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// ScoreManager Description
/// </summary>
public class ScoreManager : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("Object In World"), Tooltip("canvas des players"), SerializeField]
    private List<Text> canvasPlayer;
    public List<Text> CanvasPlayer { get { return canvasPlayer; } }

    //[FoldoutGroup("Object In World"), Tooltip("canvas de victoire des scores players"), SerializeField]
    //private List<Text> canvasVictoryPlayer;
    //public List<Text> CanvasVictoryPlayer { get { return canvasVictoryPlayer; } }

    [FoldoutGroup("Object In World"), Tooltip("canvas contenant le place holder prevenant d'un highscore"), SerializeField]
    private List<Text> canvasIsHighScoreVictoryPlayer;
    public List<Text> CanvasIsHighScoreVictoryPlayer { get { return canvasIsHighScoreVictoryPlayer; } }

    [FoldoutGroup("Object In World"), Tooltip("canvas contenant le place holder des lettres d'un joueur"), SerializeField]
    private List<GameObject> canvasLettersVictoryPlayer;
    public List<GameObject> CanvasLettersVictoryPlayer { get { return canvasLettersVictoryPlayer; } }

    [FoldoutGroup("Debug"), Tooltip("Sauvegarde du joueur"), SerializeField]
    private PlayerData data = new PlayerData();
    public PlayerData Data { get { return data; } }

    private bool[] isPlayerEnteringName;
    public bool[] IsPlayerEnteringName { get { return isPlayerEnteringName; } }
    #endregion

    #region Initialization
    /// <summary>
    /// lors du start: load les données, sinon, sauvegarder !
    /// </summary>
    private void Start()
    {
        //if (!load())
        ResetAll();
        SetupTextScore();
        isPlayerEnteringName = new bool[PlayerConnected.GetSingleton.PlayerNumber];
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
            canvasPlayer[i].text = data.scorePlayer[i].ToString("00000000");
        }
        
    }

    /// <summary>
    /// set le score à un joueur, et actualise
    /// </summary>
    public void SetScore(int idPlayer, int score)
    {
        data.scorePlayer[idPlayer] = score;
        canvasPlayer[idPlayer].text = score.ToString("00000000");
    }

    //public void SetVictoryScores()
    //{
    //    for (int i = 0; i < canvasVictoryPlayer.Count; i++)
    //    {
    //        canvasVictoryPlayer[i].text = canvasPlayer[i].text;

    //        bool isHighScore = PlayerConnected.GetSingleton.playerArrayConnected[i]; // TODO : tester si highscore

    //        canvasIsHighScoreVictoryPlayer[i].enabled = isHighScore;
    //        canvasLettersVictoryPlayer[i].SetActive(isHighScore);
    //        //isPlayerEnteringName[i] = isHighScore;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public void ResetAll()
    {
        //Debug.Log("reset les stats du joueurs");
        if (data == null)
            data = new PlayerData();
        data.SetDefault();

        Save();
    }


    /// <summary>
    /// renvoi VRAI si ça à loadé !
    /// </summary>
    /// <returns></returns>
    [FoldoutGroup("Debug"), Button("load")]
    public bool Load()
    {
        data = DataSaver.Load<PlayerData>("playerData.dat");
        if (data == null)
            return (false);
        return (!(data == null));
    }


    [FoldoutGroup("Debug"), Button("save")]
    public void Save()
    {
        DataSaver.Save(data);
    }

    [FoldoutGroup("Debug"), Button("delete")]
    public void Delete()
    {
        DataSaver.DeleteSave("playerData.dat");
    }
    #endregion

    #region Unity ending functions

    #endregion
}
