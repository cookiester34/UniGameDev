using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableUIOnBuildingSelect : MonoBehaviour {

    public Text buildingsAssignedBees;

    private void Awake() {
        gameObject.SetActive(false);
        BuildingManager.Instance.OnBuildingSelected += gameObject.SetActive;
    }

    private void OnEnable()
    {
        UpdateAssignedBeesUI();
    }

    private void UpdateAssignedBeesUI()
    {
        buildingsAssignedBees.text = "Assigned Bees: " + BuildingManager.Instance.selectedBuildingData.BuildingData.assignedBees;
    }

    public void UpgradeBuilding()
    {
        BuildingManager.Instance.UpgradeBuilding();
    }

    public void AddBee()
    {
        BuildingManager.Instance.AddBeeToBuilding();
        UpdateAssignedBeesUI();
    }

    public void RemoveBeeFromBuilding()
    {
        BuildingManager.Instance.RemoveBeeFromBuilding();
        UpdateAssignedBeesUI();
    }
}
