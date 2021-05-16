using Research;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchUI : UiHoverable, IPointerEnterHandler {
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private ProgressBar progressBar;
    private ResearchObject _researchObject;
    private bool _isResearchStarted = false;

    protected override void Awake() {
        base.Awake();

        _button.interactable = true;
        if (_scriptableObject is ResearchObject) {
            _researchObject = (ResearchObject) _scriptableObject;
            _researchObject.OnResearchStarted += OnResearchStart;
            _researchObject.OnResearchFinished += OnResearchFinish;
        } else {
            Debug.LogError("Research UIs scriptable object is not of type ResearchObject");
        }

        if (progressBar == null) {
            Debug.Log("Research UI is missing its progress bar, will not show progress.");
        }

        if (descriptionText == null) {
            Debug.LogError("Research UI is missing a reference to the description text, its description will not be shown");
        }
    }

    private void OnResearchStart() {
        _button.interactable = false;
        _isResearchStarted = true;
        progressBar.gameObject.SetActive(true);
    }

    private void OnResearchFinish() {
        //_isResearchStarted = false;
        _button.interactable = false;
        progressBar.gameObject.SetActive(false);
    }

    private void Update() {
        if (progressBar.isActiveAndEnabled) {
            progressBar.TargetProgress = _researchObject.ResearchProgress / 100;
        }

        if (!_isResearchStarted && _researchObject)
        {
            _button.interactable = ResourceManagement.Instance.CanUseResources(_researchObject.Resources) && _researchObject.PrerequisitesMet();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        descriptionText.text = _researchObject.ResearchName + ":\n\n" + _researchObject.Description;
    }
}
