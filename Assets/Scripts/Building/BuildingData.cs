using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
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
    /// Resources required to purchase the building
    /// </summary>
    [SerializeField] private List<ResourcePurchase> resourcePurchase;

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

    public Sprite UiImage => uiImage;

    public BuildingType BuildingType => buildingType;

    public List<ResourcePurchase> ResourcePurchase => resourcePurchase;

    public BuildingShape BuildingShape => buildingShape;
	
	public int MaxInstances => maxInstances;
	
	public string Description => description;

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
        BuildingMode mode = buildingType == BuildingType.Destroy ? BuildingMode.Destroy : BuildingMode.Build;
        BuildingManager.Instance.SetBuildMode(mode, this);
    }
}
