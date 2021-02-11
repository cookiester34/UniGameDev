﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip soundClip;

    [Range(0f, 2f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;
    //public bool autoModulation; - Not yet implemented.

    public AudioMixerGroup output;

    [HideInInspector]
    public AudioSource source;
}
