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
    private static bool isInit = false;
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
        Init();
    }

    private void Init()
    {
        if (!isInit)
        {
            print("Init sound");
            PlaySound("Play_musique");
            isInit = true;
        }
    }
    #endregion

    #region Core

    private void PlaySound(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void PlayMenuMusic()
    {
        print("Play menu music");
        AkSoundEngine.SetState("musique", "Menu");

        //SoundManager.GetSingularity.PlaySound("Stop_ingame");
        //SoundManager.GetSingularity.PlaySound("Play_Menu");

    }

    public void PlayGameMusic()
    {
        print("Play game music");
        AkSoundEngine.SetState("musique", "In_game");

        //SoundManager.GetSingularity.PlaySound("Stop_Menu");
        //SoundManager.GetSingularity.PlaySound("Play_ingame");

    }

    public void PlayGameOverMusic()
    {
        print("Play game music");
        AkSoundEngine.SetState("musique", "Game_Over");

        //SoundManager.GetSingularity.PlaySound("Stop_Menu");
        //SoundManager.GetSingularity.PlaySound("Play_ingame");

    }
    #endregion
}
