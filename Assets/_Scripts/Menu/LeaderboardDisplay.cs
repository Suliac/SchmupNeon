using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

/// <summary>
/// LeaderboardDisplay Description
/// </summary>
public class LeaderboardDisplay : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("Debug"), Tooltip("objet contenant les scores"), SerializeField]
    private Transform grid;
    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;
    private bool displayed = false;

    private Leaderboard leaderboard;
    #endregion

    #region Initialization

    private void Start()
    {
        leaderboard = Leaderboard.GetSingleton;
    }
    #endregion

    #region Core
    /// <summary>
    /// affiche le leaderboard par rapport aux donné online sauvegardé dans highscoresList de la class Leaderboard
    /// fonction appelé après que DownloadHighscoresFromDatabase de Leaderboard ai loadé toutes les infos
    /// </summary>
    public void SetupLeaderboard()
    {
        int indexChild = 0;
        foreach (Transform child in grid)
        {
            //Something(child.gameObject);
            TextMeshProUGUI textGUI = child.GetComponent<TextMeshProUGUI>();

            //s'il n'y a plus de donné, afficher empty
            if (indexChild >= leaderboard.highscoresList.Length)
                textGUI.text = (indexChild + 1) + ": AAA - 00000000";
            else
                textGUI.text = (indexChild + 1) + ": " + leaderboard.highscoresList[indexChild].username + " - " + leaderboard.highscoresList[indexChild].score;

            indexChild++;
        }
    }

    private void InputMenu()
    {
        if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
        {
             SceneChangeManager.GetSingleton.JumpToSceneWithFade("1_Menu");
        }
    }
#endregion

#region Unity ending functions
    private void Update()
    {
        InputMenu();
        //optimisation des fps
        if (updateTimer.Ready())
        {
            if (leaderboard.uploadedScore && !displayed)
            {
                SetupLeaderboard();
                displayed = true;
            }
        }
    }
#endregion
}
