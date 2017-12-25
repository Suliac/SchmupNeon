using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiWinReady : MonoBehaviour {

	public void OnEndAnimationPlayerWin()
    {
        GameManager.GetSingleton.WinManager.PlayerUIWinReady();
    }
}
