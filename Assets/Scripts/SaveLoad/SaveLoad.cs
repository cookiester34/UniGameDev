using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class that handles saving and loading functionality
/// </summary>
public static class SaveLoad {
    /// <summary>
    /// Extension type for saved files
    /// </summary>
    private const string saveExtension = ".json";

    /// <summary>
    /// Using the current scene generates a save and stores it
    /// </summary>
    /// <param name="savename">Name of the scene</param>
    public static void CreateSaveFromScene(string savename) {
        Save save = new Save();

        Building[] buildings = Object.FindObjectsOfType<Building>().ToArray();
        foreach (Building building in buildings) {
            save.buildingTransforms.Add(new SavedTransform(building.gameObject.transform));
            save.BuildingDatas.Add(building.BuildingData);
        }

        foreach (Resource resource in ResourceManagement.Instance.resourceList) {
            Resource resourceCopy = Object.Instantiate(resource);
            resourceCopy.Copy(resource);
            save.resources.Add(resourceCopy);
        }

        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.persistentDataPath + "/" + savename + saveExtension, json);
    }

    /// <summary>
    /// Loads the data from a save with a corresponding savename
    /// </summary>
    /// <param name="savename">Name of the save to load</param>
    public static void Load(string savename) {
        WipeScene();

        string json = File.ReadAllText(Application.persistentDataPath + "/" + savename + saveExtension);

        Save save = JsonUtility.FromJson<Save>(json);

        for (int i = 0; i < save.BuildingDatas.Count; i++) {
            SavedTransform transform = save.buildingTransforms[i];
            Object.Instantiate(save.BuildingDatas[i].BuildingPrefab, transform.Position, transform.Rotation);
        }

        for (int i = 0; i < save.resources.Count; i++) {
            ResourceManagement.Instance.resourceList[i].Copy(save.resources[i]);
        }
    }

    /// <summary>
    /// Wipes the current scene of data that gets saved, used when loading data to avoid duplicates
    /// </summary>
    private static void WipeScene() {
        Building[] buildings = Object.FindObjectsOfType<Building>().ToArray();
        foreach (Building building in buildings) {
            Object.Destroy(building.gameObject);
        }
    }
} 
