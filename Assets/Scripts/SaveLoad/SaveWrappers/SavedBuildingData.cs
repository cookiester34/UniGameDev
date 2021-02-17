using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper to save building data, does not include ui image as does not require saving
/// </summary>
[Serializable]
public class SavedBuildingData {
    /// <summary>
    /// The buildings type
    /// </summary>
    public BuildingType buildingType;

    /// <summary>
    /// Resources required to purchase the building
    /// </summary>
    public List<ResourcePurchase> resourcePurchase;

    public SavedBuildingData(BuildingData buildingData) {
        buildingType = buildingData.BuildingType;
        resourcePurchase = buildingData.ResourcePurchase;
    }
}
