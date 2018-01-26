using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Linq;

/// <summary>
/// TutoStart Description
/// </summary>
[RequireComponent(typeof(GameManager))]    //item manager
public class TutoStart : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("position Ligne Rouge"), SerializeField, Range(0, 1)]
    private float redLineX = 0.3f;

    [FoldoutGroup("GamePlay"), Tooltip("position Ligne Rouge"), SerializeField, Range(0, 1)]
    private float leftLineX = 0.3f;
    [FoldoutGroup("GamePlay"), Tooltip("position Ligne Rouge"), SerializeField, Range(0, 1)]
    private float rightLineX = 0.5f;
    [FoldoutGroup("GamePlay"), Tooltip("position Ligne Rouge"), SerializeField, Range(0, 1)]
    private float topLineY = 0.5f;
    [FoldoutGroup("GamePlay"), Tooltip("position Ligne Rouge"), SerializeField, Range(0, 1)]
    private float botLineY = 0.3f;

    [FoldoutGroup("GamePlay"), Tooltip("Autorise moins de 4 manettes"), SerializeField]
    private bool fourPlayerOnly = false;
    [FoldoutGroup("GamePlay"), Tooltip("Timer Chrono"), SerializeField]
    private int timerTuto = 3;


    [FoldoutGroup("Object In World"), Tooltip("panel Tuto"), SerializeField]
    private GameObject panelTutoInGame;

    [FoldoutGroup("Object In World"), Tooltip("canvas des players"), SerializeField]
    private List<GameObject> listTutoPX;
    public List<GameObject> Overlay;

    [FoldoutGroup("Object In World"), Tooltip("Revenir à gauche"), SerializeField]
    private GameObject comeBackLeft;
    //[FoldoutGroup("Object In World"), Tooltip("Sprite joypad"), SerializeField]
    //private List<GameObject> connectJoypad;
    [FoldoutGroup("Object In World"), Tooltip("redLine"), SerializeField]
    private Image redLine;
    [FoldoutGroup("Object In World"), Tooltip("Lines"), SerializeField]
    private List<Image> Lines;

    [FoldoutGroup("Object In World"), Tooltip("Chrono tuto"), SerializeField]
    private GameObject tutoChrono;
    [FoldoutGroup("Object In World"), Tooltip("Text chrono"), SerializeField]
    private Text textChrono;

    [FoldoutGroup("Object In World"), Tooltip("Couleur ligne"), SerializeField]
    private Color colorLine;

    [FoldoutGroup("Debug"), Tooltip("canvas des players"), SerializeField]
    private int[] listTutoState = new int[4];

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private GameManager gameManager;
    private Camera cam;
    private bool enableTuto = true;
    private int currentChrono;
    private bool onChrono = false;
    private List<Coroutine> coroutines;
    #endregion

    #region Initialization

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        cam = Camera.main;
        coroutines = new List<Coroutine>();
    }

    private void Start()
    {
        tutoChrono.SetActive(false);
    }
    #endregion

    #region Core
    /// <summary>
    /// active les tutos !
    /// </summary>
    [FoldoutGroup("Debug")]
    [Button("DesactiveTuto")]
    public void DesactiveTuto() { ActiveTuto(false); }

    public void ActiveTuto(bool active)
    {
        panelTutoInGame.SetActive(active);
        //tutoChrono.SetActive(false);
        onChrono = false;
        if (active)
        {
            comeBackLeft.SetActive(false);
            if (!active)
                gameManager.ActiveGame(true);
        }
    }

    /// <summary>
    /// change l'état du canvas joueur
    /// </summary>
    /// <param name="index"></param>
    private void ChangeState(int index)
    {
        if (listTutoState[index] == 0)
        {
            listTutoPX[index].GetComponent<Animator>().SetBool("Pressed", true);
            listTutoState[index] = 1;
            StartCoroutine(EnablePlayer(index, 0.5f));
        }
    }

    private IEnumerator EnablePlayer(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameManager.SpawnPlayer[index].gameObject.SetActive(true);    //active les spawn des players
        listTutoState[index] = 2;

        yield return null;

        if (gameManager.PlayerControllers[index] != null) //désactiver l'overlay du player
        {
            Renderer rend = Overlay[index].GetComponent<SpriteRenderer>();
            rend.material.color = gameManager.PlayerControllers[index].ColorPlayer;

            yield return new WaitForSeconds(3.0f);

            Overlay[index].SetActive(false);
            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// pour le  joueur (index), vérifi sa position par rapport à la ligne rouge
    /// </summary>
    private bool DetectIfOffLine(int index)
    {
        Vector3 playerPos = gameManager.SpawnPlayer[index].PlayerController.transform.position;
        Vector2 ViewportPosition = cam.WorldToViewportPoint(playerPos);
        if (!(ViewportPosition.x > leftLineX && ViewportPosition.x < rightLineX && ViewportPosition.y > botLineY && ViewportPosition.y < topLineY) /*|| (!gameManager.SpawnPlayer[index].PlayerController.EnabledPlayer && gameManager.SpawnPlayer[index].SpawnedOnce)*/)
        {
            listTutoState[index] = 3;
            return true;
        }
        else
        {
            listTutoState[index] = 2;
            return false;
        }

    }

    /// <summary>
    /// défini si les players sont à droite où à gauche de la ligne rouge
    /// </summary>
    private void UpdateMoveLeft()
    {
        int countPlayerOffLine = 0;
        for (int i = 0; i < 4; i++)
        {
            if (listTutoState[i] >= 2)   //si l'index est supérieur à 1, le joueur à déja appuyé sur A et est affiché !
            {
                if (DetectIfOffLine(i))     // Si player a droite
                    countPlayerOffLine++;
            }
        }

        // Si au moins un des joueurs est a dorite de la ligne -> on affiche les fleches
        comeBackLeft.SetActive(countPlayerOffLine > 0);
    }

    /// <summary>
    /// commence le chrono
    /// </summary>
    private void ChronoTuto()
    {
        if (onChrono)   //si on est déjà en mode chrono...
            return;
        StopCoroutineChrono();
        onChrono = true;
        tutoChrono.SetActive(true);
        currentChrono = timerTuto;
        coroutines.Add(StartCoroutine(ChronoPass()));
    }

    private void stopChronoTuto()
    {
        if (!onChrono)   //si on est déjà en mode chrono...
            return;
        StopCoroutineChrono();
        tutoChrono.SetActive(false);
        onChrono = false;
    }

    private void StopCoroutineChrono()
    {
        foreach (var coroutine in coroutines)
        {
            StopCoroutine(coroutine);
        }
        coroutines.Clear();
    }

    IEnumerator ChronoPass()
    {
        while (currentChrono > 0)
        {
            if (!onChrono)
                yield break;
            textChrono.text = currentChrono.ToString();
            SoundManager.GetSingularity.PlayChronoSound();
            yield return new WaitForSeconds(1);
            currentChrono--;
        }

        textChrono.text = "Go !";
        SoundManager.GetSingularity.PlayChronoFinalSound();

        for (int i = 0; i < 4; i++)
        {
            gameManager.PlayersInGame[i] = listTutoState[i] > 1;
        }

        activeGame();
        //Debug.Log("la");
        yield return new WaitForSeconds(1);

        tutoChrono.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateRedLine()
    {
        bool isAllOk = true;

        //for (int i = 0; i < 4; i++)
        //{
        //    if (listTutoState[i] < 3)   //si l'index est supérieur à 1, le joueur à déja appuyé sur A et est affiché !
        //    {
        //        if (!PlayerConnected.GetSingleton.playerArrayConnected[i] && !gameManager.SpawnPlayer[i].SpawnedOnce && !fourPlayerOnly)    //si on a jamais spawner, et qu'on est déconecté... on oublie ?
        //        {
        //            continue;
        //        }
        //        isAllOk = false;
        //    }
        //}
        int countPlayerOffLine = 0;
        for (int i = 0; i < 4; i++)
        {
            if (listTutoState[i] == 3)
                countPlayerOffLine++;
        }

        isAllOk = countPlayerOffLine == 0 && listTutoState.Any(state => state >= 2);

        if (!isAllOk || PlayerConnected.GetSingleton.NoPlayer())
        {
            //ici red line !
            //redLine.color = Color.red;
            foreach (var line in Lines)
            {
                line.color = Color.red;
            }

            stopChronoTuto();
        }
        else
        {
            //ici green line !
            foreach (var line in Lines)
            {
                line.color = colorLine;
            }
            //redLine.color = colorLine;
            ChronoTuto();
        }
    }

    //private void UpdateLockJoypad()
    //{
    //    for (int i = 0; i < connectJoypad.Count; i++)
    //    {
    //        bool activeJoypad = PlayerConnected.GetSingleton.playerArrayConnected[i];
    //        if (activeJoypad != !connectJoypad[i].activeInHierarchy)
    //        {
    //            connectJoypad[i].SetActive(!activeJoypad);
    //        }
    //    }
    //}

    /// <summary>
    /// gère les inputs pour le tuto !
    /// </summary>
    private void InputTuto()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerConnected.GetSingleton.getPlayer(i).GetButtonDown("FireA"))
            {
                ChangeState(i);
            }
        }
    }




    /// <summary>
    /// active le jeu après le tuto
    /// </summary>
    private void activeGame()
    {
        enableTuto = false;
        ActiveTuto(false);
        gameManager.ActiveGame(true);
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (!enableTuto)
            return;
        InputTuto();
        //optimisation des fps
        if (updateTimer.Ready())
        {
            UpdateMoveLeft();
            UpdateRedLine();
            //UpdateLockJoypad();
        }
    }

    #endregion
}
