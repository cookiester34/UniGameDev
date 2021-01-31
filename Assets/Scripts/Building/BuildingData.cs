using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to store building data, should be one for each building type
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Building Data")]
public class BuildingData : ScriptableObject {
    /// <summary>
    /// The building prefab to instantiate
    /// </summary>
    [SerializeField] private GameObject buildingPrefab;
    
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

    public GameObject BuildingPrefab => buildingPrefab;

    public Sprite UiImage => uiImage;

    public BuildingType BuildingType => buildingType;

    public List<ResourcePurchase> ResourcePurchase => resourcePurchase;
}
