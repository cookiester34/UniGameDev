using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {
    [SerializeField] private Text text;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    private string _savename;
    public string Savename => _savename;

    private void Awake() {
        if (text == null) {
            Debug.LogError("LoadPanel is missing its text component");
        }

        if (loadButton == null) {
            Debug.LogError("LoadPanel is missing its load button component");
        }

        if (deleteButton == null) {
            Debug.LogError("LoadPanel is missing its delete button component");
        }
        
        SetupButtonListeners();
    }

    public void SetText(string newText) {
        text.text = newText;
        _savename = newText;
    }

    /// <summary>
    /// Sets up button listeners
    /// </summary>
    void SetupButtonListeners() {
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(OnLoadButtonClick);
        
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(OnDeleteButtonClick);
    }

    /// <summary>
    /// Load the save data
    /// </summary>
    void OnLoadButtonClick() {
        SaveLoad.Load(text.text);
    }

    /// <summary>
    /// Delete the save and destroy this gameobject
    /// </summary>
    void OnDeleteButtonClick() {
        SaveLoad.DeleteSave(text.text);
        Destroy(gameObject);
    }
}
