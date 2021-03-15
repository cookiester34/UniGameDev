using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
    [SerializeField] private GameObject fogOfWar;
    
    private void Awake() {
        fogOfWar.SetActive(CurrentSceneType.SceneType == SceneType.GameLevel);
    }
}
