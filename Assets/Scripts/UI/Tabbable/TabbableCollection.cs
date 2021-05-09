using UnityEngine;

/// <summary>
/// A collection that holds multiple tabbable contents, ensures only one is active at a time
/// </summary>
public class TabbableCollection : MonoBehaviour {
    [SerializeField] private TabbableContent[] contents;
    [SerializeField] private TabbableContent beginningActiveContent;
    private TabbableContent _activeContent = null;

    public TabbableContent ActiveContent {
        set => _activeContent = value;
    }

    private void Start() {
        foreach (TabbableContent content in contents) {
            content.Hide();
        }
        
        if (beginningActiveContent != null) {
            MakeActive(beginningActiveContent);
        }
    }

    public void MakeActive(TabbableContent content) {
        if (_activeContent != null) {
            _activeContent.gameObject.SetActive(false);
        }
        content.gameObject.SetActive(content != _activeContent);
    }

    public void Deactivate(TabbableContent content) {
        content.gameObject.SetActive(false);
    }
}
