﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building component that allows a building to supply, or use a resource
/// </summary>
public class ResourceSupplier : MonoBehaviour {
    [Tooltip("Cannot be 0")]
    [Range(1, 100)]//just to stop it being set to 0;
    [SerializeField] private int productionTime = 1;
    [SerializeField] private int productionAmount = 0;

    [SerializeField] private Resource resource;

    void Awake() {
        if (resource == null) {
            Debug.LogError("A resource supplier has been created with no resource set");
        }
        InvokeRepeating(nameof(Production), productionTime, productionTime);
    }

    private void Production() {
        resource.ModifyAmount(productionAmount);
    }
}
