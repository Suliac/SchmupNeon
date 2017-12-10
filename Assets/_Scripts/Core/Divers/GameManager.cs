using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// GameManager Description
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetSingleton
    {
        get { return instance; }
    }

    #region Attributes
    [FoldoutGroup("Object In World"), Tooltip("Spawn des players"), SerializeField]
    private List<SpawnPlayer> spawnPlayer;
    public List<SpawnPlayer> SpawnPlayer { get { return spawnPlayer; } }

    [FoldoutGroup("Object In World"), Tooltip("Platform mouvante"), SerializeField]
    private Transform movingPlatform;
    public Transform MovingPlatform { get { return movingPlatform; } }

    [FoldoutGroup("Debug"), Tooltip("Mouvement du joueur"), SerializeField]
    private GameObject prefabsPlayer;

    [SerializeField]
	private FrequencyTimer updateTimer;

    private PlayerConnected playerConnect;
    public PlayerConnected PlayerConnect { get { return playerConnect; } }

    #endregion

    #region Initialization

    public void SetSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Awake()
    {
        SetSingleton();
        playerConnect = PlayerConnected.GetSingleton;
    }
    #endregion

    #region Core
    /// <summary>
    /// ici gère la déconexion des manettes
    /// (si une manette se connecte, le spawnPlayer va automatiquement
    ///     spawner le joueur à la fin de l'animation de spawn)
    /// </summary>
    public void updateJoypadDisconnect(int id, bool connected)
    {
        if (!connected)
            spawnPlayer[id].PlayerController.Kill();
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {

    }

	#endregion
}
