using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building component that allows the building to increase or decrease(on destroy) the cap on a resource
/// </summary>
public class ResourceStorage : MonoBehaviour, IBeforeDestroy {
    /// <summary>
    /// The amount to increase the cap by
    /// </summary>
    [Tooltip("The amount to increase the cap by")]
    [SerializeField] private int amount;

    /// <summary>
    /// The resource that this storage will affect
    /// </summary>
    [SerializeField] private Resource resource;

    private void Awake() {
        if (resource == null) {
            Debug.LogError("A resource storage has been created with no resource set");
        }

        Building building = GetComponent<Building>();
        if (building != null) {
            building.OnBuildingPlaced += () => ModifyCap(true);
        } else {
            ModifyCap(true);
        }
    }

    private void ModifyCap(bool increase) {
        if (increase) {
            resource.ModifyCap(amount);
        } else {
            resource.ModifyCap(amount * -1);
        }
    }


    public void BeforeDestroy() {
        ModifyCap(false);
    }
}
