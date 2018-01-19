using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Rewired;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager Description
/// </summary>
[RequireComponent(typeof(WinManager))]
[RequireComponent(typeof(TutoStart))]    //tuto !!
[RequireComponent(typeof(ScoreManager))]    // le scoreManager doit être accroché à l'objet
[RequireComponent(typeof(ItemManager))]    // item manager
public class GameManager : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Object In World"), Tooltip("Spawn des players"), SerializeField]
    private List<SpawnPlayer> spawnPlayer;
    public List<SpawnPlayer> SpawnPlayer { get { return spawnPlayer; } }

    [FoldoutGroup("Object In World"), Tooltip("Entitées créées dynamiquement mouvantes"), SerializeField]
    private Transform objectDynamiclyCreated;
    public Transform ObjectDynamiclyCreated { get { return objectDynamiclyCreated; } }

    [FoldoutGroup("Object In World"), Tooltip("Entitées créées dynamiquement fixes"), SerializeField]
    private Transform objectNotMovingDynamiclyCreated;
    public Transform ObjectNotMovingDynamiclyCreated { get { return objectNotMovingDynamiclyCreated; } }

    [FoldoutGroup("Object In World"), Tooltip("Group des Ennemy"), SerializeField]
    private Transform enemyGroup;
    public Transform EnemyGroup { get { return enemyGroup; } }


    [FoldoutGroup("Object In World"), Tooltip("movingPlatform"), SerializeField]
    private MovePlatform movingPlatform;

    [FoldoutGroup("Object In World"), Tooltip("panel Canvas des joueurs (in game)"), SerializeField]
    private GameObject panelCanvasInGame;

    [FoldoutGroup("Object In World"), Tooltip("panel Canvas de gameover"), SerializeField]
    private GameObject panelCanvasGameOver;

    [FoldoutGroup("Debug"), Tooltip("Mouvement du joueur"), SerializeField]
    private GameObject prefabsPlayer;

    [FoldoutGroup("Debug"), Tooltip("Désactive le gameover & victoire"), SerializeField]
    private bool desactivateGameOverAndVictory = false;

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

    private WinManager winManager;
    public WinManager WinManager { get { return winManager; } }

    private PlayerController[] playerControllers;
    public PlayerController[] PlayerControllers { get { return playerControllers; } }

    private StateManager.GameState stateBeforePause;
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
        playerControllers = new PlayerController[4];
        playerConnect = PlayerConnected.GetSingleton;
        tutoStart = GetComponent<TutoStart>();
        winManager = GetComponent<WinManager>();
        scoreManager = GetComponent<ScoreManager>();
        itemManager = GetComponent<ItemManager>();
        if (!objectDynamiclyCreated)
            Debug.LogError("error");
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        StateManager.GetSingleton.State = StateManager.GameState.Tuto;
        SoundManager.GetSingularity.PlayGameMusic();

        ActiveGame(false);  //desactive tout au start au cas ou
        tutoStart.ActiveTuto(true); //active les tutos
        panelCanvasGameOver.SetActive(false);

        panelCanvasGameOver.SetActive(false);
        scoreManager.ResetAll();
        itemManager.ResetAll();


    }

    #endregion

    #region Core
    public void AddPlayerController(PlayerController playerController, int idPlayer)
    {
        playerControllers[idPlayer] = playerController;
    }

    public void RemovePlayerController(int idPlayer)
    {
        playerControllers[idPlayer] = null;
    }

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
            //scoreManager.Save();
            SceneChangeManager.GetSingleton.JumpToSceneWithFade("1_Menu");
        }
    }

    [FoldoutGroup("Debug")]
    [Button("ActiveGame")]
    public void ActiveGame() { ActiveGame(true); }

    public void ActiveGame(bool active)
    {
        panelCanvasInGame.SetActive(true);                //active le canvas des scores
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
            StateManager.GetSingleton.State = StateManager.GameState.Play;

            tutoStart.ActiveTuto(false);
            tutoStart.enabled = false;
        }
    }

    public void IsGameOver()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Play)
        {
            int countDeadPlayer = 0;
            foreach (var player in spawnPlayer)
            {
                if (player.PlayerController)
                {
                    LifeBehavior life = player.PlayerController.GetComponent<LifeBehavior>();

                    if (!life || life.CurrentLife <= 0)
                        countDeadPlayer++;
                }
                else
                {
                    countDeadPlayer++;
                }
            }

            if (countDeadPlayer >= spawnPlayer.Count)
                GameOver();

        }

    }

    private void GameOver()
    {
        SoundManager.GetSingularity.CleanProjectileSound();
        
        StateManager.GetSingleton.State = StateManager.GameState.GameOver;
        panelCanvasGameOver.SetActive(true);
        panelCanvasInGame.SetActive(false);
        movingPlatform.IsScrollingAcrtive = false;
    }

    private void InputGameOver()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.GameOver)
        {
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireA"))
            {
                StateManager.GetSingleton.State = StateManager.GameState.Tuto;
                SceneChangeManager.GetSingleton.JumpToSceneWithFade();
            }
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB"))
            {
                //Init();
                StateManager.GetSingleton.State = StateManager.GameState.Menu;
                SceneChangeManager.GetSingleton.JumpToSceneWithFade("1_Menu");
            }
        }
    }

    private void InputPause()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Pause)
        {
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireA"))
            {
                Resume();
            }
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB"))
            {
                StateManager.GetSingleton.State = StateManager.GameState.Menu;
                SceneChangeManager.GetSingleton.JumpToSceneWithFade("1_Menu");
            }
        }
    }

    public void OnWin()
    {
        panelCanvasInGame.SetActive(false);
        movingPlatform.IsScrollingAcrtive = false;
    }

    public void Pause()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Tuto || StateManager.GetSingleton.State == StateManager.GameState.Play)
        {
            movingPlatform.IsScrollingAcrtive = false;
            stateBeforePause = StateManager.GetSingleton.State;
            StateManager.GetSingleton.State = StateManager.GameState.Pause;
            Pausable[] pausableObjects = GameObject.FindObjectsOfType<Pausable>();
            foreach (var pausable in pausableObjects)
            {
                pausable.Pause();
            }
        }
    }

    public void Resume()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Pause)
        {
            movingPlatform.IsScrollingAcrtive = true;
            StateManager.GetSingleton.State = stateBeforePause;
            Pausable[] pausableObjects = GameObject.FindObjectsOfType<Pausable>();
            foreach (var pausable in pausableObjects)
            {
                pausable.Resume();
            }
        }
    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (updateTimer.Ready())
        {
        }

        if (!desactivateGameOverAndVictory && StateManager.GetSingleton.State < StateManager.GameState.Pause)
        {
            IsGameOver();
            winManager.IsVictory();
        }
        else if (StateManager.GetSingleton.State == StateManager.GameState.Pause)
        {
            InputPause();
        }

        InputGameOver();
        Quit(); //input quitter

    }

    #endregion
}
