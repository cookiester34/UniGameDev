using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxUIMenu : MonoBehaviour {
    [SerializeField] private Toggle fogOfWar;
    [SerializeField] private Dropdown currentSeason;
    [SerializeField] private Slider seasonLength;

    private void Awake() {
        if (fogOfWar == null) {
            Debug.LogError("SandboxUIMenu not setup correctly, expects fogOfWar toggle");
        }
        
        if (currentSeason == null) {
            Debug.LogError("SandboxUIMenu not setup correctly, expects current season dropdown");
        }
        
        if (seasonLength == null) {
            Debug.LogError("SandboxUIMenu not setup correctly, expects season length slider");
        }
    }

    private void OnEnable() {
        Seasons season = SeasonManager.Instance.CurrentSeason;
        currentSeason.value = (int) season;

        seasonLength.value = SeasonManager.Instance.seasonLength;
        
        fogOfWar.isOn = FogOfWar.Instance.Enabled;
    }

    private void OnDisable() {
        //SeasonManager.Instance.CurrentSeason = (Seasons) currentSeason.value;
        SeasonManager.Instance.seasonLength = (int)seasonLength.value;
        SeasonManager.Instance.SetCurrentSeason((Seasons)currentSeason.value);
        FogOfWar.Instance.Enable(fogOfWar.isOn);
    }
}
