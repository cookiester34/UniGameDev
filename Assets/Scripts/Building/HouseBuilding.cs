using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilding : Building {
    [SerializeField] private ResourceStorage populationStorage;

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
