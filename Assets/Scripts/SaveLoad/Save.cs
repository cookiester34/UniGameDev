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
/// </summary>
[Serializable]
public class Save {
    public List<BuildingData> BuildingDatas = new List<BuildingData>();
    public List<SavedTransform> buildingTransforms = new List<SavedTransform>();
    public List<Resource> resources = new List<Resource>();

    public void ClearSave() {
        BuildingDatas.Clear();
        buildingTransforms.Clear();
        resources.Clear();
    }
}

[Serializable]
public class SavedTransform {
    public Vector3 Position;
    public Quaternion Rotation;

    public SavedTransform(Vector3 position, Quaternion rotation) {
        Position = position;
        Rotation = rotation;
    }
    
    public SavedTransform(Transform transform) {
        Position = transform.position;
        Rotation = transform.rotation;
    }
}
