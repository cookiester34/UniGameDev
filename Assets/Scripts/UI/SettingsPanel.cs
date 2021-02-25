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


    #region settingsStrings

    public const string CameraPanSpeed = "cameraPanSpeed";
    public const string CameraMousePan = "cameraMousePan";
    public const string MasterVolume = "MasterVolume";
    public const string MusicVolume = "MusicVolume";
    public const string EffectsVolume = "EffectsVolume";
    public const string UIVolume = "UIVolume";
    
    private const float CameraPanSpeedDefault = 5f;
    private const bool CanCameraMousePanDefault = true;
    private const float MasterVolumeDefault = 1f;
    private const float MusicVolumeDefault = 0.11f;
    private const float EffectsVolumeDefault = 1f;
    private const float UIVolumeDefault = 1f;

    #endregion

    private void Awake() {
        InitialiseSettings();
        InitialiseUiComponents();
    }

    /// <summary>
    /// Checks if the settings exist, if not, sets them to default values
    /// </summary>
    private void InitialiseSettings() {
        if (!PlayerPrefs.HasKey(CameraPanSpeed)) {
            PlayerPrefs.SetFloat(CameraPanSpeed, CameraPanSpeedDefault);
        }

        if (!PlayerPrefs.HasKey(CameraMousePan)) {
            PlayerPrefsBool.SetBool(CameraMousePan, CanCameraMousePanDefault);
        }

        if (!PlayerPrefs.HasKey(MasterVolume)) {
            PlayerPrefs.SetFloat(MasterVolume, MasterVolumeDefault);
        }

        if (!PlayerPrefs.HasKey(MusicVolume)) {
            PlayerPrefs.SetFloat(MusicVolume, MusicVolumeDefault);
        }

        if (!PlayerPrefs.HasKey(EffectsVolume)) {
            PlayerPrefs.SetFloat(EffectsVolume, EffectsVolumeDefault);
        }

        if (!PlayerPrefs.HasKey(UIVolume)) {
            PlayerPrefs.SetFloat(UIVolume, UIVolumeDefault);
        }
    }

    /// <summary>
    /// Set UI components to the values from settings
    /// </summary>
    private void InitialiseUiComponents() {
        cameraSpeedSlider.value = PlayerPrefs.GetFloat(CameraPanSpeed);
        cameraMousePan.isOn = PlayerPrefsBool.GetBool(CameraMousePan);
        masterSlider.value = PlayerPrefs.GetFloat(MasterVolume);
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolume);
        effectsSlider.value = PlayerPrefs.GetFloat(EffectsVolume);
        uiSlider.value = PlayerPrefs.GetFloat(UIVolume);        
    }

    /// <summary>
    /// Updates the settings using the values set in the panel
    /// </summary>
    public void SaveSettings() {
        PlayerPrefs.SetFloat(CameraPanSpeed, cameraSpeedSlider.value);
        PlayerPrefsBool.SetBool(CameraMousePan, cameraMousePan.isOn);
        PlayerPrefs.SetFloat(MasterVolume, masterSlider.value);
        PlayerPrefs.SetFloat(MusicVolume, musicSlider.value);
        PlayerPrefs.SetFloat(EffectsVolume, effectsSlider.value);
        PlayerPrefs.SetFloat(UIVolume, uiSlider.value);
    }
}
