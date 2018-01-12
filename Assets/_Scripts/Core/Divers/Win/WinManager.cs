using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum VictoryStates
{
    NotWin,
    PlayerComingOut,
    DisplayUI,
    PlayerUIComing,
    DisplayScore,
    DisplayWinner,
    EnteringNames,
    Ready
}

[RequireComponent(typeof(GameManager))]
public class WinManager : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Object In World"), Tooltip("panel Win"), SerializeField]
    private GameObject panelWin;

    [FoldoutGroup("Object In World"), Tooltip("Joueurs dans l'UI a afficher depuis la gauche"), SerializeField]
    private List<GameObject> playerUiWin;

    [FoldoutGroup("Object In World"), Tooltip("Nom des joueurs dans l'UI "), SerializeField]
    private List<GameObject> playerNameInUI;

    [FoldoutGroup("Object In World"), Tooltip("Lettres des joueurs dans l'UI "), SerializeField]
    private List<GameObject> playerLettersInUI;

    [FoldoutGroup("Object In World"), Tooltip("Score des joueurs dans l'UI "), SerializeField]
    private List<Text> playerScoreInUI;

    [FoldoutGroup("Debug"), Tooltip("Nombre d'ennemis dans le niveau")]
    private int numberEnemyInLevel = 0;

    [FoldoutGroup("Debug"), Tooltip("Nombre d'ennemis tués")]
    private int currentEnemiesKilled = 0;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;
    [FoldoutGroup("Debug"), Tooltip("Etat de la gestion de victoire")]
    private VictoryStates currentState;
    private VictoryStates lastState;

    private GameManager gameManager;
    private LifeBehavior[] playersLife;
    private bool enableWin = true;
    private int numberPlayers = 4;

    private int playerUIReady = 0;
    private int scoreUIReady = 0;

    private bool coroutineLaunched = false;
    private bool coroutineScoreLaunched = false;
    #endregion

    #region Initialization

    private void Awake()
    {
        panelWin.SetActive(false);
        for (int i = 0; i < playerNameInUI.Count; i++)
        {
            playerNameInUI[i].SetActive(false);
        }

        for (int i = 0; i < playerScoreInUI.Count; i++)
        {
            playerScoreInUI[i].enabled = false;
        }

        foreach (var playerInUI in playerUiWin)
        {
            playerInUI.SetActive(false);
        }

        foreach (var playerLetters in playerLettersInUI)
        {
            playerLetters.SetActive(false);
        }

        gameManager = GetComponent<GameManager>();
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {

        currentState = VictoryStates.NotWin;
        BaseEnemy[] allEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
        numberEnemyInLevel = allEnemies.Length;
        currentEnemiesKilled = 0;
        //print("enemies = " + string.Join(" ", allEnemies.Select(e => e.name).ToArray()));

        playerUIReady = 0;
    }

    #endregion

    #region Core
    public void NewEnemyKill()
    {
        lock (this)
        {
            currentEnemiesKilled++;
        }
    }

    public bool IsVictory()
    {
        if (StateManager.Get.State == StateManager.GameState.Play)
        {
            if (currentEnemiesKilled >= numberEnemyInLevel)
            {
                StateManager.Get.State = StateManager.GameState.Victory;
                currentState = VictoryStates.PlayerComingOut;
                gameManager.OnWin();

                playersLife = new LifeBehavior[numberPlayers];
                for (int i = 0; i < numberPlayers; i++)
                {
                    if (gameManager.PlayerControllers[i])
                        playersLife[i] = gameManager.PlayerControllers[i].gameObject.GetComponent<LifeBehavior>();
                }

                return true;
            }

        }
        return false;
    }

    private void Victory()
    {
        switch (currentState)
        {
            case VictoryStates.NotWin:
                // Rien a faire tant que la partie n'est pas gagnée
                break;
            case VictoryStates.PlayerComingOut:
                if (lastState != currentState)
                {
                    // On change le layer des players pour qu'ils ne soient plus bloqués
                    //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

//ici changement, on a déjà une liste des players dans le gameManager
                    PlayerController[] players = GameManager.GetSingleton.PlayerControllers;

                    for (int i = 0; i < players.Length; i++)
                    {
                        //players[i].layer = 14;
                        players[i].gameObject.layer = 14;
                    }


                }

                // On attends que tous les players aient disparus de l'écran (kill) avant de passer à la phase suivante
                int numberDeath = 0;
                for (int i = 0; i < playersLife.Length; i++)
                {
                    if (!playersLife[i] || playersLife[i].CurrentLife <= 0)
                        numberDeath++;
                }
                //print("number death : " + numberDeath);
                if (numberDeath >= numberPlayers) // si tous les joueurs sont mort -> étape suivante
                    NextState();

                break;
            case VictoryStates.DisplayUI:
                // TODO : Fade in ?
                if (lastState != currentState)
                {
                    //print("Victory State : DisplayUI");
                    panelWin.SetActive(true);
                    NextState();
                }
                break;
            case VictoryStates.PlayerUIComing:
                if (lastState != currentState) // Activation que la première fois, on risque de passer plusieurs fois ici
                {
                    //print("Victory State : PlayerUIComing");

                    foreach (var playerInUI in playerUiWin)
                    {
                        playerInUI.SetActive(true);
                    }
                }

                if (playerUIReady >= numberPlayers)
                    NextState();
                break;
            case VictoryStates.DisplayScore:
                if (lastState != currentState)
                {
                    //print("Victory State : DisplayScore");

                    for (int i = 0; i < playerScoreInUI.Count; i++)
                    {
                        playerScoreInUI[i].enabled = true;
                    }

                    foreach (var name in playerNameInUI)
                    {
                        name.SetActive(true);
                    }

                    foreach (var playerLetters in playerLettersInUI)
                    {
                        playerLetters.SetActive(true);
                    }

                    if (!coroutineScoreLaunched)
                        StartCoroutine(AddScoreToPlayer(0.1f, 10, gameManager.ScoreManager.Data));
                }

                break;
            case VictoryStates.DisplayWinner:
                if (lastState != currentState)
                    NextState(); // pour l'instant on passe à la suite
                break;
            case VictoryStates.EnteringNames:
                if (lastState != currentState)
                    NextState(); // pour l'instant on passe à la suite
                break;
            case VictoryStates.Ready:
                break;
            default:
                Debug.LogError("Etat de victoire non géré (voir le WinManager)");
                break;
        }

        lastState = currentState;
    }

    private IEnumerator AddScoreToPlayer(float frequencyAdd, int amountToAdd, PlayerData datas)
    {
        coroutineScoreLaunched = true;
        float currentTime = 0f;
        bool[] scoreReady = { false, false, false, false };
        int[] currentScore = { 0, 0, 0, 0 };

        while (scoreReady.Any(isScoreReady => !isScoreReady)) // Si le tableau a au moins un score pas ready on continue
        {
            currentTime += Time.deltaTime;

            if (currentTime >= frequencyAdd)
            {
                for (int i = 0; i < numberPlayers; i++)
                {
                    if (!scoreReady[i] && currentScore[i] < datas.scorePlayer[i])
                    {
                        if (datas.scorePlayer[i] - currentScore[i] >= amountToAdd) // Si la différence entre le score affiché et le score final est plus important que la quantité qu'on va ajouter, on peut l'ajouter, sinon on affiche le score final directement
                            currentScore[i] += amountToAdd;
                        else
                            currentScore[i] = datas.scorePlayer[i];

                        playerScoreInUI[i].text = currentScore[i].ToString("00000000");
                    }
                    else
                        scoreReady[i] = true;
                }

                currentTime = 0;
            }
            yield return null;
        }

        NextState(); // Une fois les score mis a jours on passe à l'étape suivante
        coroutineScoreLaunched = false;
        yield return null;
    }

    public void PlayerUIWinReady()
    {
        lock (this)
        {
            playerUIReady++;
        }
    }

    private void NextState(float timeBeforeChangeState = 0)
    {
        if (!coroutineLaunched)
            StartCoroutine(NextStateWithTime(timeBeforeChangeState));
    }
    IEnumerator NextStateWithTime(float timeBeforeChangeState)
    {
        coroutineLaunched = true;
        yield return new WaitForSeconds(timeBeforeChangeState);
        currentState++;
        coroutineLaunched = false;
        yield return null;
    }


    private void InputVictory()
    {
        if (StateManager.Get.State == StateManager.GameState.Victory)
        {
            if (currentState == VictoryStates.Ready) // Si aucun des joueurs n'est en train de changer son nom
            {
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireA"))
                {
                    StateManager.Get.State = StateManager.GameState.Tuto;
                    SoundManager.GetSingularity.PlaySound("Stop_ingame");

                    SceneChangeManager.GetSingleton.JumpToScene();
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB"))
                {
                    //Init();
                    StateManager.Get.State = StateManager.GameState.Menu;
                    SoundManager.GetSingularity.PlaySound("Stop_ingame");

                    SceneChangeManager.GetSingleton.JumpToScene("1_Menu");
                    //SceneManager.LoadScene("1_Menu");

                }
            }
        }
    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (!enableWin)
            return;

        if (currentState > VictoryStates.NotWin)
        {
            //optimisation des fps
            if (updateTimer.Ready())
            {
                InputVictory();
                Victory();
            }
        }
    }

    #endregion
}
