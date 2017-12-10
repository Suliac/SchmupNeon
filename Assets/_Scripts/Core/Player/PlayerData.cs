using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : PersistantData
{
	#region Attributes

	[Tooltip("La progression du joueur en mode solo: l'id de la dernière map débloqué")]
	public int scorePlayer;

	#endregion

	#region Core

	public override string GetFilePath ()
	{
		return "playerData.dat";
	}

	#endregion
}