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
    private TooltipEnabler _upgradeBuildingTooltip;
    private TooltipEnabler _assignBeeTooltip;
    private TooltipEnabler _unassignBeeTooltip;
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

        _assignBeeTooltip = assignBeeButton.GetComponent<TooltipEnabler>();
        _unassignBeeTooltip = unassignBeeButton.GetComponent<TooltipEnabler>();
        _upgradeBuildingTooltip = upgradeBuildingButton.GetComponent<TooltipEnabler>();
        BuildingManager.Instance.OnBuildingSelected += UpdateDisplay;
    }

    private void UpdateDisplay(Building building) {
        _selectedBuilding = building;
        thisTabbableButton.gameObject.SetActive(building != null);
        if (building == null) {
            thisTabbableButton.ActivateContents(false);
            return;
        }

        bar.MakeContentsActive(thisTabbableButton, true);
        
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
        _upgradeBuildingTooltip.TooltipText = building.CanUpgrade()
            ? building.BuildingData.UpgradeCost(building.BuildingTier + 1) : "Missing research to upgrade";
        
        if (building is TowerBuilding tower) {
            SetupFromTower(tower);
        } else {
            string assignText = "";
            string unassignText = "";
            var resources = building.gameObject.GetComponents<ResourceSupplier>();
            foreach (ResourceSupplier i in resources) {
                assignText += i.BeeBenefitText(true);
                unassignText += i.BeeBenefitText(false);
                resourcesText.text += "Supplying " + i.Resource.name + ": " + i.actualProductionAmount + "\n";
            }

            var storage = building.gameObject.GetComponents<ResourceStorage>();
            foreach (ResourceStorage i in storage) {
                resourcesText.text += "Max " + i.Resource.name + " storage: " + i.CurrentStorage + "\n";
            }

            if (building.BuildingType == BuildingType.Housing) {
                assignText = unassignText = "Bees autoassign selves to housing as needed";
                unassignBeeButton.interactable = false;
                assignBeeButton.interactable = false;
            }
            _assignBeeTooltip.TooltipText = assignText;
            _unassignBeeTooltip.TooltipText = unassignText;

        }

    }

    public void UpgradeBuilding() {
        if (_selectedBuilding.CanUpgrade()) {
            BuildingManager.Instance.UpgradeBuilding();
            UpdateDisplay(_selectedBuilding);
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

    private void SetupFromTower(TowerBuilding tower) {
        if (tower.numAssignedBees == 0) {
            resourcesText.text = "Time between shots: infinite";
        } else {
            resourcesText.text = "Time between shots: " + tower.firingSpeed;
        }

        _assignBeeTooltip.TooltipText = "Decreases the time between shots of the tower";
        _unassignBeeTooltip.TooltipText = "Decreases the time between shots of the tower";
    }
}
