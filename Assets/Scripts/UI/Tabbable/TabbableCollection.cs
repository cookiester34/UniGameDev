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
            MakeActive(beginningActiveContent, true);
        }
    }

    /// <summary>
    /// Unable to use default parameters with unity events, hence this exists
    /// </summary>
    /// <param name="content"></param>
    public void MakeActive(TabbableContent content) {
        MakeActive(content, false);
    }
    
    public void MakeActive(TabbableContent content, bool force) {
        if (!force) {
            if (_activeContent != content) {
                if (_activeContent != null) {
                    _activeContent.gameObject.SetActive(false);
                }

                content.gameObject.SetActive(true);
            } else {
                if (_activeContent != null) {
                    _activeContent.gameObject.SetActive(false);
                }
            }
        } else {
            if (_activeContent != content) {
                if (_activeContent != null) {
                    _activeContent.gameObject.SetActive(false);
                }
                content.gameObject.SetActive(true);
            }
        }
    }

    public void Deactivate(TabbableContent content) {
        content.gameObject.SetActive(false);
    }
}
