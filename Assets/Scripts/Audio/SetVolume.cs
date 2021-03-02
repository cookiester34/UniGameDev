using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public float ConvertLevel(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    public void SetMasterLevel (float sliderValue)
    {
        float convertedLevel = ConvertLevel(sliderValue);
        mixer.SetFloat("MasterVol", convertedLevel);
    }

    public void SetMusicLevel (float sliderValue)
    {
        float convertedLevel = ConvertLevel(sliderValue);
        mixer.SetFloat("MusicVol", convertedLevel);
    }

    public void SetEffectsLevel (float sliderValue)
    {
        float convertedLevel = ConvertLevel(sliderValue);
        mixer.SetFloat("EffectsVol", convertedLevel);
    }

    public void SetUILevel (float sliderValue)
    {
        float convertedLevel = ConvertLevel(sliderValue);
        mixer.SetFloat("UIVol", convertedLevel);
    }
}
