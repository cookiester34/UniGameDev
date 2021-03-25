using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class of settings so that we can access settings anywhere required
/// </summary>
public static class Settings {
    public static FloatSetting CameraPanSpeed = new FloatSetting("CameraPanSpeed", 5f);
    public static FloatSetting CameraRotateStrength = new FloatSetting("CameraRotateStrength", 35f);
    public static BoolSetting CanMousePan = new BoolSetting("CanMousePan", true);
    public static BoolSetting CanMouseAccelerate = new BoolSetting("CanMousePanAccelerate", false);
    public static FloatSetting MasterVolume = new FloatSetting("MasterVolume", 1f);
    public static FloatSetting MusicVolume = new FloatSetting("MusicVolume", 0.11f);
    public static FloatSetting EffectsVolume = new FloatSetting("EffectsVolume", 1f);
    public static FloatSetting UiVolume = new FloatSetting("UIVolume", 1f);
}

public abstract class Setting<T> {
    /// <summary>
    /// The id string for the setting to be saved as, no two settings should have the same ID string
    /// </summary>
    protected string settingId;
    
    /// <summary>
    /// The initial value of the setting before the player has potentially changed it
    /// </summary>
    protected T initialValue;
    
    /// <summary>
    /// The current value of the setting
    /// </summary>
    public T Value;

    protected Setting(string settingId, T initialValue) {
        this.settingId = settingId;
        this.initialValue = initialValue;
        InitValue();
    }

    /// <summary>
    /// A way to initialise the value to either its initial if the setting does not exist, or its value if it does
    /// </summary>
    public abstract void InitValue();
    
    /// <summary>
    /// Updates the value of the setting, and its saved value
    /// </summary>
    /// <param name="newValue">The new value for the setting</param>
    public abstract void SetValue(T newValue);
}

public class FloatSetting : Setting<float> {
    public FloatSetting(string settingId, float initialValue) : base(settingId, initialValue) { }

    public override void InitValue() {
        if (!PlayerPrefs.HasKey(settingId)) {
            PlayerPrefs.SetFloat(settingId, initialValue);
            Value = initialValue;
        } else {
            Value = PlayerPrefs.GetFloat(settingId);
        }
    }

    public override void SetValue(float newValue) {
        PlayerPrefs.SetFloat(settingId, newValue);
        Value = newValue;
    }

}

public class BoolSetting : Setting<bool> {
    public BoolSetting(string settingId, bool initialValue) : base(settingId, initialValue) { }

    public override void InitValue() {
        if (!PlayerPrefs.HasKey(settingId)) {
            PlayerPrefsBool.SetBool(settingId, initialValue);
            Value = initialValue;
        } else {
            Value = PlayerPrefsBool.GetBool(settingId);
        }
    }

    public override void SetValue(bool newValue) {
        PlayerPrefsBool.SetBool(settingId, newValue);
        Value = newValue;
    }
}
