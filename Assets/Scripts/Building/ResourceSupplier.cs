using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building component that allows a building to supply, or use a resource
/// </summary>
[RequireComponent(typeof(Building))]
public class ResourceSupplier : MonoBehaviour, IBeforeDestroy {
    [Tooltip("Cannot be 0")]
    [Range(1, 100)]//just to stop it being set to 0;
    [SerializeField] private int productionTime = 1;
    [SerializeField] private int baseProductionAmount = 0;
    private float actualProductionAmount;

    [SerializeField] private Resource resource;

    Building building;
    int _lastAssignedBees = 0;

    void Awake() {
        if (resource == null) {
            Debug.LogError("A resource supplier has been created with no resource set");
            return;
        }

        building = GetComponent<Building>();
        if (building != null) {
            building.OnBuildingPlaced += () => resource.ModifyTickDrain(actualProductionAmount, productionTime);
        } else {
            resource.ModifyTickDrain(actualProductionAmount, productionTime);
        }
        CalculateProductionAmount();
    }

    void Update()
    {
        if(_lastAssignedBees != building.BuildingData.assignedBees)
        {
            _lastAssignedBees = building.BuildingData.assignedBees;
            resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
            CalculateProductionAmount();
            resource.ModifyTickDrain(actualProductionAmount, productionTime);
        }
    }

    void CalculateProductionAmount()
    {
        if (building.BuildingData.assignedBees == 0)
            actualProductionAmount = 0;
        else
            actualProductionAmount = (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.BuildingData.assignedBees;
    }

    public void BeforeDestroy() {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
    }
}
