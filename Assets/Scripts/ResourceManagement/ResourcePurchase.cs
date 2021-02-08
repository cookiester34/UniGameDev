using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcePurchase {
    /// <summary>
    /// Cost of the purchase
    /// </summary>
    public int cost;
    
    /// <summary>
    /// The resource type to purchase from
    /// </summary>
    public ResourceType resourceType;

    public ResourcePurchase(ResourceType type, int cost) {
        this.cost = cost;
        resourceType = type;
    }
}
