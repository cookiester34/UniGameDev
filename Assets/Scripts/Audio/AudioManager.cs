using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public List<Music> peaceMusic;
    public List<Music> combatMusic;
    public Music mainMenuMusic;

    private Music currentTrack;
    private float currentTrackLength;
    private Coroutine musicLoop;

    private AudioSource musicSource;

    private MusicQueue musicQueue;

    public static AudioManager instance;

    // Loop through our list of sounds and add an audio source for each.
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Apply settings chosen in list to sound sources.

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;
        }

        foreach(Music m in peaceMusic)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.musicClip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.outputAudioMixerGroup = m.output;
        }

        foreach(Music m in combatMusic)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.musicClip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.outputAudioMixerGroup = m.output;
        }

        mainMenuMusic.source = gameObject.AddComponent<AudioSource>();
        mainMenuMusic.source.clip = mainMenuMusic.musicClip;
        mainMenuMusic.source.volume = mainMenuMusic.volume;
        mainMenuMusic.source.pitch = mainMenuMusic.pitch;
        mainMenuMusic.source.outputAudioMixerGroup = mainMenuMusic.output;
        mainMenuMusic.source.loop = true;

    }

    // Start is called before the first frame update.
    void Start()
    {
        // Music playback can be started here.
        //StartPeaceMusic();
        if (mainMenuMusic != null)
            mainMenuMusic.source.Play();

    }

    // This is what is used to play the sound in code.
    public void PlaySound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found. Check spelling!");
            return;
        }

        s.source.Play();
    }

    // Ensures the current source is stopped before assigning and playing the new one.
    public void PlayMusicClip(Music music)
    {
        musicSource.Stop();
        musicSource = music.source;
        musicSource.Play();
    }

    // If there's a queue, stop it, if there is any music audiosource playing, stop it.
    public void StopMusic()
    {
        if (musicLoop != null)
            StopCoroutine(musicLoop);
        if (musicSource != null)
            musicSource.Stop();
    }

    // Initialises the music queue and starts playback.
    public void StartMusic()
    {
        musicLoop = StartCoroutine(musicQueue.LoopMusic(this, 0, PlayMusicClip));
    }

    // Two functions stop the current queue, assign a new one and start playback. These could be condensed in to one function with a parameter, but user input isn't required so typos won't break stuff.
    public void StartCombatMusic ()
    {
        if (mainMenuMusic.source.isPlaying)
            mainMenuMusic.source.Stop();
        StopMusic();
        musicQueue = new MusicQueue(combatMusic);
        musicSource = GetComponent<AudioSource>();
        StartMusic();
    }

    public void StartPeaceMusic ()
    {
        if (mainMenuMusic.source.isPlaying)
            mainMenuMusic.source.Stop();
        StopMusic();
        musicQueue = new MusicQueue(peaceMusic);
        musicSource = GetComponent<AudioSource>();
        StartMusic();
    }
}
