using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Belongs as part of a tabbable collection, and has a button that enables its content
/// </summary>
public class TabbableContent : MonoBehaviour {
    [Tooltip("The collection this content belongs to")]
    [SerializeField] private TabbableCollection collection;
    
    [Tooltip("Whether this contents button should be hidden when this content is")]
    [SerializeField] private bool hideButtonOnDismiss;
    
    [Tooltip("The button to show this content")]
    [SerializeField] private Button associatedButton;
    
    public event Action OnHide;

    private void Awake() {
        
        if (collection == null) {
            Debug.LogWarning("TabbableContent misused, requires a collection to belong to");
        }
        
        if (associatedButton == null) {
            Debug.LogWarning("TabbableContent misused, requires a button to control its visibility");
        }
    }

    public void Toggle() {
        if (gameObject.activeSelf) {
            Hide();
        } else {
            Show();
        }
    }

    public void Hide() {
        collection.Deactivate(this);
        if (hideButtonOnDismiss) {
            associatedButton.gameObject.SetActive(false);
        }
    }

    public void Show() {
        collection.MakeActive(this);
    }

    private void OnEnable() {
        collection.ActiveContent = this;
        if (hideButtonOnDismiss) {
            associatedButton.gameObject.SetActive(true);
        }
    }

    private void OnDisable() {
        collection.ActiveContent = null;
        if (hideButtonOnDismiss) {
            associatedButton.gameObject.SetActive(false);
        }
        OnHide?.Invoke();
    }
}
