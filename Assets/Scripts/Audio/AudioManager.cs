﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Util;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;
    public Sound[] sounds;
    public List<Music> peaceMusic;
    public List<Music> combatMusic;
    public List<Sound> ambienceTracks;
    public Music mainMenuMusic;

    public Sound buildingSelectSound;
    public SceneMusicType startingMusicType;
    public float lowResourceThreshold;

    private Music currentTrack;
    private float currentTrackLength;
    private IEnumerator musicLoop;
    private AudioSource musicSource;
    private MusicQueue peaceMusicQueue;
    private MusicQueue combatMusicQueue;

    private Sound currentAmbience;

    private Building lastSelectedBuilding;


    #region SINGLETON PATTERN

    private static AudioManager _instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.AudioSingleton);
                Instantiate(go);
            }
                
            return _instance;
        }
    }
    #endregion

    // Loop through our list of sounds and add an audio source for each.
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Debug.LogWarning("Creating Second instance of AudioManager. Deleting.");
            Destroy(gameObject);
            return;
        }

        #region AUDIO SOURCE INITIALISATION
        // Apply settings chosen in list to sound sources.
        foreach (Sound s in sounds)
        {
            InitialiseSound(s);
        }

        foreach(Music m in peaceMusic)
        {
            InitialiseMusic(m);
        }

        foreach(Music m in combatMusic)
        {
            InitialiseMusic(m);
        }

        foreach(Sound s in ambienceTracks)
        {
            InitialiseSound(s);
        }

        InitialiseMusic(mainMenuMusic);

        InitialiseSound(buildingSelectSound);
        #endregion
        HandleSceneChange();
        SceneManager.activeSceneChanged += MatchMusicToScene;

        peaceMusicQueue = new MusicQueue(peaceMusic);
        combatMusicQueue = new MusicQueue(combatMusic);

        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update.
    void Start()
    {
        SceneManager.sceneLoaded += HandleSceneChangeEvent;
        UpdateAudioLevels();
        // Music playback can be started here.
        //StartPeaceMusic();
    }

    void HandleSceneChangeEvent(Scene scene, LoadSceneMode mode)
    {
        HandleSceneChange();
    }

    void HandleSceneChange()
    {
        BuildingManager.Instance.OnBuildingSelected += PlayBuildingClip;
        ResourceManagement.Instance.resourceList.ForEach(GetValueChanged);
        SeasonManager.Instance.SeasonChange += ChangeAmbienceTrack;
        UIEventAnnounceManager.Instance.announcement += PlayAnnouncementAlert;
    }

    private void InitialiseSound(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.soundClip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.outputAudioMixerGroup = s.output;
    }

    private void InitialiseMusic(Music m)
    {
        m.source = gameObject.AddComponent<AudioSource>();
        m.source.clip = m.musicClip;
        m.source.volume = m.volume;
        m.source.pitch = m.pitch;
        m.source.outputAudioMixerGroup = m.output;
    }

    public void UpdateAudioLevels() {
        mixer.SetFloat("MasterVol", ConvertLevel(Settings.MasterVolume.Value));
        mixer.SetFloat("MusicVol", ConvertLevel(Settings.MusicVolume.Value));
        mixer.SetFloat("EffectsVol", ConvertLevel(Settings.EffectsVolume.Value));
        mixer.SetFloat("UIVol", ConvertLevel(Settings.UiVolume.Value));
    }

    // Offset the pitch and volume by 5% from its setting in soundlist. Could promote the ranges to a variable but isn't required as it's a subtle effect.
    void AutoModulate(Sound sound)
    {
        float pitchOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        float volumeOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        sound.source.pitch = sound.pitch + pitchOffset;
        sound.source.volume = sound.volume + volumeOffset;
    }

    public void ModulateAudioSource(AudioSource source)
    {
        float pitchOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        float volumeOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        source.pitch = 1 + pitchOffset;
        source.volume = 1 + volumeOffset;
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

        if (s.autoModulation)
        {
            AutoModulate(s);
        }

        // Try playing the sound, since we're taking input from code.
        try
        {
            s.source.Play();
        }
        catch (Exception e)
        {
            Debug.Log("Error playing sound: " + e.Message);
        } 
    }

    public void PlaySoundClip (AudioClip sound)
    {
        // Find the specified sound in the list.
        foreach (Sound s in sounds)
        {
            if (s.soundClip == sound)
            {
                // Modulate if enabled.

                if (s.autoModulation)
                {
                    AutoModulate(s);
                }

                // Play the sound, then stop iterating through the list after we've found our sound.
                s.source.Play();
                break;
            }
        }
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
        //if (musicQueue != null)
        //{
        //    Debug.Log("Clearing Tracks");
        //    musicQueue.ClearTracks();
        //}

        if (musicLoop != null)
        {
            Debug.Log("Stopping coroutine " + musicLoop.ToString());
            StopCoroutine(musicLoop);
        }

        if (musicSource != null)
            musicSource.Stop();
    }

    // Initialises the music queue and starts playback.

    // We stop the current queue and any music playing. We then queue and play the type of music requested.
    public void StartMusic (SceneMusicType type)
    {
        if (mainMenuMusic.source.isPlaying)
            mainMenuMusic.source.Stop();
        StopMusic();
        Debug.Log("Music Stopped");
        switch(type)
        {
            case SceneMusicType.mainMenu:
                mainMenuMusic.source.Play();
                break;
            case SceneMusicType.peace:
                //peaceMusicQueue = new MusicQueue(peaceMusic);
                musicSource = GetComponent<AudioSource>();
                musicLoop = peaceMusicQueue.LoopMusic(this, 0, PlayMusicClip);
                Debug.Log("playing peace");
                StartCoroutine(musicLoop);
                break;
            case SceneMusicType.combat:
                //combatMusicQueue = new MusicQueue(combatMusic);
                musicSource = GetComponent<AudioSource>();
                musicLoop = combatMusicQueue.LoopMusic(this, 0, PlayMusicClip);
                Debug.Log("playing combat");
                StartCoroutine(musicLoop);
                break;
        }
    }

    public void MatchMusicToScene(Scene current, Scene next)
    {
        Debug.Log("Changing scene type to " + CurrentSceneType.SceneType);

        if (next.name == "Main")
        {
            StartMusic(SceneMusicType.mainMenu);
            if (currentAmbience != null)
            {
                currentAmbience.source.Stop();
            }
        }
        else
        {
            StartMusic(SceneMusicType.peace);
        }
    }

    public void StartCombatMusic()
    {
        Debug.Log("Switching to combat");
        StartMusic(SceneMusicType.combat);
    }

    public void StartPeaceMusic()
    {
        Debug.Log("Switching to peace");
        StartMusic(SceneMusicType.peace);
    }

    public void ChangeAmbienceTrack()
    {
        PlayAmbienceTrack(SeasonManager.Instance.CurrentSeason);
    }

    public void AmbienceFadeTo (Sound ambienceClip)
    {
        StartCoroutine(FadeTo(ambienceClip));
    }

    // Pass in the next sound as clip, volume stars at 0 and begine playing.
    IEnumerator FadeTo(Sound clip)
    {
        clip.source.volume = 0f;

        float fadeSpeed = 1f;
        float t = 0;
        float v = currentAmbience.volume;
        clip.source.Play();

        // Swap volume values by lerping exponentially. Smaller fadespeed value results in quicker fade. Cannot fade shorter than 0.1s. 
        while (t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * fadeSpeed);
            currentAmbience.source.volume = Mathf.Lerp(v, 0f, t);
            clip.source.volume = Mathf.Lerp(0f, clip.volume, t);
            //Debug.Log(clip.source.volume.ToString()); - For debugging.
            yield return null;
        }

        // Finally set the volume of the new clip, stop the old clip, and make the new clip the current clip to be used next time.
        clip.source.volume = clip.volume;

        currentAmbience.source.Stop();
        currentAmbience = clip;
        yield break;
    }

    public void PlayAmbienceTrack(Seasons season)
    {
        // Ambient Season tracks must be named correctly. Could possibly be a better way to do this, however not really necessary.
        if (currentAmbience != null && currentAmbience.source.isPlaying)
        {
            AmbienceFadeTo(ambienceTracks.Find(sound => sound.name == season.ToString()));
        }
        else
        {
            currentAmbience = ambienceTracks.Find(sound => sound.name == season.ToString());
            Debug.Log("Playing " + season.ToString());
            currentAmbience.source.Play();
        }
    }
    
    /// <summary>
    /// Converts the saved value to log scale
    /// </summary>
    /// <param name="value">Saved setting value</param>
    /// <returns>Converted value</returns>
    private float ConvertLevel(float value) {
        return Mathf.Log10(value) * 20;
    }

    public void PlayBuildingClip(Building building)
    {
        if (!buildingSelectSound.source.isPlaying && building != null && building != lastSelectedBuilding)
        {
            buildingSelectSound.source.Play();
        }
        lastSelectedBuilding = building;        
    }

    private void GetValueChanged(Resource resource)
    {
        if (resource.resourceType != ResourceType.AssignedPop)
        {
            resource.OnCurrentValueChanged += PlayResourceAlert;
        }
    }

    public void PlayResourceAlert(float value)
    {
 
        if (value <= 0f)
        {
            if (BuildingManager.Instance.Buildings.Count > 0)
            {
                PlaySound("ResourceDepleted");
            }
        }
    }

    public void PlayAnnouncementAlert(AnnounceEventType eventType)
    {
        // TODO - Sam add some short audio clips here, for different event types
        switch (eventType) {
            case AnnounceEventType.Alert:
                PlaySound("AnnouncementAlert");
                break;
            case AnnounceEventType.Misc:
                PlaySound("AnnouncementMisc");
                break;
            case AnnounceEventType.Tutorial:
                PlaySound("AnnouncementTutorial");
                break;
            default:
                PlaySound("AnnouncementMisc");
                break;
        }
    }
}

[Serializable]
public enum SceneMusicType
{
    mainMenu,
    combat,
    peace,
}
