using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UI;
using UnityEngine;

public class ResearchUI : UiHoverable {
    [SerializeField] private ProgressBar progressBar;
    private ResearchObject _researchObject;

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
    }

    private void OnResearchStart() {
        _button.interactable = false;
        progressBar.gameObject.SetActive(true);
    }

    private void OnResearchFinish() {
        progressBar.gameObject.SetActive(false);
    }

    private void Update() {
        if (progressBar.isActiveAndEnabled) {
            progressBar.TargetProgress = _researchObject.ResearchProgress / 100;
        }
    }
}
