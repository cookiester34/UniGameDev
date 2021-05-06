using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
    [SerializeField] private GameObject fogOfWar;
    private bool _enabled;
    
    private static FogOfWar instance;

    public bool Enabled => _enabled;

    public static FogOfWar Instance => instance;

    private void Awake() {
        if (instance != null) {
            Destroy(instance);
            Debug.LogWarning("2nd instance of singleton created, destroyed");
            return;
        }

        instance = this;
        _enabled = CurrentSceneType.SceneType == SceneType.GameLevel;
        fogOfWar.SetActive(_enabled);
    }

    public void Enable(bool enable) {
        _enabled = enable;
        fogOfWar.SetActive(enable);
    }
}
