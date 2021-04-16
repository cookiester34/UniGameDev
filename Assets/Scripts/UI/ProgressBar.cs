using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour {
    /// <summary>
    /// Slider component
    /// </summary>
    private Slider _slider;

    /// <summary>
    /// Fill component of the progress bar
    /// </summary>
    [SerializeField] private Image fill;
    
    /// <summary>
    /// Potential colours to use for the slider
    /// </summary>
    private List<ColourThreshold> _colourThresholds;

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

    public List<ColourThreshold> ColourThresholds {
        set {
            _colourThresholds = value;
            _colourThresholds.Sort((threshold1, threshold2) => 
                (int) ((threshold1.threshold - threshold2.threshold) * 10f));
        }
    }

    private void Awake() {
        _slider = GetComponent<Slider>();
        if (_slider == null) {
            Debug.LogError("Progress bar is missing its slider component");
        }

        if (fill == null) {
            Debug.LogError("Progress bar is missing its fill component");
        }

        _slider.interactable = false;
        _slider.value = 0;
        _targetProgress = 0;
    }

    /// <summary>
    /// Smoothly move the progress towards the target
    /// </summary>
    private void Update() {

        if ((_slider.value < _targetProgress) || (_slider.value > _targetProgress)) {
            var currentValue = _slider.value;

            // Fill direction used to determine whether the value should increase or decrease 
            float fillDirection = currentValue < _targetProgress ? 1f: -1f;
            float smoothValue = currentValue + (fillSpeed * Time.deltaTime * fillDirection);

            SetSliderValue(Mathf.Clamp(smoothValue, 0, _targetProgress));
        }
    }

    /// <summary>
    /// Changes the value of the slider 
    /// </summary>
    /// <param name="value">A value between 0 and 1</param>
    /// <param name="instant">Whether it should instantly happen causing the target progress to update</param>
    public void SetSliderValue(float value, bool instant = false) {
        if (instant) {
            _targetProgress = value;
        }

        _slider.value = Mathf.Clamp(value, 0, 1);
        UpdateColour();
    }
    
    /// <summary>
    /// Updates the colour of the fill component if colour thresholds have been supplied
    /// </summary>
    private void UpdateColour() {
        if (_colourThresholds != null) {
            foreach (ColourThreshold colourThreshold in _colourThresholds) {
                if (_slider.value < colourThreshold.threshold) {
                    fill.color = colourThreshold.color;
                    break;
                }
            }
        }
    }
}

/// <summary>
/// Util so that colours can be tied to a value
/// </summary>
public class ColourThreshold {
    /// <summary>
    /// The colour paired to the threshold
    /// </summary>
    public Color color;

    /// <summary>
    /// Threshold between 0 and 1 where the colour should be used
    /// </summary>
    public float threshold;

    public ColourThreshold(Color color, float threshold) {
        this.color = color;
        this.threshold = threshold;
    }
}
