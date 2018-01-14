using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Leaderboard Description
/// </summary>
public class Leaderboard : MonoBehaviour
{
    #region Attributes

	[FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    #endregion

    #region Initialization

    private void Start()
    {
		// Start function
    }
    #endregion

    #region Core
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

        }
    }

	#endregion
}
