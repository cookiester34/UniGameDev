using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBuildingUI : MonoBehaviour {
    [SerializeField] private Text assignedBeesText;
    [SerializeField] private Text resourcesText;
    [SerializeField] private Text nameText;
    [SerializeField] private Image icon;
    [SerializeField] private UIBottomBar bar;
    [SerializeField] private UIBottomBarButton thisTabbableButton;
    [SerializeField] private Button upgradeBuildingButton;
    [SerializeField] private Button assignBeeButton;
    [SerializeField] private Button unassignBeeButton;
    private Building _selectedBuilding;

    private void Awake() {
        if (!assignedBeesText) {
            Debug.LogWarning("Selected building UI missing assigned bees text");
        }

        if (!resourcesText) {
            Debug.LogWarning("Selected building UI resources text not setup");
        }

        if (!nameText) {
            Debug.LogWarning("Selected Building name text not assigned");
        }

        if (!icon) {
            Debug.LogWarning("Selected Building icon image not assigned");
        }

        if (!upgradeBuildingButton) {
            Debug.LogWarning("Selected Building UI upgrade building button not assigned");
        }

        BuildingManager.Instance.OnBuildingSelected += UpdateDisplay;
    }

    private void UpdateDisplay(Building building) {
        _selectedBuilding = building;
        thisTabbableButton.gameObject.SetActive(building != null);
        if (building == null) {
            thisTabbableButton.ActivateContents(false);
            return;
        }

        bar.MakeContentsActive(thisTabbableButton);
        
        upgradeBuildingButton.interactable = _selectedBuilding.CanUpgrade();
        assignBeeButton.interactable =
            _selectedBuilding.numAssignedBees < _selectedBuilding.BuildingData.maxNumberOfWorkers;
        unassignBeeButton.interactable = _selectedBuilding.numAssignedBees > 0;

        icon.sprite = _selectedBuilding.BuildingData.UiImage;
        nameText.text = _selectedBuilding.BuildingData.name;
        assignedBeesText.text = "Assigned Bees: " + building.numAssignedBees + " / " + 
                                building.BuildingData.maxNumberOfWorkers + "\n" + "Unassigned Bees: " +
                                (int)(ResourceManagement.Instance.GetResource(ResourceType.Population).CurrentResourceAmount
                                - (int)ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).CurrentResourceAmount);

        resourcesText.text = "";
        var resources = building.gameObject.GetComponents<ResourceSupplier>();
        foreach (ResourceSupplier i in resources) {
            resourcesText.text += "Supplying " + i.Resource.name + ": " + i.actualProductionAmount + "\n";
        }

        var storage = building.gameObject.GetComponents<ResourceStorage>();
        foreach (ResourceStorage i in storage) {
            resourcesText.text += "Max " + i.Resource.name + " storage: " + i.GetStorage() + "\n";
        }
    }

    public void UpgradeBuilding() {
        if (_selectedBuilding.CanUpgrade()) {
            BuildingManager.Instance.UpgradeBuilding();
        }
    }

    public void AddBee() {
        BuildingManager.Instance.AddBeeToBuilding();
        UpdateDisplay(_selectedBuilding);
    }

    public void UnassignBee() {
        BuildingManager.Instance.RemoveBeeFromBuilding();
        UpdateDisplay(_selectedBuilding);
    }
}
