using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

/// <summary>
/// Settings panel controls setting up and reading values from settings
/// </summary>
public class SettingsPanel : MonoBehaviour {
    
    [SerializeField] private Slider cameraSpeedSlider;
    [SerializeField] private Slider cameraRotateStrengthSlider;
    [SerializeField] private Toggle cameraMousePan;
    [SerializeField] private Toggle cameraAcceleration;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private ResolutionDropdown resolution;

    private void Start() {
        InitialiseUiComponents();
    }

    /// <summary>
    /// Set UI components to the values from settings
    /// </summary>
    private void InitialiseUiComponents() {
        cameraSpeedSlider.value = Settings.CameraPanSpeed.Value;
        cameraRotateStrengthSlider.value = Settings.CameraRotateStrength.Value;
        cameraMousePan.isOn = Settings.CanMousePan.Value;
        cameraAcceleration.isOn = Settings.CanMouseAccelerate.Value;
        masterSlider.value = Settings.MasterVolume.Value;
        musicSlider.value = Settings.MusicVolume.Value;
        effectsSlider.value = Settings.EffectsVolume.Value;
        uiSlider.value = Settings.UiVolume.Value;
        quality.value = Settings.Quality.Value;
        fullscreen.isOn = Settings.Fullscreen.Value;
        resolution.SetToResolution(Settings.xResolution.Value, Settings.yResolution.Value);
    }

    /// <summary>
    /// Updates the settings using the values set in the panel
    /// </summary>
    public void SaveSettings() {
        Settings.CameraPanSpeed.SetValue(cameraSpeedSlider.value);
        Settings.CameraRotateStrength.SetValue(cameraRotateStrengthSlider.value);
        Settings.CanMousePan.SetValue(cameraMousePan.isOn);
        Settings.CanMouseAccelerate.SetValue(cameraAcceleration.isOn);
        Settings.MasterVolume.SetValue(masterSlider.value);
        Settings.MusicVolume.SetValue(musicSlider.value);
        Settings.EffectsVolume.SetValue(effectsSlider.value);
        Settings.UiVolume.SetValue(uiSlider.value);
        Settings.Quality.SetValue(quality.value);
        Settings.Fullscreen.SetValue(fullscreen.isOn);
        Tuple<int, int> currentResolution = resolution.GetCurrentResolution();
        Settings.xResolution.SetValue(currentResolution.Item1);
        Settings.yResolution.SetValue(currentResolution.Item2);

        AudioManager.Instance.UpdateAudioLevels();
        LoadSavedSettings.LoadSettings();
    }

    public void ApplySettings() {
        QualitySettings.SetQualityLevel(Settings.Quality.Value, true);
    }
}
