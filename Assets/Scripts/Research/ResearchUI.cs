using System.Collections;
using System.Collections.Generic;
using Research;
using UI;
using UnityEngine;

public class ResearchUI : UiHoverable {
    protected override void Awake() {
        base.Awake();

        _button.interactable = true;
        if (_scriptableObject is ResearchObject researchObject) {
            researchObject.OnResearchStarted += () => _button.interactable = false;
        } else {
            Debug.LogError("Research UIs scriptable object is not of type ResearchObject");
        }
    }
}
