using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;


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

    public void QuitGame()
    {
        Application.Quit();
    }
}
