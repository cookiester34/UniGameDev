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
    public int buildingTier = 1;

    public SavedBuilding(Building building) {
        transform = new SavedTransform(building.transform);
        health = new SavedHealth(building.GetComponent<Health>());
        buildingData = new SavedBuildingData(building.BuildingData);
        assignedBees = new List<SavedBee>();
        buildingTier = building.BuildingTier;
        if (building.AssignedBees != null) {
            foreach (Bee bee in building.AssignedBees) {
                assignedBees.Add(new SavedBee(bee));
            }
            building.AssignAnimation();
        }
    }

    public Building Instantiate(List<Bee> loadedBees) {
        GameObject go = Object.Instantiate(
            buildingData.buildingType.GetPrefab(), transform.Position, transform.Rotation);
        go.GetComponent<Health>().LoadSavedHealth(health);
        Building building = go.GetComponent<Building>();
        building.UpdateBuildingVisual(buildingTier);
        foreach (SavedBee bee in assignedBees) {
            Bee matchingBee = loadedBees.Find(bee1 => bee1.Id == bee.id);
            if (matchingBee != null) {
                BeeAssign.AssignBeeToBuilding(building, matchingBee);
            }
        }

        return building;
    }
}
