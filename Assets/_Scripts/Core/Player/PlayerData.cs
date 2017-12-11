using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : PersistantData
{
	#region Attributes

	[Tooltip("score des 4 players")]
    public List<int> scorePlayer = new List<int>();
    #endregion


    #region Core
    /// <summary>
    /// reset toute les valeurs à celle d'origine pour le jeu
    /// </summary>
    public void SetDefault()
    {
        scorePlayer.Clear();
        scorePlayer.Add(0);
        scorePlayer.Add(0);
        scorePlayer.Add(0);
        scorePlayer.Add(0);
    }

    public override string GetFilePath ()
	{
		return "playerData.dat";
	}

	#endregion
}