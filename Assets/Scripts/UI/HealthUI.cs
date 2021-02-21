using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour {
    [SerializeField] private Health health;

    private ProgressBar _progressBar;

    private void Awake() {
        if (health == null) {
            health = GetComponentInParent<Health>();
        }

        if (health == null) {
            Debug.LogError("Health UI relies on health component but is not setup");
            return;
        } 
        
        _progressBar = GetComponent<ProgressBar>();
        health.OnHealthGain += UpdateHealthBar;
        health.OnHealthLost += UpdateHealthBar;
        health.OnDeath += UpdateHealthBar;
        _progressBar.ColourThresholds = new List<ColourThreshold> {
            new ColourThreshold(Color.green, 1f),
            new ColourThreshold(Color.yellow, 0.4f),
            new ColourThreshold(Color.red, 0.1f)
        };
    }

    private void Start() {
        _progressBar.SetSliderValue(health.NormalizedHealth, true);
    }

    private void UpdateHealthBar() {
        _progressBar.TargetProgress = health.NormalizedHealth;
    }
}
