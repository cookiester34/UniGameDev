using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using CameraNameSpace;
using Research;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

/// <summary>
/// Class that handles saving and loading functionality
/// </summary>
public static class SaveLoad {
    public delegate void SaveAdded(Save save);
    public static event SaveAdded OnSaveAdded;
    
    /// <summary>
    /// Extension type for saved files
    /// </summary>
    private const string saveExtension = ".json";
    private const string saveFolderName = "BeesSaves";
    private static string saveDirectoryPath;

    /// <summary>
    /// Save file that is currently being dealt with
    /// </summary>
    private static Save _currentSave;

    /// <summary>
    /// Using the current scene generates a save and stores it
    /// </summary>
    /// <param name="savename">Name of the scene</param>
    public static void CreateSaveFromScene(string savename) {
        SetupSaveDirectory();
        Save save = new Save(savename, SceneManager.GetActiveScene().name, true);

        Building[] buildings = Object.FindObjectsOfType<Building>().ToArray();
        foreach (Building building in buildings) {
            save.buildings.Add(new SavedBuilding(building));
        }

        List<SavedBee> savedBees = new List<SavedBee>();
        foreach (Bee bee in Object.FindObjectsOfType<Bee>().ToArray()) {
            savedBees.Add(new SavedBee(bee));
        }
        save.bees = savedBees;

        foreach (EnemyBuilding building in Object.FindObjectsOfType<EnemyBuilding>()) {
            save.enemyBuildings.Add(new SavedTransform(building.transform));
        }

        foreach (Resource resource in ResourceManagement.Instance.resourceList) {
            SavedResource savedResource = new SavedResource(resource);
            save.resources.Add(savedResource);
        }

        foreach (ResearchObject researchObject in ResearchManager.Instance.AllResearch) {
            SavedResearch savedResearch = new SavedResearch(researchObject);
            save.researches.Add(savedResearch);
        }

        save.currentSeason = (int)SeasonManager.Instance.GetCurrentSeason();//save current season
        save.waveNumber = EnemySpawnManager.Instance.waveNumber;

        GameCamera gameCamera = Object.FindObjectOfType<GameCamera>();
        save.cameraTransform = new SavedTransform(gameCamera.transform);
        save.cameraTarget = gameCamera.TargetPositon;

        string json = JsonUtility.ToJson(save, true);
        string savePath = Path.Combine(saveDirectoryPath, savename);
        File.WriteAllText(savePath + saveExtension, json);
        OnSaveAdded?.Invoke(save);
    }

    /// <summary>
    /// Loads the data from a save with a corresponding savename
    /// </summary>
    /// <param name="savename">Name of the save to load</param>
    public static void Load(string savename) {
        CurrentSceneType.SceneType = SceneType.GameLevel;
        string savePath = Path.Combine(saveDirectoryPath, savename + saveExtension);
        string json;
        if (File.Exists(savePath)) {
            json = File.ReadAllText(savePath);
        } else {
            // missing, most likely a save provided with the game
            json = Resources.Load<TextAsset>("Saves/" + savename).text;
        }
        Save save = JsonUtility.FromJson<Save>(json);
        _currentSave = save;

        // Have to re-register on every load for reasons unbeknown to me, thought registering once in constructor would
        // work but it is somehow cleared
        SceneManagement.Instance.SceneLoaded += SceneLoaded;
        SceneManagement.Instance.LoadScene(save.terrainSceneName);
    }

    /// <summary>
    /// All functionality to be done after the scene is loaded with the terrain 
    /// </summary>
    private static void SceneLoaded() {
        List<Bee> loadedBees = new List<Bee>();
        foreach (SavedBee savedBee in _currentSave.bees) {
            loadedBees.Add(savedBee.Instantiate());
        }

        foreach (SavedBuilding savedBuilding in _currentSave.buildings) {
            var building = savedBuilding.Instantiate(loadedBees);
            var position = building.transform.position;
            position.y += 10f;
            position.x += 0.1f;
            position.z += 0.1f;
            var hits = Physics.RaycastAll(new Ray(position, Vector3.down),
                float.MaxValue);
            foreach (RaycastHit hit in hits) {
                var bf = hit.collider.gameObject.GetComponentInParent<BuildingFoundation>();
                if (bf != null) {
                    bf.BuildMulti(building.BuildingData.BuildingShape);
                }
            }
        }

        BeeManager.Instance.OnLoad(loadedBees);

        foreach (SavedTransform savedTransform in _currentSave.enemyBuildings) {
            GameObject go = Resources.Load<GameObject>(ResourceLoad.EnemyBuilding);
            Object.Instantiate(go, savedTransform.Position, savedTransform.Rotation);
        }

        for (int i = 0; i < _currentSave.resources.Count; i++) {
            ResourceManagement.Instance.resourceList[i].CopySavedResource(_currentSave.resources[i]);
        }

        foreach (SavedResearch research in _currentSave.researches) {
            ResearchObject obj = ResearchManager.Instance.AllResearch.Find(
                o => o.ResearchName == research.name);
            obj.CopySavedResearch(research);
            
            // If research was being researched begin research again on load
            if (obj.Timer.Active) {
                ResearchManager.Instance.ResearchTopic(obj, false);
            }
        }

        SeasonManager.Instance.SetCurrentSeason((Seasons)_currentSave.currentSeason);//set currrent season
        EnemySpawnManager.Instance.waveNumber = _currentSave.waveNumber;

        GameCamera gameCamera = Object.FindObjectOfType<GameCamera>();
        Transform cameraTransform = gameCamera.transform;
        cameraTransform.position = _currentSave.cameraTransform.Position;
        cameraTransform.rotation = _currentSave.cameraTransform.Rotation;
        gameCamera.TargetPositon = _currentSave.cameraTarget;
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
        var jsonSaves = Resources.LoadAll<TextAsset>("Saves");
        foreach (TextAsset jsonSave in jsonSaves) {
            Save save = JsonUtility.FromJson<Save>(jsonSave.text);
            if (save != null) {
                OnSaveAdded?.Invoke(save);
            }
        }

        string[] savedFiles = Directory.GetFiles(saveDirectoryPath);
        foreach (string savedFile in savedFiles) {
            if (savedFile.EndsWith(saveExtension)) {
                string json = File.ReadAllText(savedFile);
                Save save = JsonUtility.FromJson<Save>(json);
                if (save != null) {
                    OnSaveAdded?.Invoke(save);
                }
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
