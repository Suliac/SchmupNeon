using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuUtilities : MonoBehaviour
{
    bool firstSoundPlayed = false;
    public void PlayMenuSoundChangeButton()
    {
        if (StateManager.GetSingleton.State == StateManager.GameState.Menu)
        {
            if (firstSoundPlayed)
                SoundManager.GetSingularity.PlayMenuSound();
            else
                firstSoundPlayed = true;
        }
    }

    public void PlayMenuSoundEnter()
    {
        SoundManager.GetSingularity.PlayMenuSoundEnter();
    }

    public void PlayMenuSoundLeave()
    {
        SoundManager.GetSingularity.PlayMenuSoundLeave();
    }
}
