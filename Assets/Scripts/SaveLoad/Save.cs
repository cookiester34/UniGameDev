using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// A save holds a bunch of data, this can then be turned into json to save and read back in.
/// </summary>s
[Serializable]
public class Save {
    public string terrainSceneName;
    
    public List<SavedBuildingData> BuildingDatas = new List<SavedBuildingData>();
    public List<int> AssignedBees = new List<int>();

    public List<SavedBuilding> buildings = new List<SavedBuilding>();
    public List<SavedBee> bees = new List<SavedBee>();

    public List<SavedTransform> buildingTransforms = new List<SavedTransform>();
    public List<SavedHealth> buildingHealth = new List<SavedHealth>();

    public List<SavedResource> resources = new List<SavedResource>();
    
    public List<SavedResearch> researches = new List<SavedResearch>();

	public List<SavedTransform> hexesTransforms = new List<SavedTransform>();

    public SavedTransform cameraTransform;
    public Vector3 cameraTarget;

    public int currentSeason;
    public int waveNumber;
}
