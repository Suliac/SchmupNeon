﻿using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Rewired;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// GameManager Description
/// </summary>
[RequireComponent(typeof(TutoStart))]    //tuto !!
[RequireComponent(typeof(ScoreManager))]    // le scoreManager doit être accroché à l'objet
[RequireComponent(typeof(ItemManager))]    // item manager
[RequireComponent(typeof(StateManager))]    // state
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

    [FoldoutGroup("Object In World"), Tooltip("panel canvas de victoire"), SerializeField]
    private GameObject panelCanvasVictory;

    [FoldoutGroup("Debug"), Tooltip("Mouvement du joueur"), SerializeField]
    private GameObject prefabsPlayer;

    [FoldoutGroup("Debug"), Tooltip("Nombre d'ennemis dans le niveau")]
    private int numberEnemyInLevel = 0;

    [FoldoutGroup("Debug"), Tooltip("Nombre d'ennemis tués")]
    private int currentEnemiesKilled = 0;

    [FoldoutGroup("Debug"), Tooltip("Désactive le gameover & victoire"), SerializeField]
    private bool desactivateGameOverAndVictory = false;
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
        Init();   
    }

    private void Init()
    {
        //print("yo");
        StateManager.Get.State = StateManager.GameState.Tuto;
        SoundManager.GetSingularity.PlaySound("Stop_Menu");
        SoundManager.GetSingularity.PlaySound("Play_ingame");

        var allEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
        numberEnemyInLevel = allEnemies.Length;
        print("enemies = "+string.Join(" ", allEnemies.Select(e => e.name).ToArray()));
        currentEnemiesKilled = 0;

        ActiveGame(false);  //desactive tout au start au cas ou
        tutoStart.ActiveTuto(true); //active les tutos
        panelCanvasGameOver.SetActive(false);
        panelCanvasVictory.SetActive(false);

        panelCanvasGameOver.SetActive(false);
        scoreManager.ResetAll();
        itemManager.ResetAll();
        

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
            scoreManager.Save();
            SceneChangeManager.GetSingleton.Quit();
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
            StateManager.Get.State = StateManager.GameState.Play;

            tutoStart.ActiveTuto(false);
            tutoStart.enabled = false;
        }
    }

    public void IsGameOver()
    {
        if (StateManager.Get.State == StateManager.GameState.Play)
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
        StateManager.Get.State = StateManager.GameState.GameOver;
        panelCanvasGameOver.SetActive(true);
        panelCanvasInGame.SetActive(false);
        movingPlatform.IsScrollingAcrtive = false;
    }

    private void InputGameOver()
    {
        if (StateManager.Get.State == StateManager.GameState.GameOver)
        {
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireA"))
            {
                ////Init();
                //StateManager.Get.State = StateManager.GameState.Tuto;
                //SceneManager.LoadScene("2_Game");
            }
            if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB"))
            {
                //Init();
                StateManager.Get.State = StateManager.GameState.Menu;
                SceneManager.LoadScene("1_Menu");
            }
        }
    }

    public void NewEnemyKill()
    {
        lock (this)
        {
            currentEnemiesKilled++;
        }
    }

    public void IsVictory()
    {
        if (StateManager.Get.State == StateManager.GameState.Play)
        {
            if (currentEnemiesKilled >= numberEnemyInLevel)
                Victory();

        }

    }

    private void Victory()
    {
        StateManager.Get.State = StateManager.GameState.Victory;
        panelCanvasVictory.SetActive(true);
        panelCanvasInGame.SetActive(false);
        movingPlatform.IsScrollingAcrtive = false;
        scoreManager.SetVictoryScores();
    }

    private void InputVictory()
    {
        if (StateManager.Get.State == StateManager.GameState.Victory)
        {
            if (!scoreManager.IsPlayerEnteringName.Any(entering => entering)) // Si aucun des joueurs n'est en train de changer son nom
            {
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireA"))
                {
                    ////Init();
                    //StateManager.Get.State = StateManager.GameState.Tuto;
                    //SceneManager.LoadScene("2_Game");
                }
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB"))
                {
                    //Init();
                    StateManager.Get.State = StateManager.GameState.Menu;
                    SceneManager.LoadScene("1_Menu");
                } 
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

        if (!desactivateGameOverAndVictory)
        {
            IsGameOver();
            IsVictory();
        }

        InputVictory();
        InputGameOver();
        Quit(); //input quitter

    }

    #endregion
}
