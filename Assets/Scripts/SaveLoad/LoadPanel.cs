using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {
    [SerializeField] private Text text;
    [SerializeField] private Button button;

    private void Awake() {
        if (text == null) {
            Debug.LogError("LoadPanel is missing its text component");
        }

        if (button == null) {
            Debug.LogError("LoadPanel is missing its button component");
        }
        
        SetupButtonListeners();
    }

    public void SetText(string newText) {
        text.text = newText;
    }

    /// <summary>
    /// Sets up button listeners
    /// </summary>
    void SetupButtonListeners() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// Load the save data
    /// </summary>
    void OnButtonClick() {
        SaveLoad.Load(text.text);
    }
}
