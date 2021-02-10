using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour {
    /// <summary>
    /// Slider component
    /// </summary>
    private Slider _slider;

    /// <summary>
    /// The target for the progress, allows the progress to smoothly increment
    /// </summary>
    private float _targetProgress;
    
    /// <summary>
    /// The speed at which to fill the progress bar
    /// </summary>
    public float fillSpeed = 0.5f;

    public float TargetProgress {
        get => _targetProgress;
        set => _targetProgress = value;
    }

    private void Awake() {
        _slider = GetComponent<Slider>();
        if (_slider == null) {
            Debug.LogError("Progress bar is missing its slider component");
        }

        _slider.value = 0;
        _targetProgress = 0;
    }

    /// <summary>
    /// Smoothly move the progess towards the target
    /// </summary>
    private void Update() {
        if (_slider.value < _targetProgress) {
            _slider.value += fillSpeed * Time.deltaTime;
            _slider.value = Mathf.Clamp(_slider.value, 0, _targetProgress);
        }
    }
}
