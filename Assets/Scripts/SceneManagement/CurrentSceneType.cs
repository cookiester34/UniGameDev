using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple small class to get the current scene type, use to have different functionality between scene types
/// </summary>
public static class CurrentSceneType {
    private static SceneType _sceneType = SceneType.Main;

    public static SceneType SceneType {
        get => _sceneType;
        set {
            if (_sceneType == SceneType.GameLevel && value == SceneType.LevelEditor) {
                Debug.Log("Cannot change from game level to level editor");
                return;
            }
            _sceneType = value;
            Debug.Log("Scene type changed to: " + _sceneType);
        }
    }

    static CurrentSceneType() {
        _sceneType.FromScene(SceneManager.GetActiveScene().name);
        Debug.Log("Scene type changed to: " + _sceneType);
    }
}
