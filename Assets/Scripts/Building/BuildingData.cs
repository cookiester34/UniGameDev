using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

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
    /// Resources required to purchase the building
    /// </summary>
    [SerializeField] private List<ResourcePurchase> resourcePurchase;

    public Sprite UiImage => uiImage;

    public BuildingType BuildingType => buildingType;

    public List<ResourcePurchase> ResourcePurchase => resourcePurchase;

    /// <summary>
    /// Allows copyng in of saved data
    /// </summary>
    /// <param name="savedData">Data to copy in</param>
    public void CopySavedData(SavedBuildingData savedData) {
        buildingType = savedData.buildingType;
        resourcePurchase = savedData.resourcePurchase;
    }

    public Sprite GetSprite() {
        return uiImage;
    }

    public string GetHoverText() {
        return name;
    }

    public void OnClick() {
        if (buildingType != BuildingType.Destroy) {
            BuildingManager.Instance.PlaceBuilding(this);
        } else {
            BuildingManager.Instance.DestroyBuilding();
        }
    }
}
