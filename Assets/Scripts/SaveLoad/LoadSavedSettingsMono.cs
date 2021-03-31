using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSavedSettingsMono : MonoBehaviour {
    private void Awake() {
        LoadSavedSettings.LoadSettings();
    }
}

public static class LoadSavedSettings {
    public static void LoadSettings() {
        FullScreenMode fullScreenMode = Settings.Fullscreen.Value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(Settings.xResolution.Value, Settings.yResolution.Value, fullScreenMode);
        if (QualitySettings.GetQualityLevel() != Settings.Quality.Value) {
            QualitySettings.SetQualityLevel(Settings.Quality.Value, true);
        }
    }
}
