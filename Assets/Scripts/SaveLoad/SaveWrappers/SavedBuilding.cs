using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SavedBuilding {
    public SavedTransform transform;
    public SavedHealth health;
    public SavedBuildingData buildingData;
    public List<SavedBee> assignedBees;

    public SavedBuilding(Building building) {
        transform = new SavedTransform(building.transform);
        health = new SavedHealth(building.GetComponent<Health>());
        buildingData = new SavedBuildingData(building.BuildingData);
        assignedBees = new List<SavedBee>();
        foreach (Bee bee in building.AssignedBees) {
            assignedBees.Add(new SavedBee(bee));
        }
    }

    public void Instantiate() {
        GameObject go = Object.Instantiate(
            buildingData.buildingType.GetPrefab(), transform.Position, transform.Rotation);
        go.GetComponent<Health>().LoadSavedHealth(health);
    }
}
