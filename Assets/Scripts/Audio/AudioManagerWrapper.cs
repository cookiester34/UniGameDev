using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerWrapper : MonoBehaviour
{
    // Wrapper used on UI components to access UI Sounds on click.

    public void PlaySoundUI (AudioClip clip)
    {
        AudioManager.Instance.PlaySoundClip(clip);
    }
}
