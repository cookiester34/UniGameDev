using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResourceValues : MonoBehaviour {
    [SerializeField] private Resource pollenResource;
    [SerializeField] private Resource honeyResource;
    [SerializeField] private Resource waxResource;
    [SerializeField] private Resource royalJellyResource;
    [SerializeField] private Resource populationResource;

    public Text pollenText;
    public Text honeyText;
    public Text jellyText;
    public Text waxText;
    public Text populationText;

    private void Awake() {
        SetupListeners();
        InitialiseText();
    }

    /// <summary>
    /// Sets up listeners so that when the value of a resource changes then so will its text display
    /// </summary>
    private void SetupListeners() {
        pollenResource.OnCurrentValueChanged += UpdatePollenText;
        honeyResource.OnCurrentValueChanged += UpdateHoneyText;
        royalJellyResource.OnCurrentValueChanged += UpdateRoyalJellyText;
        waxResource.OnCurrentValueChanged += UpdateWaxText;
        populationResource.OnCurrentValueChanged += UpdatePopulationText;
    }

    private void InitialiseText() {
        pollenText.text = "Pollen " + pollenResource.currentResourceAmount;
        honeyText.text = "Honey " + honeyResource.currentResourceAmount;
        jellyText.text = "Royal Jelly " + royalJellyResource.currentResourceAmount;
        waxText.text = "Wax " + waxResource.currentResourceAmount;
        populationText.text = "Population: " + populationResource.currentResourceAmount;
    }

    private void UpdatePollenText() {
        pollenText.text = "Pollen " + pollenResource.currentResourceAmount;
    }

    private void UpdateHoneyText() {
        honeyText.text = "Honey " + honeyResource.currentResourceAmount;
    }

    private void UpdateRoyalJellyText() {
        jellyText.text = "Royal Jelly " + royalJellyResource.currentResourceAmount;
    }

    private void UpdateWaxText() {
        waxText.text = "Wax " + waxResource.currentResourceAmount;
    }

    private void UpdatePopulationText() {
        populationText.text = "Population: " + populationResource.currentResourceAmount;
    }
}
