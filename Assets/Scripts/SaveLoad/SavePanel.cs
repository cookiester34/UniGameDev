using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour {
    [SerializeField] private Text text;

    [SerializeField] private Button button;

    private void Awake() {
        if (text == null) {
            Debug.LogError("SavePanel is missing the text component");
        }
        if (button == null) {
            Debug.LogError("SavePanel is missing the button component");
            return;
        }

        SetupButtonListener();
    }

    /// <summary>
    /// Sets up button listeners
    /// </summary>
    void SetupButtonListener() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// Sav the scene if some text for the savename
    /// </summary>
    void OnButtonClick() {
        string currentText = text.text;
        if (string.IsNullOrEmpty(currentText)) {
            Debug.LogError("Trying to save game with no name");
        } else {
            SaveLoad.CreateSaveFromScene(currentText);
        }
    }
}
