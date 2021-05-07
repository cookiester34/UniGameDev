using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

/// <summary>
/// Class to store building data, should be one for each building type
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Building Data")]
public class BuildingData : ScriptableObject, IUiClickableHover {
    /// <summary>
    /// The buildings type
    /// </summary>
    [SerializeField] private BuildingType buildingType;
    
    /// <summary>
    /// An image to display in the UI for the building
    /// </summary>
    [SerializeField] private Sprite uiImage;

    /// <summary>
    /// The building shape
    /// sizes yet
    /// </summary>
    [SerializeField] private BuildingShape buildingShape;
	
	/// <summary>
    /// How many instances of this type of building can exist at once
    /// </summary>
    [SerializeField] private int maxInstances;
	
	/// <summary>
    /// Description of the building
    /// </summary>
    [SerializeField] private string description = "Building description here!";

    /// <summary>
    /// max number of bees allowed to work at this building
    /// </summary>
    [Range(0,10)]
    public int maxNumberOfWorkers;
    
    [SerializeField] private List<ResearchObject> tier1RequiredResearch;
    [SerializeField] private List<ResearchObject> tier2RequiredResearch;
    [SerializeField] private List<ResearchObject> tier3RequiredResearch;
    [FormerlySerializedAs("resourcePurchase")] [SerializeField] private List<ResourcePurchase> tier1Cost;
    [SerializeField] private List<ResourcePurchase> tier2Cost;
    [SerializeField] private List<ResourcePurchase> tier3Cost;

    public Sprite UiImage => uiImage;

    public BuildingType BuildingType => buildingType;

    public List<ResourcePurchase> Tier1Cost => tier1Cost;
    public List<ResourcePurchase> Tier2Cost => tier2Cost;
    public List<ResourcePurchase> Tier3Cost => tier3Cost;

    public BuildingShape BuildingShape => buildingShape;
	
	public int MaxInstances => maxInstances;
	
	public string Description => description;

    /// <summary>
    /// Allows copyng in of saved data
    /// </summary>
    /// <param name="savedData">Data to copy in</param>
    public void CopySavedData(SavedBuildingData savedData) {
        buildingType = savedData.buildingType;
        tier1Cost = savedData.resourcePurchase;
    }

    public bool CanUpgrade(int newTier) {
        bool canUpgrade = true;
        List<ResearchObject> researchList = null;
        switch (newTier) {
            case 1:
                researchList = tier1RequiredResearch;
                break;
            
            case 2:
                researchList = tier2RequiredResearch;
                break;
            
            case 3:
                researchList = tier3RequiredResearch;
                break;
        }
        
        if (researchList != null) {
            foreach (ResearchObject o in researchList) {
                if (!o.Researched) {
                    canUpgrade = false;
                    break;
                }
            }
        }

        return canUpgrade;
    }

    public string UpgradeCost(int buildingTier) {
        string costString = "";
        List<ResourcePurchase> purchase = null;

        switch (buildingTier) {
            case 1:
                purchase = tier1Cost;
                break;
            case 2:
                purchase = tier2Cost;
                break;
            case 3:
                purchase = tier3Cost;
                break;
            default:
                costString = "Building at max tier";
                break;
        }

        if (purchase != null) {
            if (purchase.Count < 0) {
                costString = "No cost";
            } else {
                foreach (ResourcePurchase resourcePurchase in purchase) {
                    costString += resourcePurchase.resourceType + ": " + resourcePurchase.cost;
                }
            }
        }

        return costString;
    }

    public Sprite GetSprite() {
        return uiImage;
    }

    public string GetHoverText() {
        return name;
    }

    public void OnClick() {
        BuildingMode mode;
        if(BuildingType == BuildingType.Destroy)
        {
            mode = BuildingMode.Destroy;
        }
        else if (BuildingType == BuildingType.Repair)
        {
            mode = BuildingMode.Repair;
        }
        else
        {
            mode = BuildingMode.Build;
        }
        BuildingManager.Instance.SetBuildMode(mode, this);
    }
}
