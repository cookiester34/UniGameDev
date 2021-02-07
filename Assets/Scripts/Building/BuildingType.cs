﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[Serializable]
public enum BuildingType
{
    Storage,
    Housing,
    EggFactory,
    HoneyFactory,
    Destroy,
    Tower,
    QueenBee
}

/// <summary>
/// Extension methods for building type enum
/// </summary>
public static class BuildingTypeExtension {
    
    /// <summary>
    /// Relies on list in building manager to get the prefab from
    /// </summary>
    /// <param name="type">Building type</param>
    /// <returns>The buildings prefab, may be null</returns>
    public static GameObject GetPrefab(this BuildingType type) {
        GameObject obj = null;
        switch (type) {
            case BuildingType.Storage:
                obj = Resources.Load<GameObject>("Storage");
                break;

            case BuildingType.Housing:
                obj = Resources.Load<GameObject>("Housing");
                break;

            case BuildingType.Tower:
                obj = Resources.Load<GameObject>("Tower");
                break;

            case BuildingType.HoneyFactory:
                obj = Resources.Load<GameObject>("Tower");
                break;

            case BuildingType.EggFactory:
                obj = Resources.Load<GameObject>("Tower");
                break;

            case BuildingType.QueenBee:
                obj = Resources.Load<GameObject>("QueenBeeBuilding");
                break;
        }

        if (obj == null) {
            Debug.LogError(type + " has no matching prefab in building type enum");
        }

        return obj;
    }
}
