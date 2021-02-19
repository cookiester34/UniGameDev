using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {
    
    [SerializeField] private Slider cameraSpeedSlider;
    
    #region settingsStrings

    public const string CameraPanSpeed = "cameraPanSpeed";
    private const float CameraPanSpeedDefault = 5f;

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
    }

    /// <summary>
    /// Set UI components to the values from settings
    /// </summary>
    private void InitialiseUiComponents() {
        cameraSpeedSlider.value = PlayerPrefs.GetFloat(CameraPanSpeed);
    }

    /// <summary>
    /// Updates the settings using the values set in the panel
    /// </summary>
    public void SaveSettings() {
        PlayerPrefs.SetFloat(CameraPanSpeed, cameraSpeedSlider.value);
    }
}
