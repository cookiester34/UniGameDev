using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;

    public AudioClip musicClip;

    [Range(0f, 2f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    /* Not Yet Implemented
    public bool fadeOut;
    */

    public AudioMixerGroup output;

    [HideInInspector]
    public AudioSource source;
}
