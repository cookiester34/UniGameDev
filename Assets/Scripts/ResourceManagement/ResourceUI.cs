using System;
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
        resource.OnCapChanged += UpdateText;
        UpdateText(resource.CurrentResourceAmount);
    }

    private void OnDestroy() {
        resource.OnCurrentValueChanged -= UpdateText;
        resource.OnCapChanged -= UpdateText;
    }

    void UpdateText(float newValue) {
        _text.text =
            String.Format("{0}: {1:0} / {2}", resource.name, resource.GetFloorCurrentAmount(), resource.ResourceCap);
        if (newValue <= resource.ResourceLowThreshold)
        {
            _text.color = Color.red;
        }
        else
        {
            _text.color = Color.black;
        }

        float currentResourceTickAmount = resource.GetResourceTickAmount();

        //rounding to 2 decimal places
        float mult = Mathf.Pow(10f, 2f);
        currentResourceTickAmount = Mathf.Round(currentResourceTickAmount * mult) / mult;

        if (currentResourceTickAmount > 0)
            _text.text += "   +" + currentResourceTickAmount;
        else
            _text.text += "   " + currentResourceTickAmount;
    }
}
