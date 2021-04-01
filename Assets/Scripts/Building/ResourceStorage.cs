using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Building component that allows the building to increase or decrease(on destroy) the cap on a resource
/// </summary>
public class ResourceStorage : MonoBehaviour {

    /// <summary>
    /// The resource that this storage will affect
    /// </summary>
    [SerializeField] private Resource resource;

    /// <summary>
    /// The amount to increase the cap by
    /// </summary>
    [FormerlySerializedAs("amount")]
    [Header("Tier 1")]
    [Tooltip("The amount to increase the cap by")]
    [SerializeField] private int tier1Amount;

    /// <summary>
    /// The amount to increase the cap by
    /// </summary>
    [Header("Tier 2")]
    [Tooltip("The amount to increase the cap by going from tier 1, to tier 2")]
    [SerializeField] private int tier2Increase;

    /// <summary>
    /// The amount to increase the cap by
    /// </summary>
    [Header("Tier 3")]
    [Tooltip("The amount to increase the cap by going from tier 2, to tier 3")]
    [SerializeField] private int tier3Increase;

    private int _currentStorage;

    /// <summary>
    /// Whether the storage should be filled on placement
    /// </summary>
    [Tooltip("Whether the storage should be filled on placement")]

    [SerializeField]private bool fillOnPlace = false;

    public Resource Resource => resource;

    public int CurrentStorage => _currentStorage;

    private void Awake() {
        if (resource == null) {
            Debug.LogError("A resource storage has been created with no resource set");
        }

        Building building = GetComponent<Building>();
        _currentStorage = tier1Amount;
        if (building != null) {
            building.OnBuildingPlaced += () => {
                ModifyCap(true);
                if (fillOnPlace) {
                    resource.ModifyAmount(tier1Amount);
                }
            };
        } else {
            ModifyCap(true);
        }
    }

    /// <summary>
    /// Recalculates the storage amount based on the building tier
    /// </summary>
    public void RecalculateStorage() {
        Building building = GetComponent<Building>();
        resource.ModifyCap(_currentStorage * -1, true);
        if (building != null) {
            switch (building.BuildingTier) {
                case 1:
                    _currentStorage = tier1Amount;
                    break;
                case 2:
                    _currentStorage = tier1Amount + tier2Increase;
                    break;
                case 3:
                    _currentStorage = tier1Amount + tier2Increase + tier3Increase;
                    break;
            }
        }
        ModifyCap(true);
    }

    private void ModifyCap(bool increase) {
        if (increase) {
            resource.ModifyCap(_currentStorage);
        } else {
            resource.ModifyCap(_currentStorage * -1);
        }
    }

    private void OnDestroy() {
        ModifyCap(false);
    }
}
