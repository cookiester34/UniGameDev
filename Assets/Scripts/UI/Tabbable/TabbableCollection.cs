using UnityEngine;

/// <summary>
/// A collection that holds multiple tabbable contents, ensures only one is active at a time
/// </summary>
public class TabbableCollection : MonoBehaviour {
    [SerializeField] private TabbableContent[] _contents;
    [SerializeField] private TabbableContent beginningActiveContent;
    private TabbableContent activeContent = null;

    public TabbableContent ActiveContent {
        set => activeContent = value;
    }

    private void Start() {
        foreach (TabbableContent content in _contents) {
            content.Hide();
        }
        
        if (beginningActiveContent != null) {
            MakeActive(beginningActiveContent);
        }
    }

    public void MakeActive(TabbableContent content) {
        if (activeContent != null) {
            activeContent.gameObject.SetActive(false);
        }
        content.gameObject.SetActive(content != activeContent);
    }

    public void Deactivate(TabbableContent content) {
        content.gameObject.SetActive(false);
    }
}
