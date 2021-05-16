using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small class to have a button use the value from some text to modify the resources
/// </summary>
[RequireComponent(typeof(Button))]
public class UIResourceDebugButton : MonoBehaviour {
    [SerializeField] private TMP_InputField text;
    [SerializeField] private Resource resource;
    [SerializeField] private Toggle resourceToggle;
    [SerializeField] private Toggle resourceCapToggle;
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        if (_button == null) {
            Debug.LogError("UIResourceDebugButton component has been added to an object with no button");
            return;
        }

        if (text == null) {
            Debug.LogError("UIResourceDebugButton relies on a text field and it ahs not been setup");
            return;
        }

        _button.onClick.AddListener(() => {
            if (int.TryParse(text.text, out int value)) {
                if (resourceToggle.isOn) {
                    resource.ModifyAmount(value);
                } else if (resourceCapToggle.isOn) {
                    resource.ModifyCap(value);
                }
            }
        });
    }
}
