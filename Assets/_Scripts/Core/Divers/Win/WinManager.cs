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

    [Header("Etape d'ajout de score")]
    [FoldoutGroup("Object In World"), Tooltip("Frequence d'ajout du score"), SerializeField]
    private float frequencyAddScore = 0.1f;

    [FoldoutGroup("Object In World"), Tooltip("Score ajouté à chaque tick de la fréquence précisé au dessus"), SerializeField]
    private int scoreToAdd = 500;

    [Header("Etape choix pseudo")]
    [FoldoutGroup("Object In World"), Tooltip("Fréquence de blink"), SerializeField]
    private float blinkFrequency = 0.1f;

    [Header("Canvas")]

    [FoldoutGroup("Object In World"), Tooltip("panel Win"), SerializeField]
    private GameObject panelWin;

    [FoldoutGroup("Object In World"), Tooltip("Infos pour state ready"), SerializeField]
    private GameObject inputInfo;

    [FoldoutGroup("Object In World"), Tooltip("Infos pour state pseudo"), SerializeField]
    private GameObject lettersInfo;

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

    [FoldoutGroup("Debug"), Tooltip("prefabs Courone"), SerializeField]
    private GameObject prefabsCouroneWinner;

    [FoldoutGroup("Debug"), Tooltip("Nombre d'ennemis tués")]
    private int currentEnemiesKilled = 0;

    [FoldoutGroup("Object In World"), Tooltip("Lettres des joueurs dans l'UI "), SerializeField]
    private List<GameObject> objectPositionCourone;


    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;
    [FoldoutGroup("Debug"), Tooltip("Etat de la gestion de victoire")]
    private VictoryStates currentState;
    private VictoryStates lastState;

    private GameManager gameManager;
    private LifeBehavior[] playersLife;
    private bool enableWin = true;
    private int numberPlayers = 4;
    private List<Rewired.Player> playerConnected;

    [FoldoutGroup("Debug"), Tooltip("Pseudo joueurs")]
    private List<List<char>> playerNames;
    [FoldoutGroup("Debug"), Tooltip("Lettres joueurs dans UI")]
    private List<List<Text>> playersUILetters;
    private List<int> currentLetterEditing;
    private float changeLetterEach = 0.5f;
    private List<float> changeLetterTimer;

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

        playersUILetters = new List<List<Text>>();
        currentLetterEditing = new List<int> { 0, 0, 0, 0 };
        changeLetterTimer = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f };
        foreach (var playerLetters in playerLettersInUI)
        {
            playerLetters.SetActive(false);

            List<Text> lettersSpecificPlayer = new List<Text>();
            Text letter1 = playerLetters.transform.GetChild(0).GetComponent<Text>();
            Text letter2 = playerLetters.transform.GetChild(1).GetComponent<Text>();
            Text letter3 = playerLetters.transform.GetChild(2).GetComponent<Text>();

            lettersSpecificPlayer.Add(letter1);
            lettersSpecificPlayer.Add(letter2);
            lettersSpecificPlayer.Add(letter3);

            playersUILetters.Add(lettersSpecificPlayer);
        }

        playerNames = new List<List<char>>();
        playerNames.Add(new List<char> { 'A', 'A', 'A' });
        playerNames.Add(new List<char> { 'A', 'A', 'A' });
        playerNames.Add(new List<char> { 'A', 'A', 'A' });
        playerNames.Add(new List<char> { 'A', 'A', 'A' });

        UpdateUiName();

        playerConnected = new List<Rewired.Player>();

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
        if (StateManager.GetSingleton.State == StateManager.GameState.Play)
        {
            if (currentEnemiesKilled >= numberEnemyInLevel)
            {
                OnVictory();
                return true;
            }

        }
        return false;
    }

    [FoldoutGroup("Debug"), Button("Victory")]
    private void OnVictory()
    {
        StateManager.GetSingleton.State = StateManager.GameState.Victory;
        SoundManager.GetSingularity.CleanProjectileSound();
        currentState = VictoryStates.PlayerComingOut;

        inputInfo.SetActive(false);
        lettersInfo.SetActive(true);

        gameManager.OnWin();

        playersLife = new LifeBehavior[numberPlayers];
        for (int i = 0; i < numberPlayers; i++)
        {
            if (gameManager.PlayerControllers[i])
                playersLife[i] = gameManager.PlayerControllers[i].gameObject.GetComponent<LifeBehavior>();
        }
        var connected = PlayerConnected.GetSingleton.playerArrayConnected;
        for (int i = 0; i < connected.Length; i++)
        {
            if (connected[i])
            {
                playerConnected.Add(PlayerConnected.GetSingleton.getPlayer(i));
            }
        }
    }

    /// <summary>
    /// set la couronne sur le joueur
    /// </summary>
    private void setCourone()
    {
        //Debug.Log("set couronne !");
        //GameObject coutoneTmp = Instantiate(prefabsCouroneWinner);
        PlayerData dataScore = gameManager.ScoreManager.Data;
        int score = 0;
        int indexPlayerWinner = 0;
        for (int i = 0; i < dataScore.scorePlayer.Count; i++)
        {
            if (dataScore.scorePlayer[i] > score && gameManager.PlayersInGame[i])
            {
                indexPlayerWinner = i;
                score = dataScore.scorePlayer[i];
            }
                
        }
        prefabsCouroneWinner.transform.position = objectPositionCourone[indexPlayerWinner].transform.position;
        prefabsCouroneWinner.SetActive(true);
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
                        if (players[i] != null)
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
                    SoundManager.GetSingularity.PlayMenuMusic();

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
                    SoundManager.GetSingularity.PlayScoreSound();
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
                        StartCoroutine(AddScoreToPlayer(frequencyAddScore, scoreToAdd, gameManager.ScoreManager.Data));
                }

                break;
            case VictoryStates.DisplayWinner:
                if (lastState != currentState)
                {
                    NextState(); // pour l'instant on passe à la suite
                    setCourone();
                }
                    
                break;
            case VictoryStates.EnteringNames:
                if (lastState != currentState)
                {
                    StartCoroutine(BlinkLetters(blinkFrequency));
                    bool[] arePlayersConnected = gameManager.PlayersInGame;
                    for (int i = 0; i < arePlayersConnected.Length; i++)
                    {
                        if (!arePlayersConnected[i])
                            currentLetterEditing[i] = 3;
                    }
                }

                int numberPlayersReady = currentLetterEditing.Where(l => l == 3).Count();
                if (numberPlayersReady >= 4)
                    NextState();
                break;
            case VictoryStates.Ready:
                if (lastState != currentState)
                {
                    inputInfo.SetActive(true);
                    lettersInfo.SetActive(false);

                    PlayerData datas = gameManager.ScoreManager.Data;
                    for (int i = 0; i < datas.scorePlayer.Count; i++)
                    {
                        if (datas.scorePlayer[i] > 0 && i < 4)
                        {
                            string playerName = playerNames[i][0].ToString() + playerNames[i][1].ToString() + playerNames[i][2].ToString();
                            Leaderboard.GetSingleton.AddNewHighscore(playerName, datas.scorePlayer[i]);
                        }
                    }
                }
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

        SoundManager.GetSingularity.StopScore();
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
        if (StateManager.GetSingleton.State == StateManager.GameState.Victory)
        {
            if (currentState == VictoryStates.Ready) // Si aucun des joueurs n'est en train de changer son nom
            {
                bool[] playersConnected = gameManager.PlayersInGame;
                int firstIndexPlayerInGame = 0;
                for (int i = 0; i < playersConnected.Length; i++)
                {
                    if(playersConnected[i])
                    {
                        firstIndexPlayerInGame = i;
                        break;
                    }
                }

                if (PlayerConnected.GetSingleton.getPlayer(firstIndexPlayerInGame).GetButtonDown("FireA"))
                {
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    StateManager.GetSingleton.State = StateManager.GameState.Tuto;
                    SceneChangeManager.GetSingleton.JumpToScene();
                }
                if (PlayerConnected.GetSingleton.getPlayer(firstIndexPlayerInGame).GetButtonDown("FireB"))
                {
                    //Init();
                    //SceneManager.LoadScene("1_Menu");
                    StateManager.GetSingleton.State = StateManager.GameState.Menu;
                    SceneChangeManager.GetSingleton.JumpToScene("1_Menu");
                }
            }
        }
    }

    private void InputEnteringNames()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Victory && currentState == VictoryStates.EnteringNames)
        {
            foreach (var player in playerConnected)
            {
                changeLetterTimer[player.id] += Time.deltaTime;
                if (player.GetButtonDown("FireA") || player.GetButtonDown("FireB"))
                {
                    if (player.GetButtonDown("FireA"))
                    {
                        if (currentLetterEditing[player.id] < 3)
                            currentLetterEditing[player.id]++;
                    }
                    else if (player.GetButtonDown("FireB"))
                    {
                        if (currentLetterEditing[player.id] > 0)
                            currentLetterEditing[player.id]--;
                    }
                }
                else if (currentLetterEditing[player.id] < 3)
                {
                    var vertMove = player.GetAxis("Move Vertical");
                    int orientation = -1;

                    if (vertMove > 0)
                        orientation = 0; // up
                    else if (vertMove < 0)
                        orientation = 1; // down
                    else
                        orientation = -1; // nothing

                    if (changeLetterTimer[player.id] > changeLetterEach && orientation > -1)
                    {
                        changeLetterTimer[player.id] = 0.0f;
                        switch (orientation)
                        {
                            case 0: // Haut
                                    // Changer lettre précédante de l'alphabet                        
                                playerNames[player.id][currentLetterEditing[player.id]]--;
                                if (playerNames[player.id][currentLetterEditing[player.id]] < 65)
                                    playerNames[player.id][currentLetterEditing[player.id]] = (char)90;
                                break;
                            case 1: // Bas
                                    // Changer lettre suivante de l'alphabet
                                playerNames[player.id][currentLetterEditing[player.id]]++;
                                if (playerNames[player.id][currentLetterEditing[player.id]] > 90)
                                    playerNames[player.id][currentLetterEditing[player.id]] = (char)65;
                                break;
                            default:
                                // Aucun controle récupéré
                                break;
                        }

                        if (orientation > -1)
                            UpdateUiName();
                    }
                    else if (orientation == -1)
                    {
                        changeLetterTimer[player.id] = changeLetterEach;
                    }

                    //print(playerNames[0][0].ToString() + playerNames[0][1].ToString() + playerNames[0][2].ToString() + "  " + playerNames[1][0].ToString() + playerNames[1][1].ToString() + playerNames[1][2].ToString());
                }
            }
        }
    }

    private void UpdateUiName()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            playersUILetters[i][0].text = playerNames[i][0].ToString();
            playersUILetters[i][1].text = playerNames[i][1].ToString();
            playersUILetters[i][2].text = playerNames[i][2].ToString();
        }
    }

    IEnumerator BlinkLetters(float blinkFrequency)
    {
        float currentTime = 0.0f;
        float currentPhaseTime = 0.0f;

        float phaseDuration = blinkFrequency;
        bool isGrey = true;
        List<List<Text>> uiLetters = new List<List<Text>>(playersUILetters);


        List<Color> oldColors = new List<Color>();
        foreach (var letters in uiLetters)
        {
            oldColors.Add(letters.First().color);
        }

        while (currentState == VictoryStates.EnteringNames)
        {
            currentTime += Time.deltaTime;
            currentPhaseTime += Time.deltaTime;

            if (currentPhaseTime > phaseDuration)
            {
                isGrey = !isGrey;
                currentPhaseTime = 0.0f;
            }

            for (int i = 0; i < uiLetters.Count; i++)
            {
                for (int j = 0; j < uiLetters[i].Count; j++)
                {
                    if (currentLetterEditing[i] == j && currentLetterEditing[i] < 3 && isGrey)
                        uiLetters[i][j].color = Color.grey;
                    else
                        uiLetters[i][j].color = oldColors[i];
                }
            }

            yield return null;
        }

        for (int i = 0; i < uiLetters.Count; i++)
        {
            for (int j = 0; j < uiLetters[i].Count; j++)
            {
                uiLetters[i][j].color = oldColors[i];
            }
        }

        yield return null;
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
                InputEnteringNames();
                Victory();
            }
        }
    }

    #endregion
}
