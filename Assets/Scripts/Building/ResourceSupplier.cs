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
    [SerializeField] private float baseProductionAmount = 1;
    [SerializeField] private RadialProgress progressBar;
    [HideInInspector]
    public float actualProductionAmount;

    [SerializeField] private Resource resource;
    public Resource Resource => resource;

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
            CalculateProductionAmount();
        } else {
            resource.ModifyTickDrain(actualProductionAmount, productionTime);
        }
    }

    void Update()
    {
        if(building != null &&_lastAssignedBees != building.numAssignedBees)
        {
            _lastAssignedBees = building.numAssignedBees;
            CalculateProductionAmount();
        }
    }

    public void CalculateProductionAmount()
    {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
        if (building.numAssignedBees == 0)
        {
            actualProductionAmount = 0;
            if (progressBar != null)
            {
                progressBar.active = false;
                progressBar.progressBar.fillAmount = 0;
            }
        }
        else
        {
            actualProductionAmount = (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.numAssignedBees * building.BuildingTier;
            if (progressBar != null)
            {
                progressBar.active = true;
                progressBar.fillSpeed = actualProductionAmount / productionTime / ResourceManagement.Instance.resourceTickTime;
            }
        }
        resource.ModifyTickDrain(actualProductionAmount, productionTime);
    }

    public void BeforeDestroy() {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
    }
}
