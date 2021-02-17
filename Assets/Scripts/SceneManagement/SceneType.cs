using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SceneType {
    Main,
    LevelEditor,
    GameLevel
}

public static class SceneTypeExtensions {
    public static void FromScene(ref this SceneType sceneType, string sceneName) {
        switch (sceneName) {
            case "Joe":
            case "George":
            case "Peter":
            case "Sam":
            case "Ryan":
                sceneType = SceneType.LevelEditor;
                break;
            case "Main":
                sceneType = SceneType.Main;
                break;
            case "Flat":
                sceneType = SceneType.GameLevel;
                break;
            default:
                sceneType = SceneType.GameLevel;
                Debug.LogWarning("Scene name: " + sceneName + " not recognised, will class as a game level");
                break;
        }
    }
}
