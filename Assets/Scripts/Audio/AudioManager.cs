using System;
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
    private Coroutine musicLoop;
    private AudioSource musicSource;
    private MusicQueue musicQueue;

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

        BuildingManager.Instance.OnBuildingSelected += PlayBuildingClip;
        ResourceManagement.Instance.resourceList.ForEach(GetValueChanged);
        SceneManager.activeSceneChanged += MatchMusicToScene;
        SeasonManager.SeasonChange += ChangeAmbienceTrack;
        UIEventAnnounceManager.announcement += PlayAnnouncementAlert;

        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update.
    void Start()
    {
        UpdateAudioLevels();
        // Music playback can be started here.
        //StartPeaceMusic();
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

        s.source.Play();
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
        if (musicLoop != null)
            StopCoroutine(musicLoop);
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
        switch(type)
        {
            case SceneMusicType.mainMenu:
                mainMenuMusic.source.Play();
                break;
            case SceneMusicType.peace:
                musicQueue = new MusicQueue(peaceMusic);
                musicSource = GetComponent<AudioSource>();
                musicLoop = StartCoroutine(musicQueue.LoopMusic(this, 0, PlayMusicClip));
                break;
            case SceneMusicType.combat:
                musicQueue = new MusicQueue(combatMusic);
                musicSource = GetComponent<AudioSource>();
                musicLoop = StartCoroutine(musicQueue.LoopMusic(this, 0, PlayMusicClip));
                break;
        }
    }

    public void MatchMusicToScene(Scene current, Scene next)
    {
        //Debug.Log("Changing scene type to " + CurrentSceneType.SceneType + "WHY DO I NOT CHANGE THE MUSIC!?");

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

    public void ChangeAmbienceTrack()
    {
        PlayAmbienceTrack(SeasonManager.Instance.currentSeason);
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
            PlaySound("ResourceDepleted");
        }
        else if (value <= lowResourceThreshold)
        {
            PlaySound("ResourceLow");
        }
        else
        {

        }
    }

    public void PlayAnnouncementAlert()
    {
        PlaySound("Announcement");
    }
}

[Serializable]
public enum SceneMusicType
{
    mainMenu,
    combat,
    peace,
}
