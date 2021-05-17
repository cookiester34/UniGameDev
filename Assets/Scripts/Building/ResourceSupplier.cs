using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building component that allows a building to supply, or use a resource
/// </summary>
public class ResourceSupplier : MonoBehaviour {
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

    [SerializeField]
    private List<Research.ResearchObject> research = new List<Research.ResearchObject>();
    private float oldResearchAmount = 0;
    private float productionBuff = 1;
    private int numberOfResearched = 0;
    private int oldNumberOfResearched = 0;

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

        numberOfResearched = 0;
        foreach (Research.ResearchObject i in research)
        {
            if (i.Researched)
                numberOfResearched++;
        }
        if (oldNumberOfResearched != numberOfResearched)
        {
            oldNumberOfResearched = numberOfResearched;
            CalculateProductionAmount();
        }
    }

    public void CalculateProductionAmount()
    {
        CheckResearch();
        Debug.Log(productionBuff);
        if (building != null) {
            resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
            if (building.numAssignedBees == 0) {
                actualProductionAmount = 0;
            } else {
                actualProductionAmount = ((baseProductionAmount / building.BuildingData.maxNumberOfWorkers) *
                                         building.numAssignedBees * building.BuildingTier) * productionBuff;
            }

            resource.ModifyTickDrain(actualProductionAmount, productionTime);
        }

        ProductionChanged?.Invoke();
    }

    void CheckResearch()
    {
        float newResearchAmount = 0;
        foreach (Research.ResearchObject i in research)
        {
            if (i.Researched)
                newResearchAmount += 10;
        }
        if(oldResearchAmount != newResearchAmount)
        {
            oldResearchAmount = newResearchAmount;

            if (actualProductionAmount < 0)
                productionBuff = 1 + (newResearchAmount / 200);
            else
                productionBuff = 1 + (newResearchAmount / 100);

            

            if (productionBuff <= 0)
                productionBuff = 1;
        }
    }

    public string BeeBenefitText(bool assign) {
        CheckResearch();
        string text = "+ ";
        float increaseAmount = ((baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.BuildingTier) * productionBuff;
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
        CheckResearch();
        if (building.numAssignedBees == 0)
        {
            return 0;
        }
        else
        {
            return ((baseProductionAmount / building.BuildingData.maxNumberOfWorkers) * building.numAssignedBees * building.BuildingTier) * productionBuff;
        }
    } 

    public void OnDestroy() {
        resource.ModifyTickDrain(actualProductionAmount * -1, productionTime);
    }
}
