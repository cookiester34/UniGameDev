using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using Util;

public class SceneManagement : MonoBehaviour {
    public delegate void OnSceneLoaded();
    public event OnSceneLoaded SceneLoaded;
    public Animator loadScreenAnimator;

    private static SceneManagement _instance = null;
    private static readonly int Load = Animator.StringToHash("Load");
    private static readonly int Unload = Animator.StringToHash("Unload");
    private bool _isLoading = false;

    public static SceneManagement Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.SceneSingleton);
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

    private void Start() {
        if (EntryTracker.VisitedMainMenu) {
            loadScreenAnimator.SetTrigger(Load);
            loadScreenAnimator.ResetTrigger(Unload);
        }
    }

    public void LoadScene(string sceneName) {
        if (!_isLoading) {
            _isLoading = true;
            StartCoroutine(SceneLoad(sceneName));
        }
    }

    /// <summary>
    /// Loads the specified scene, can receive a callback by listening to the SceneLoaded event
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    private IEnumerator SceneLoad(string sceneName) {
        loadScreenAnimator.SetTrigger(Unload);
        loadScreenAnimator.ResetTrigger(Load);
        yield return new WaitForSecondsRealtime(2);
        var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        ao.allowSceneActivation = false;
        ao.completed += delegate {
            _isLoading = false;
            SceneType newType = SceneType.Main;
            newType.FromScene(sceneName);
            CurrentSceneType.SceneType = newType;
            SceneLoaded?.Invoke();
        };

        while (!ao.isDone) {
            if (ao.progress >= 0.9f) {
                ao.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
