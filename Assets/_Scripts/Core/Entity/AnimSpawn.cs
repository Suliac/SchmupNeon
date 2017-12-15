using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// AnimSpawn Description
/// </summary>
public class AnimSpawn : MonoBehaviour
{
    #region Attributes
    private SpawnPlayer spawnPlayer;    //reference du spawnPlayer
    public SpawnPlayer SpawnPlayer { set { spawnPlayer = value; } } //permet de set la référence seulement

    #endregion

    #region Initialization

    #endregion

    #region Core
    /// <summary>
    /// Fonction appelé par l'animator de cet objet
    /// l'animation est terminé, exécuter une action...
    /// </summary>
    public void animOver()
    {
        spawnPlayer.spawnIt();
    }
    #endregion

    #region Unity ending functions

	#endregion
}
