using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Research;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Building : MonoBehaviour {
    public delegate void EmptyEvent();
    public event EmptyEvent OnBuildingPlaced;

    [SerializeField] private BuildingType buildingType;

    [SerializeField] private BuildingData buildingData;
    protected int buildingTier = 1;
    [FormerlySerializedAs("buildingTeir1")] public GameObject buildingTier1;
    [FormerlySerializedAs("buildingTeir2")] public GameObject buildingTier2;
    [FormerlySerializedAs("buildingTeir3")] public GameObject buildingTier3;

    public BuildingData BuildingData => buildingData;

    public BuildingType BuildingType => buildingType;
    public int BuildingTier => buildingTier;

    /// <summary>
    /// number of bees assigned to this building
    /// </summary>
    [HideInInspector]


    public int numAssignedBees;
    private List<Bee> _assignedBees;
    private List<BuildingFoundation> usedFoundations = new List<BuildingFoundation>();

    public List<BuildingFoundation> UsedFoundations {
        get => usedFoundations;
        set => usedFoundations = value;
    }

    public List<Bee> AssignedBees => _assignedBees;

    protected virtual void Start()
    {
        if (buildingTier1 != null) {
            buildingTier1.SetActive(buildingTier == 1);
        }

        if (buildingTier2 != null) {
            buildingTier2.SetActive(buildingTier == 2);
        }

        if (buildingTier3 != null) {
            buildingTier3.SetActive(buildingTier == 3);
        }
        BuildingManager.Instance.AddBuildingToBuildings(this);
    }

    public GameObject GetActiveBuilding()
    {
        switch (buildingTier)
        {
            case 0:
                return buildingTier1;
            case 1:
                return buildingTier2;
            case 2:
                return buildingTier3;
        }
        return null;
    }

    public void PlaceBuilding() {
        OnBuildingPlaced?.Invoke();
    }

    public virtual string GetAssignedBeesText() {
        return "Assigned Bees: " + numAssignedBees + " / " +
               BuildingData.maxNumberOfWorkers + "\n" + "Unassigned Bees: " +
               (int) (ResourceManagement.Instance.GetResource(ResourceType.Population).CurrentResourceAmount
                      - (int) ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).CurrentResourceAmount);
    }

    public virtual bool CanAssignBee() {
        return numAssignedBees < buildingData.maxNumberOfWorkers;
    }

    public virtual void AssignBee(Bee bee) {
        if (_assignedBees == null) {
            _assignedBees = new List<Bee>();
        }

        if (_assignedBees.Contains(bee)) {
            Debug.LogWarning("Attempting to assign a bee that is already assigned to this building");
        }

        _assignedBees.Add(bee);
        numAssignedBees = _assignedBees.Count;
        
        var suppliers = GetComponentsInChildren<ResourceSupplier>();
        if (suppliers != null && suppliers.Length > 1) {
            foreach (var supplier in suppliers) {
                supplier.CalculateProductionAmount();
            }
        }
    }

    /// <summary>
    /// Attempts to unassign a bee from the building
    /// </summary>
    /// <param name="bee">A specific bee to remove, or null for a random one</param>
    /// <returns>The bee removed</returns>
    public virtual Bee UnassignBee(Bee bee = null) {
        if (_assignedBees == null) {
            _assignedBees = new List<Bee>();
        }

        Bee beeUnassigned = bee;
        if (_assignedBees.Count < 1) {
            Debug.LogWarning("Attempting to unassign a bee when no bees are  assigned to this building");
            beeUnassigned = null;
        } else {
            if (beeUnassigned == null) {
                beeUnassigned = _assignedBees[0];
            }

            _assignedBees.Remove(beeUnassigned);
        }
        numAssignedBees = _assignedBees.Count;
        
        var suppliers = GetComponentsInChildren<ResourceSupplier>();
        if (suppliers != null && suppliers.Length > 1) {
            foreach (var supplier in suppliers) {
                supplier.CalculateProductionAmount();
            }
        }

        return beeUnassigned;
    }

    private void OnDestroy() {
        foreach (BuildingFoundation foundation in usedFoundations) {
            foundation.CanBuild = true;
        }

        if (!ApplicationUtil.IsQuitting) {
            BuildingManager.Instance.RemoveFromBuildings(this);
        }
    }

    public bool CanUpgrade() {
        bool canUpgrade = true;
        int newTier = buildingTier + 1;
        switch (newTier) {
            case 2:
                canUpgrade = buildingTier2 != null;
                break;
            case 3:
                canUpgrade = buildingTier3 != null;
                break;
        }
        return canUpgrade && buildingData.CanUpgrade(buildingTier + 1);
    }

    public void Upgrade() {
        buildingTier++;
        buildingTier1.SetActive(buildingTier == 1);
        buildingTier2.SetActive(buildingTier == 2);
        buildingTier3.SetActive(buildingTier == 3);

        var suppliers = GetComponentsInChildren<ResourceSupplier>();
        if (suppliers != null && suppliers.Length > 0) {
            foreach (var supplier in suppliers) {
                supplier.CalculateProductionAmount();
            }
        }

        var storages = GetComponentsInChildren<ResourceStorage>();
        if (storages != null && storages.Length > 0) {
            foreach (ResourceStorage storage in storages) {
                storage.RecalculateStorage();
            }
        }
    }

    /// <summary>
    /// Gets the amount that should be returned for a refund, uses a multiplier to get less than the purchase cost
    /// </summary>
    /// <returns>An amount to refund based on the tier of the building, uses an internal multiplier</returns>
    public List<ResourcePurchase> GetRefundAmount() {
        List<ResourcePurchase> resourcesUsed = new List<ResourcePurchase>();
        switch (buildingTier) {
            case 1:
                 resourcesUsed.AddRange(buildingData.Tier1Cost);
                break;
            case 2:
                 resourcesUsed.AddRange(buildingData.Tier1Cost);
                 resourcesUsed.AddRange(buildingData.Tier2Cost);
                break;
            case 3:
                resourcesUsed.AddRange(buildingData.Tier1Cost);
                resourcesUsed.AddRange(buildingData.Tier2Cost);
                resourcesUsed.AddRange(buildingData.Tier3Cost);
                break;
        }
        
        // Little copy dance to avoid modifying the values of the actual purchases
        List<ResourcePurchase> copy = new List<ResourcePurchase>();
        foreach (ResourcePurchase purchase in resourcesUsed) {
            copy.Add(new ResourcePurchase(purchase.resourceType, purchase.cost));
        }

        float refundPercent = 0.75f;
        foreach (ResourcePurchase purchase in copy) {
            purchase.cost = Mathf.FloorToInt(purchase.cost * refundPercent);
        }

        return copy;
    }
}
