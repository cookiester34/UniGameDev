using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Settings panel controls setting up and reading values from settings
/// </summary>
public class SettingsPanel : MonoBehaviour {
    
    [SerializeField] private Slider cameraSpeedSlider;
    [SerializeField] private Toggle cameraMousePan;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider uiSlider;

    private void Awake() {
        InitialiseUiComponents();
    }

    /// <summary>
    /// Set UI components to the values from settings
    /// </summary>
    private void InitialiseUiComponents() {
        cameraSpeedSlider.value = Settings.CameraPanSpeed.Value;
        cameraMousePan.isOn = Settings.CanMousePan.Value;
        masterSlider.value = Settings.MasterVolume.Value;
        musicSlider.value = Settings.MusicVolume.Value;
        effectsSlider.value = Settings.EffectsVolume.Value;
        uiSlider.value = Settings.UiVolume.Value;
    }

    /// <summary>
    /// Updates the settings using the values set in the panel
    /// </summary>
    public void SaveSettings() {
        Settings.CameraPanSpeed.SetValue(cameraSpeedSlider.value);
        Settings.CanMousePan.SetValue(cameraMousePan.isOn);
        Settings.MasterVolume.SetValue(masterSlider.value);
        Settings.MusicVolume.SetValue(musicSlider.value);
        Settings.EffectsVolume.SetValue(effectsSlider.value);
        Settings.UiVolume.SetValue(uiSlider.value);

        AudioManager.Instance.UpdateAudioLevels();
    }
}
