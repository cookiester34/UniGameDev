using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadMonoWrapper : MonoBehaviour {
    /// <summary>
    /// Text component to rely on for savename
    /// </summary>
    [SerializeField] private Text text;

    private void Awake() {
        if (text == null) {
            Debug.LogWarning("A SaveLoadMonoWrapper is missing its text component, attempt to save will fail," +
                             " text is required to know what to call the save");
        }
    }

    /// <summary>
    /// Save the game using the text in the text field for its name
    /// </summary>
    public void Save() {
        if (text == null) {
            Debug.LogError("Save called on SaveLoadMonoWrapper, but its text field is not setup, save aborted");
            return;
        }

        string currentText = text.text;
        if (string.IsNullOrEmpty(currentText)) {
            Debug.LogError("Trying to save game with no name");
        } else {
            SaveLoad.CreateSaveFromScene(currentText);
        }
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void ToMainMenu() {
        SceneManagement.Instance.LoadScene("Main");
    }

    /// <summary>
    /// Returns to desktop quitting the game
    /// </summary>
    public void ToDesktop() {
        Application.Quit();
    }
}
