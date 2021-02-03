using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves the values of a resource, used for writing to json
/// </summary>
[Serializable]
public class SavedResource {
    public ResourceType Type;
    public int StartingAmount;
    public int TickDrainAmount;
    public int CurrentResourceAmount;
    public int Cap;

    /// <summary>
    /// Sets up values from a resource
    /// </summary>
    /// <param name="resource">Resource to save</param>
    public SavedResource(Resource resource) {
        Type = resource.resourceType;
        StartingAmount = resource.resourceStartingAmount;
        TickDrainAmount = resource.resourceTickDrainAmount;
        CurrentResourceAmount = resource.currentResourceAmount;
        Cap = resource.resourceCap;
    }
}
