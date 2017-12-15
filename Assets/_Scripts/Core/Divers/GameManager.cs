using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// GameManager Description
/// </summary>
[RequireComponent(typeof(TutoStart))]    //tuto !!
[RequireComponent(typeof(ScoreManager))]    //le scoreManager doit être accroché à l'objet
[RequireComponent(typeof(ItemManager))]    //item manager
public class GameManager : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Object In World"), Tooltip("Spawn des players"), SerializeField]
    private List<SpawnPlayer> spawnPlayer;
    public List<SpawnPlayer> SpawnPlayer { get { return spawnPlayer; } }

    [FoldoutGroup("Object In World"), Tooltip("Platform mouvante"), SerializeField]
    private Transform objectDynamiclyCreated;
    public Transform ObjectDynamiclyCreated { get { return objectDynamiclyCreated; } }

    [FoldoutGroup("Object In World"), Tooltip("movingPlatform"), SerializeField]
    private MovePlatform movingPlatform;

    [FoldoutGroup("Object In World"), Tooltip("panel Canvas des joueurs (in game)"), SerializeField]
    private GameObject panelCanvasInGame;

    [FoldoutGroup("Debug"), Tooltip("Mouvement du joueur"), SerializeField]
    private GameObject prefabsPlayer;

    
    //public MovePlatform MovingPlatform { get { return movingPlatform; } }

    [FoldoutGroup("Debug"), Tooltip("optimisation fps"), SerializeField]
	private FrequencyTimer updateTimer;

    //ref player
    private PlayerConnected playerConnect;
    public PlayerConnected PlayerConnect { get { return playerConnect; } }

    private ScoreManager scoreManager;  //reference du scoreManager;
    public ScoreManager ScoreManager { get { return scoreManager; } }

    private ItemManager itemManager;  //reference de l'itemManager;
    public ItemManager ItemManager { get { return itemManager; } }

    private TutoStart tutoStart;  //reference de l'itemManager;
    public TutoStart TutoStart { get { return tutoStart; } }

    //singleton
    private static GameManager instance;
    public static GameManager GetSingleton
    {
        get { return instance; }
    }

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
        tutoStart = GetComponent<TutoStart>();
        scoreManager = GetComponent<ScoreManager>();
        itemManager = GetComponent<ItemManager>();
        if (!objectDynamiclyCreated)
            Debug.LogError("error");
    }

    private void Start()
    {
        ActiveGame(false);  //desactive tout au start au cas ou
        tutoStart.ActiveTuto(true); //active les tutos
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
        if (!connected && spawnPlayer[id] && spawnPlayer[id].PlayerController)
            spawnPlayer[id].PlayerController.Kill();
    }

    /// <summary>
    /// quite le jeu ?
    /// </summary>
    private void Quit()
    {
        if (PlayerConnected.GetSingleton.getPlayer(-1).GetButtonDown("Escape")
            || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("Start"))
        {
            scoreManager.save();
            SceneChangeManager.GetSingleton.Quit();
        }
    }

    [FoldoutGroup("Debug")]
    [Button("ActiveGame")]
    public void ActiveGame() { ActiveGame(true); }

    public void ActiveGame(bool active)
    {
        panelCanvasInGame.SetActive(active);                //active le canvas des scores
        movingPlatform.IsScrollingAcrtive = active;         //active la platforme mouvante
        if (!active)
        {
            for (int i = 0; i < spawnPlayer.Count; i++)
            {
                spawnPlayer[i].gameObject.SetActive(active);    //active les spawn des players
            }
        }
        else
        {
            tutoStart.ActiveTuto(false);
            tutoStart.enabled = false;
        }
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (updateTimer.Ready())
        {

        }

        Quit(); //input quitter
        
    }

	#endregion
}
