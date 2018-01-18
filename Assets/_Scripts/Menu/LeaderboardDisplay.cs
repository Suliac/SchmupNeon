using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.SceneManagement;

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
    private bool isQuiting = false;

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

            string name = indexChild >= leaderboard.highscoresList.Length ? "AAA" : leaderboard.highscoresList[indexChild].username.Substring(0, 3).ToUpper();
            string score = indexChild >= leaderboard.highscoresList.Length ? "00000000" : leaderboard.highscoresList[indexChild].score.ToString("00000000");

            textGUI.text = (indexChild + 1) + " : " + name + " - " + score;
            indexChild++;
        }
    }

    private void InputMenu()
    {
        if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB") && !isQuiting)
        {
            isQuiting = true;
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
