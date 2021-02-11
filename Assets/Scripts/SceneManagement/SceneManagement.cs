using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class SceneManagement : MonoBehaviour {
    private static SceneManagement _instance = null;

    public static SceneManagement Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.EventAnnounceSingleton);
                Instantiate(go);
            }

            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("Attempted to create 2nd Scene management singleton, will destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() {
        Application.Quit();
    }
}
