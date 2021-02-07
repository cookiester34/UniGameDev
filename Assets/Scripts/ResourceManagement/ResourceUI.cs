using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {
    [SerializeField] private Resource resource;

    private Text _text;
    // Start is called before the first frame update
    void Awake() {
        if (resource == null) {
            Debug.LogError("Forgot to assign a resource to the resource UI");
            return;
        }

        _text = GetComponent<Text>();

        resource.OnCurrentValueChanged += UpdateText;
        UpdateText(resource.currentResourceAmount);
    }

    void UpdateText(int newValue) {
        _text.text = resource.name + ": " + newValue + "/" + resource.resourceCap;
    }
}
