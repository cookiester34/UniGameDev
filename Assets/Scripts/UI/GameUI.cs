using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    private static GameUI _instance;

    public static GameUI Instance => _instance;

    [SerializeField] private GameObject[] hideOnGameOver;
    [SerializeField] private GameObject gameOver;

    private void Awake() {
        _instance = this;
    }

    public void ShowGameOver() {
        gameOver.SetActive(true);
        foreach (GameObject o in hideOnGameOver) {
            o.SetActive(false);
        }
    }

}
