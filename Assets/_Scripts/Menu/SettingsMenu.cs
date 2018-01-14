using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    
    [FoldoutGroup("GamePlay"), Tooltip("phase du menu (pour les inputs)"), SerializeField]
    public int phaseMenu = 0;

    [FoldoutGroup("GamePlay"), Tooltip("audio mixer (a changer car on est dans wwise)"), SerializeField]
    public AudioMixer audioMixer;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug"), Tooltip("Boutton quitter"), SerializeField]
    private Button quitButton;
    [FoldoutGroup("Debug"), Tooltip("Boutton 'no' pas quitter"), SerializeField]
    private Button quitNoButton;
    [FoldoutGroup("Debug"), Tooltip("boutton back dans les settings"), SerializeField]
    private Button settingBackButton;

    private TimeWithNoEffect TWNE;

    private void Start()
    {
        TWNE = GetComponent<TimeWithNoEffect>();
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
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    quitButton.onClick.Invoke();
                }
                break;
            case 1:
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    quitNoButton.onClick.Invoke();
                }
                break;
            case 2:
                if (PlayerConnected.GetSingleton.getPlayer(0).GetButton("FireB"))
                {
                    TWNE.isOk = false;
                    settingBackButton.onClick.Invoke();
                }
                break;

        }
        
    }

    private void Update()
    {
        InputMenu();
        //optimisation des fps
        if (updateTimer.Ready())
        {

        }
    }
}
