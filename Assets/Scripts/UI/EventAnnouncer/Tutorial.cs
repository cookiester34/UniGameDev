using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    private int currentProgress = -1;

    private void Awake() {
        Progress();
    }

    void Progress() {
        currentProgress++;

        switch (currentProgress) {
            case 0:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "Welcome to the hive, it looks like you have some work to do. For starters you'll" +
                    " need more housing for additional bees", AnnounceEventType.Tutorial);
                BuildingManager.Instance.OnBuildingPlaced += CheckForHousingPlaced;
                break;
            case 1:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "That's great, bees will now assign themselves to the housing, remember you wont be" +
                    " able to produce more bees unless there is free housing for them to go to", AnnounceEventType.Tutorial);
                StartCoroutine(ShowWaxMessage());
                break;
            case 2:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "Many buildings cost wax to build. Try making a wax converter to produce additional wax.", AnnounceEventType.Tutorial);
                BuildingManager.Instance.OnBuildingPlaced += CheckForWaxConverterPlaced;
                break;
            case 3:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "That converter won't work itself. Don't forget to assign some bees so they can start" +
                    " producing wax", AnnounceEventType.Tutorial);
                BeeManager.Instance.BeeAssigned += CheckBeeAssignedToWax; 
                break;
            case 4:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "Next up you may want some towers, these defend the hive from any unwanted pests",
                    AnnounceEventType.Tutorial);
                BuildingManager.Instance.OnBuildingPlaced += CheckForTowerPlaced;
                break;
            case 5:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "The tower cannot man itself, it will require bees to be assigned for it to function",
                    AnnounceEventType.Tutorial);
                BeeManager.Instance.BeeAssigned += CheckBeeAssignedToTower;
                break;
            case 6:
                UIEventAnnounceManager.Instance.AnnounceEvent(
                    "That covers the fundamentals. Prepare yourself for autumn",
                    AnnounceEventType.Tutorial);
                break;
        }
    }

    private void CheckForHousingPlaced(Building building) {
        if (building.BuildingType == BuildingType.Housing) {
            BuildingManager.Instance.OnBuildingPlaced -= CheckForHousingPlaced;
            Progress();
        }
    }

    private IEnumerator ShowWaxMessage() {
        yield return new WaitForSeconds(7.5f);
        Progress();
    }

    private void CheckForWaxConverterPlaced(Building building) {
        if (building.BuildingType == BuildingType.WaxConverter) {
            BuildingManager.Instance.OnBuildingPlaced -= CheckForWaxConverterPlaced;
            Progress();
        }
    }

    private void CheckBeeAssignedToWax(Building building) {
        if (building.BuildingType == BuildingType.WaxConverter) {
            BeeManager.Instance.BeeAssigned -= CheckBeeAssignedToWax;
            Progress();
        }
    }

    private void CheckForTowerPlaced(Building building) {
        if (building.BuildingType == BuildingType.Tower) {
            BuildingManager.Instance.OnBuildingPlaced -= CheckForTowerPlaced;
            Progress();
        }
    }

    private void CheckBeeAssignedToTower(Building building) {
        if (building.BuildingType == BuildingType.Tower) {
            BeeManager.Instance.BeeAssigned -= CheckBeeAssignedToTower;
            Progress();
        }
    }
}
