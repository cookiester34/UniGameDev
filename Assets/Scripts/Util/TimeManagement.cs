using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManagement : MonoBehaviour {
    [SerializeField] private Button timePause;
    [SerializeField] private Button timeDefault;
    [SerializeField] private Button timeDouble;

    private void Start() {
        timeDefault.interactable = false;
    }

    public void SetTimeRate(float timeScale) {
        Time.timeScale = timeScale;
        timePause.interactable = !(Math.Abs(timeScale) < 0.05f);
        timeDefault.interactable = !(Math.Abs(Math.Abs(timeScale) - 1f) < 0.05f);
        timeDouble.interactable = !(Math.Abs(Math.Abs(timeScale) - 2f) < 0.05f);
    }
}
