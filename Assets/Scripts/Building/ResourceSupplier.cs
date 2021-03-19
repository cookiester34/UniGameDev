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
        resource.OnCapReached += SupplyCapped; // Event from resource in supplier when cap is reached.
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
        if (building.numAssignedBees == 0 || resource.ResourceCapReached())
        {
            actualProductionAmount = 0;
            StopProductionBar();
        }
        else
        {
            actualProductionAmount = (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.numAssignedBees * building.BuildingTier;
            StartProductionBar();
        }
        resource.ModifyTickDrain(actualProductionAmount, productionTime);
    }

    public string BeeBenefitText(bool assign) {
        string text = "+ ";
        float increaseAmount = (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.BuildingTier;
        if (!assign) {
            increaseAmount *= -1;
            text = "- ";
        }

        text += resource + ": " + increaseAmount + " per bee\n";
        return text;
    }


    // Halts the Progress bar an sets it to 0
    public void StopProductionBar()
    {
        if (progressBar != null)
        {
            progressBar.Deactivate();
        }
    }

    // Starts the bar filling at the same rate as production
    public void StartProductionBar()
    {
        if (progressBar != null)
        {
            progressBar.Activate(actualProductionAmount / productionTime / ResourceManagement.Instance.resourceTickTime);
        }
    }

    // Called when the resource from the supplier hits cap.
    public void SupplyCapped()
    {
        StopProductionBar();
    /// Here is where we can enable some way to show the cap is reached ///
    }

    public void BeforeDestroy() {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
    }
}
