using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerProxy : MonoBehaviour
{
    // Used to play sounds on events. If playing sound from code, try calling directly to the AudioManager instead.
    public void PlaySound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName);
    }

    public void PlayCombatMusic()
    {
        AudioManager.Instance.StartCombatMusic();
    }

    public void PlayPeaceMusic()
    {
        AudioManager.Instance.StartPeaceMusic();
    }

    public void StopMusic()
    {
        AudioManager.Instance.StopMusic();
    }
}
