using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum BuildingType
{
    Storage,
    Housing,
    EggFactory,
    HoneyFactory,
    Destroy,
    Tower
}

/// <summary>
/// Extension methods for building type enum
/// </summary>
public static class BuildingTypeExtension {
    
    /// <summary>
    /// Relies on list in building manager to get the prefab from, the buildings must be in the correct order
    /// </summary>
    /// <param name="type">Building type</param>
    /// <returns>The buildings prefab, may be null</returns>
    public static GameObject GetPrefab(this BuildingType type) {
        GameObject obj = null;
        switch (type) {
            case BuildingType.Storage:
                obj = Resources.Load<GameObject>("Building");
                break;

            case BuildingType.Housing:
                obj = Resources.Load<GameObject>("Building 1");
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
        }

        return obj;
    }
}
