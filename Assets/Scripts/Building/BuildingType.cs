using System.Collections;
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
    WaxConverter,
    HoneyConverter,
    JellyConverter,
    Destroy,
    Tower,
    QueenBee,
    Research,
    EnemyBuilding,
    Healing
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
                obj = Resources.Load<GameObject>("Buildings/Storage");
                break;

            case BuildingType.Housing:
                obj = Resources.Load<GameObject>("Buildings/Housing");
                break;

            case BuildingType.Tower:
                obj = Resources.Load<GameObject>("Buildings/Tower");
                break;

            case BuildingType.HoneyConverter:
                obj = Resources.Load<GameObject>("Buildings/HoneyConverter");
                break;

            case BuildingType.WaxConverter:
                obj = Resources.Load<GameObject>("Buildings/WaxConverter");
                break;

            case BuildingType.JellyConverter:
                obj = Resources.Load<GameObject>("Buildings/JellyConverter");
                break;

            case BuildingType.QueenBee:
                obj = Resources.Load<GameObject>("Buildings/QueenBeeBuilding");
                break;

            case BuildingType.Research:
                obj = Resources.Load<GameObject>("Buildings/Research");
                break;

            case BuildingType.EnemyBuilding:
                obj = Resources.Load<GameObject>("Buildings/EnemyBuilding");
                break;
        }

        if (obj == null) {
            Debug.LogError(type + " has no matching prefab in building type enum");
        }

        return obj;
    }
    
    public static GameObject GetModel(this BuildingType type) {
        GameObject obj = null;
        switch (type) {
            case BuildingType.Storage:
                obj = Resources.Load<GameObject>("BuildingModels/Storage");
                break;

            case BuildingType.Housing:
                obj = Resources.Load<GameObject>("BuildingModels/Housing");
                break;

            case BuildingType.Tower:
                obj = Resources.Load<GameObject>("BuildingModels/Tower");
                break;

            case BuildingType.HoneyConverter:
                obj = Resources.Load<GameObject>("BuildingModels/HoneyConverter");
                break;

            case BuildingType.WaxConverter:
                obj = Resources.Load<GameObject>("BuildingModels/WaxConverter");
                break;

            case BuildingType.JellyConverter:
                obj = Resources.Load<GameObject>("BuildingModels/JellyConverter");
                break;

            case BuildingType.QueenBee:
                obj = Resources.Load<GameObject>("BuildingModels/QueenBeeBuilding");
                break;
            
            case BuildingType.Research:
                obj = Resources.Load<GameObject>("BuildingModels/Research");
                break;

            case BuildingType.EnemyBuilding:
                obj = Resources.Load<GameObject>("BuildingModels/EnemyBuilding");
                break;
        }

        if (obj == null) {
            Debug.LogError(type + " has no matching prefab in building type enum");
        }

        return obj;
    }
}
