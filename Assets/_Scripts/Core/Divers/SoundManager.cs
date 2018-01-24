using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
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

    private static bool isPlayingShoot = false;
    private static bool isInit = false;

    private List<bool> playerShooting;
    private object mutex = new object();
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
        playerShooting = new List<bool>();
        playerShooting.Add(false);
        playerShooting.Add(false);
        playerShooting.Add(false);
        playerShooting.Add(false);
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
        //print("Play menu music");
        AkSoundEngine.SetState("musique", "Menu");

        //SoundManager.GetSingularity.PlaySound("Stop_ingame");
        //SoundManager.GetSingularity.PlaySound("Play_Menu");

    }

    public void PlayGameMusic()
    {
        //print("Play game music");
        AkSoundEngine.SetState("musique", "In_game");

        //SoundManager.GetSingularity.PlaySound("Stop_Menu");
        //SoundManager.GetSingularity.PlaySound("Play_ingame");

    }

    public void PlayGameOverMusic()
    {
        //print("Play game over music");
        AkSoundEngine.SetState("musique", "Game_Over");

        //SoundManager.GetSingularity.PlaySound("Stop_Menu");
        //SoundManager.GetSingularity.PlaySound("Play_ingame");
    }

    public void PlayDeadPlayerSound()
    {
        //print("Play deadplayer sound");
        PlaySound("Play_playerdead3");
    }

    public void PlayExplosionSound()
    {
        //print("Play explosion sound");
        PlaySound("Play_Explosion");
    }

    public void PlayPickupSound(int playerNumber)
    {
        int tmpPlayer = playerNumber + 1;
        //print("Play pickup sound for P" + tmpPlayer.ToString("0"));
        PlaySound("Play_pickupP" + tmpPlayer.ToString("0"));
    }

    public void PlayRespawnSound(int playerNumber)
    {
        int tmpPlayer = playerNumber + 1;
        //print("Play respawn sound for P" + tmpPlayer.ToString("0"));
        PlaySound("Play_respawnP" + tmpPlayer.ToString("0"));
    }

    public void PlayImpactSound(int playerNumber)
    {
        int tmpPlayer = playerNumber + 1;
        PlaySound("Play_impact" + tmpPlayer.ToString("0"));
    }

    public void PlayDeadEnemySound()
    {
        PlaySound("Play_sfx_dead");

    }

    public void PlayMenuSound()
    {
        PlaySound("Play_UI_menu1");
    }

    public void PlayMenuSoundEnter()
    {
        PlaySound("Play_UI_menu2");
    }

    public void PlayMenuSoundLeave()
    {
        PlaySound("Play_UI_menu3");
    }

    //public void PlayProjectileSound()
    //{
    //    //print("Play projectile sound");
    //    PlaySound("Play_proj3");
    //}

    //public void StopProjectileSound()
    //{
    //    //print("Play projectile sound");
    //    PlaySound("Stop_proj3");
    //}

    public void CleanProjectileSound()
    {
        for (int i = 0; i < 4; i++)
        {
            StopProjectileSound(i);
        }
    }

    public void PlayProjectileSound(int playerIndex)
    {
        lock (mutex)
        {
            playerShooting[playerIndex] = true;

            if (playerShooting.Contains(true) && !isPlayingShoot) // Si au moins un joueur tir
            {
                //print("Play projectile sound");
                PlaySound("Play_Tir");
                isPlayingShoot = true;
            }
        }
    }

    public void StopProjectileSound(int playerIndex)
    {
        lock (mutex)
        {
            playerShooting[playerIndex] = false;

            if (!playerShooting.Contains(true) && isPlayingShoot) // Si aucun joueur ne tir
            {
                //print("Stop projectile sound");
                PlaySound("Stop_Tir");
                isPlayingShoot = false;
            }
        }
    }
    #endregion
}
