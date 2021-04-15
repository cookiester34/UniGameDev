using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplicationUtil {
    /// <summary>
    /// Used to check if the application is currently quitting, strangely not already supported by application
    /// </summary>
    private static bool _isQuitting = false;
    private static bool _isLoading = false;
    public static bool IsQuitting => _isQuitting;
    public static bool IsLoading => _isLoading;
    
    static ApplicationUtil() {
        Application.quitting += () => {
            _isQuitting = true;
        };

        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    static void SceneLoaded(Scene scene, LoadSceneMode sceneMode) {
        _isLoading = false;
    }
    
    static void SceneUnloaded(Scene scene) {
        _isLoading = true;
    }
}
