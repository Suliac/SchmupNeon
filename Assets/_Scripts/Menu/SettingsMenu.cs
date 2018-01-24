using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SettingsMenu : MonoBehaviour
{
    
    [FoldoutGroup("GamePlay"), Tooltip("phase du menu (pour les inputs)")]
    public int phaseMenu = 0;

    [FoldoutGroup("GamePlay"), Tooltip("audio mixer (a changer car on est dans wwise)")]
    public AudioMixer audioMixer;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug"), Tooltip("Boutton quitter"), SerializeField]
    private Button quitButton;
    [FoldoutGroup("Debug"), Tooltip("Boutton 'no' pas quitter"), SerializeField]
    private Button quitNoButton;
    [FoldoutGroup("Debug"), Tooltip("boutton back dans les settings"), SerializeField]
    private Button settingBackButton;
    [FoldoutGroup("Debug"), Tooltip("boutton back dans les credits"), SerializeField]
    private Button creditBackButton;

    [Tooltip("Debug"), SerializeField]
    private List<Button> buttonsMainMenu;

    private GameObject myEventSystem;

    //debug de la selection des boutons
    private bool justPhase0 = false;

    private TimeWithNoEffect TWNE;

    private void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    private void Start()
    {
        TWNE = GetComponent<TimeWithNoEffect>();
        SoundManager.GetSingularity.PlayMenuMusic();
        Cursor.visible = false;
        buttonsMainMenu[0].Select();
    }

    public void SetRes1()
    {
        Screen.SetResolution(768, 432, true);
        Debug.Log(Screen.currentResolution);
    }

    public void SetRes2()
    {
        Screen.SetResolution(1280, 720, true);
        Debug.Log(Screen.currentResolution);
    }

    public void SetRes3()
    {
        Screen.SetResolution(1920, 1080, true);
        Debug.Log(Screen.currentResolution);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetPhaseMenu(int phase)
    {
        phaseMenu = phase;
    }

    /// <summary>
    /// lance le jeu
    /// </summary>
    public void Play()
    {
        SceneChangeManager.GetSingleton.JumpToSceneWithFade("2_Game");
    }

    /// <summary>
    /// lance le jeu
    /// </summary>
    public void Leaderboard()
    {
        SceneChangeManager.GetSingleton.JumpToSceneWithFade("3_Leaderboard");
    }

    /// <summary>
    /// lance le jeu
    /// </summary>
    public void Quit()
    {
        SceneChangeManager.GetSingleton.Quit();
    }

    /// <summary>
    /// set la phase 0 (debug du menu quand on appuis sur NO) pour retourner au menu)
    /// </summary>
    public void debugSetPhase0()
    {
        justPhase0 = true;
    }

    /*
     * phase 0: menu de base
     * phase 1: are you sure ?
     * phase 2: settings volume etc
     */
    private void InputMenu()
    {
        if (!TWNE.isOk)
            return;
        switch (phaseMenu)
        {
            case 0:
                if (justPhase0)
                {
                    justPhase0 = false;
                    buttonsMainMenu[0].Select();
                }

                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    quitButton.onClick.Invoke();
                    //mainMenu.enabled = true;
                }
                break;
            case 1:
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    justPhase0 = true;
                    quitNoButton.onClick.Invoke();
                }
                break;
            case 2:
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    justPhase0 = true;
                    settingBackButton.onClick.Invoke();
                }
                break;
            case 3:
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    justPhase0 = true;
                    creditBackButton.onClick.Invoke();
                }
                break;

        }

    }

    /// <summary>
    /// est appelé pour débug le clique
    /// Quand on clique avec la souris: reselect le premier bouton !
    /// </summary>
    private void DebugMouseCLick()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            buttonsMainMenu[0].Select();
        }
        if (phaseMenu == 0)
        {
            bool isSomeoneSelected = false;
            for (int i = 0; i < buttonsMainMenu.Count; i++)
            {
                if (EventSystem.current.currentSelectedGameObject == buttonsMainMenu[i].gameObject)
                    isSomeoneSelected = true;
            }
            if (!isSomeoneSelected)
            {
                //EventSystem.current.SetSelectedGameObject(PlayButton);
                buttonsMainMenu[0].Select();
            }
        }
    }

    private void Update()
    {
        InputMenu();
        //optimisation des fps
        if (updateTimer.Ready())
        {
            DebugMouseCLick();
        }
    }
}
