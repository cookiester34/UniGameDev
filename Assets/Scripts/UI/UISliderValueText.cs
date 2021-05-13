using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class UISliderValueText : MonoBehaviour {
    [SerializeField] private Slider slider;
    private TMP_Text _text;

    private void Awake() {
        if (slider == null) {
            Debug.LogError("UI slider text is missing its slider, no reason for game object to exist");
            return;
        }

        _text = GetComponent<TMP_Text>();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText() {
        // Basically float ==, use - instead with abs due to float inaccuracy
        if (Math.Abs(float.Parse(_text.text) - slider.value) > 0.05f) {
            _text.text = String.Format("{0:0.00}", slider.value);
        }
    }
}
