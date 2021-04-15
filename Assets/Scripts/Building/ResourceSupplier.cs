using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building component that allows a building to supply, or use a resource
/// </summary>
public class ResourceSupplier : MonoBehaviour, IBeforeDestroy {
    [Tooltip("Cannot be 0")]
    [Range(1, 100)]//just to stop it being set to 0;
    [SerializeField] private int productionTime = 1;
    [SerializeField] private float baseProductionAmount = 1;
    private float actualProductionAmount;

    [SerializeField] private Resource resource;
    public Resource Resource => resource;
    public int ProductionTime => productionTime;
    public float ProductionAmount => actualProductionAmount;

    [SerializeField] private Building building;
    int _lastAssignedBees = 0;

    bool buildingCatergoryManagerSet = false;

    public delegate void ProductionChangedHandler();
    public event ProductionChangedHandler ProductionChanged;

    

    void Awake() {
        if (resource == null) {
            Debug.LogError("A resource supplier has been created with no resource set");
            return;
        }

        if (building != null) {
            building.OnBuildingPlaced += () => resource.ModifyTickDrain(actualProductionAmount, productionTime);
            CalculateProductionAmount();
        } else {
            resource.ModifyTickDrain(baseProductionAmount, productionTime);
        }

        SetCatergory();
    }

    void SetCatergory()
    {
        if (transform.name == "Bee(Clone)")
            return;
        if (!buildingCatergoryManagerSet)
        {
            if (BuildingResourceCatergoriesManager.instance != null)
            {
                BuildingResourceCatergoriesManager.instance.catergories.Add(this);
                buildingCatergoryManagerSet = true;
            }
        }
    }

    void Update()
    {
        if(building != null &&_lastAssignedBees != building.numAssignedBees)
        {
            _lastAssignedBees = building.numAssignedBees;
            CalculateProductionAmount();
        }
        SetCatergory();
    }

    public void CalculateProductionAmount()
    {
        if (building != null) {
            resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
            if (building.numAssignedBees == 0) {
                actualProductionAmount = 0;
            } else {
                actualProductionAmount = (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) *
                                         building.numAssignedBees * building.BuildingTier;
            }

            resource.ModifyTickDrain(actualProductionAmount, productionTime);
        }

        ProductionChanged?.Invoke();
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

    public Transform GetBuilding()
    {
        return building.transform;
    }

    public float GetProductionAmount()
    {
        if (building.numAssignedBees == 0)
        {
            return 0;
        }
        else
        {
            return (baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.numAssignedBees * building.BuildingTier;
        }
    } 

    public void BeforeDestroy() {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
    }
}
