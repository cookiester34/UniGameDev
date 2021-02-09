using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using CameraNameSpace;
using UnityEngine;

/// <summary>
/// Class that handles saving and loading functionality
/// </summary>
public static class SaveLoad {
    public delegate void SaveAdded(string savename);
    public static event SaveAdded OnSaveAdded;
    
    /// <summary>
    /// Extension type for saved files
    /// </summary>
    private const string saveExtension = ".json";
    private const string saveFolderName = "BeesSaves";
    private static string saveDirectoryPath;

    /// <summary>
    /// Using the current scene generates a save and stores it
    /// </summary>
    /// <param name="savename">Name of the scene</param>
    public static void CreateSaveFromScene(string savename) {
        SetupSaveDirectory();
        Save save = new Save();

        Building[] buildings = Object.FindObjectsOfType<Building>().ToArray();
        foreach (Building building in buildings) {
            save.buildingTransforms.Add(new SavedTransform(building.gameObject.transform));
            save.BuildingDatas.Add(new SavedBuildingData(building.BuildingData));
        }

        foreach (Resource resource in ResourceManagement.Instance.resourceList) {
            SavedResource savedResource = new SavedResource(resource);
            save.resources.Add(savedResource);
        }
		
		HexPanel[] panels = Object.FindObjectsOfType<HexPanel>().ToArray();
        foreach (HexPanel HP in panels) {
            save.hexesTransforms.Add(new SavedTransform(HP.gameObject.transform));
        }

        save.currentSeason = (int)SeasonManager.Instance.GetCurrentSeason();//save current season

        GameCamera gameCamera = Object.FindObjectOfType<GameCamera>();
        save.cameraTransform = new SavedTransform(gameCamera.transform);
        save.cameraTarget = gameCamera.TargetPositon;

        string json = JsonUtility.ToJson(save);
        string savePath = Path.Combine(saveDirectoryPath, savename);
        File.WriteAllText(savePath + saveExtension, json);
        OnSaveAdded?.Invoke(savename);
    }

    /// <summary>
    /// Loads the data from a save with a corresponding savename
    /// </summary>
    /// <param name="savename">Name of the save to load</param>
    public static void Load(string savename) {
        WipeScene();

        string savePath = Path.Combine(saveDirectoryPath, savename);
        string json = File.ReadAllText(savePath + saveExtension);
        Save save = JsonUtility.FromJson<Save>(json);

        for (int i = 0; i < save.BuildingDatas.Count; i++) {
            SavedTransform transform = save.buildingTransforms[i];
            Object.Instantiate(save.BuildingDatas[i].buildingType.GetPrefab(), transform.Position, transform.Rotation);
        }

        for (int i = 0; i < save.resources.Count; i++) {
            ResourceManagement.Instance.resourceList[i].CopySavedResource(save.resources[i]);
        }
		
		GenerateHexMap generatorInst = Object.FindObjectOfType<GenerateHexMap>(); //need this to get an easy reference to the hex prefab (and to set the colour)
        List<HexPanel> panels = new List<HexPanel>();
        for (int i = 0; i < save.hexesTransforms.Count; i++) {
			SavedTransform transform = save.hexesTransforms[i];
			GameObject go = Object.Instantiate(generatorInst.defaultHex, transform.Position, transform.Rotation);
            panels.Add(go.GetComponent<HexPanel>());
        }

        foreach (HexPanel panel in panels) {
            panel.CalculateNeighbours();
        }

        SeasonManager.Instance.SetCurrentSeason((Seasons)save.currentSeason);//set currrent season

        GameCamera gameCamera = Object.FindObjectOfType<GameCamera>();
        Transform cameraTransform = gameCamera.transform;
        cameraTransform.position = save.cameraTransform.Position;
        cameraTransform.rotation = save.cameraTransform.Rotation;
        gameCamera.TargetPositon = save.cameraTarget;
    }

    /// <summary>
    /// Wipes the current scene of data that gets saved, used when loading data to avoid duplicates
    /// </summary>
    private static void WipeScene() {
        Building[] buildings = Object.FindObjectsOfType<Building>().ToArray();
        foreach (Building building in buildings) {
            Object.Destroy(building.gameObject);
        }
		
		HexPanel[] panels = Object.FindObjectsOfType<HexPanel>().ToArray();
        foreach (HexPanel HP in panels) {
            Object.Destroy(HP.gameObject);
        }
    }

    /// <summary>
    /// Sets up the save directory
    /// </summary>
    private static void SetupSaveDirectory() {
        string path = Application.persistentDataPath;
        path = Path.Combine(path, saveFolderName);
        saveDirectoryPath = path;
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// Checks for existing saves, calling any save added listeners
    /// </summary>
    public static void CheckExistingSaves() {
        SetupSaveDirectory();
        string[] savedFiles = Directory.GetFiles(saveDirectoryPath);
        foreach (string savedFile in savedFiles) {
            if (savedFile.EndsWith(saveExtension)) {
                string strippedSavedFile = savedFile.Replace(saveExtension, "");
                string[] splitPath = strippedSavedFile.Split(Path.DirectorySeparatorChar);
                OnSaveAdded?.Invoke(splitPath[splitPath.Length - 1]);
            }
        }
    }

    public static void DeleteSave(string savename) {
        string filePath = Path.Combine(saveDirectoryPath, savename) + saveExtension;
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        } else {
            Debug.LogError("Attempting to delete file which does not exist, filepath: " + filePath);
        }
    }
} 
