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
    
    #region settingsStrings

    public const string CameraPanSpeed = "cameraPanSpeed";
    public const string CameraMousePan = "cameraMousePan";
    
    private const float CameraPanSpeedDefault = 5f;
    private const bool CanCameraMousePanDefault = true;

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
    }

    /// <summary>
    /// Set UI components to the values from settings
    /// </summary>
    private void InitialiseUiComponents() {
        cameraSpeedSlider.value = PlayerPrefs.GetFloat(CameraPanSpeed);
        cameraMousePan.isOn = PlayerPrefsBool.GetBool(CameraMousePan);
    }

    /// <summary>
    /// Updates the settings using the values set in the panel
    /// </summary>
    public void SaveSettings() {
        PlayerPrefs.SetFloat(CameraPanSpeed, cameraSpeedSlider.value);
        PlayerPrefsBool.SetBool(CameraMousePan, cameraMousePan.isOn);
    }
}
