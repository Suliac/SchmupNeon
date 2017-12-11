using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// GameManager Description
/// </summary>
public class GameManager : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("Sauvegarde du joueur"), SerializeField]
    public PlayerData data;

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
    private Transform objectDynamiclyCreated;
    public Transform ObjectDynamiclyCreated { get { return objectDynamiclyCreated; } }

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
        if (!objectDynamiclyCreated)
            Debug.LogError("error");
    }

    /// <summary>
    /// lors du start: load les données, sinon, sauvegarder !
    /// </summary>
    private void Start()
    {
        if (!load())
            resetAll();
    }
    #endregion

    #region Core

    /// <summary>
    /// 
    /// </summary>
    public void resetAll()
    {
        Debug.Log("reset les stats du joueurs");
        data.scorePlayer = 0;
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
