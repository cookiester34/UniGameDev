using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A button that can be used as a cancel button to hide the menu
/// </summary>
[RequireComponent(typeof(Button))]
public class UICancelButton : MonoBehaviour {
    /// <summary>
    /// The menu that should be cancelled when the button is clicked
    /// </summary>
    [Tooltip("The menu that should be cancelled when the cancel button is clicked")]
    [SerializeField] private GameObject cancelObject;
    private Button _button;
    
    private void Awake() {
        _button = GetComponent<Button>();

        if (cancelObject == null) {
            Debug.LogWarning("Cancel button missing an object to cancel when it is clicked");
        }
    }

    private void OnEnable() {
        _button.onClick.AddListener(CancelUi);
    }

    private void OnDisable() {
        _button.onClick.RemoveListener(CancelUi);
    }

    void CancelUi() {
        cancelObject.SetActive(false);
    }
}
