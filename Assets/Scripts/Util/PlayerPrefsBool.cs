using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class to make getting and setting bools simpler for player prefs
/// </summary>
public static class PlayerPrefsBool {
    private const int BoolTrue = 1;

    public static void SetBool(string key, bool value) {
        int val = value ? BoolTrue : 0; 
        PlayerPrefs.SetInt(key, val);
    }
    
    public static bool GetBool(string key) {
        int val = PlayerPrefs.GetInt(key);
        return val == BoolTrue;
    }
}
