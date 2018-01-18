using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// description
/// <summary>


public class SoundManager : MonoBehaviour
{
    #region Attributes

    private static SoundManager instance;
    public static SoundManager GetSingularity
    {
        get { return instance; }
    }

    private static bool isPlayingMenuMusic = false;
    private static bool isPlayingIGMusic = false;
    #endregion

    #region Initialization
    public void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
    }
    #endregion

    #region Core

    private void PlaySound(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void PlayMenuMusic()
    {
        if (!isPlayingMenuMusic)
        {
            print("Play menu music");
            isPlayingIGMusic = false;
            isPlayingMenuMusic = true;

            SoundManager.GetSingularity.PlaySound("Stop_ingame");
            SoundManager.GetSingularity.PlaySound("Play_Menu");
        }
    }

    public void PlayGameMusic()
    {
        if (!isPlayingIGMusic)
        {
            print("play IG music");
            isPlayingIGMusic = true;
            isPlayingMenuMusic = false;

            SoundManager.GetSingularity.PlaySound("Stop_Menu");
            SoundManager.GetSingularity.PlaySound("Play_ingame");
        }
    }
    #endregion
}
