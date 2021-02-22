using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableUIOnBuildingSelect : MonoBehaviour {

    public Text buildingsAssignedBees;

    private void Awake() {
        gameObject.SetActive(false);
        BuildingManager.Instance.OnBuildingSelected += DataChanged;
    }

    private void UpdateAssignedBeesUI(int numAssigned) {
        buildingsAssignedBees.text = "Assigned Bees: " + numAssigned;
    }

    private void DataChanged(Building building) {
        if (building != null) {
            gameObject.SetActive(true);
            UpdateAssignedBeesUI(building.numAssignedBees);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void UpgradeBuilding()
    {
        BuildingManager.Instance.UpgradeBuilding();
    }

    public void AddBee()
    {
        BuildingManager.Instance.AddBeeToBuilding();
    }

    public void RemoveBeeFromBuilding()
    {
        BuildingManager.Instance.RemoveBeeFromBuilding();
    }
}
