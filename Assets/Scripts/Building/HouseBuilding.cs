using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceStorage))]
public class HouseBuilding : Building {
    private ResourceStorage populationStorage;
    private void Awake() {
        populationStorage = GetComponent<ResourceStorage>();
    }

    public override bool CanAssignBee() {
        return numAssignedBees < populationStorage.CurrentStorage;
    }
    
    public override string GetAssignedBeesText() {
        return "Assigned Bees: " + numAssignedBees + " / " +
               populationStorage.CurrentStorage + "\n" + "Unassigned Bees: " +
               (int) (ResourceManagement.Instance.GetResource(ResourceType.Population).CurrentResourceAmount
                      - (int) ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).CurrentResourceAmount);
    }
}
