using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedBuildingFoundation {
    public int id;
    public bool canBuild;

    public SavedBuildingFoundation(BuildingFoundation buildingFoundation) {
        id = buildingFoundation.Id;
        canBuild = buildingFoundation.CanBuild;
    }
}
