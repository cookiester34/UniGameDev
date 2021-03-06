﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

/// <summary>
/// Bee manager will be used for adding bees/ removing bees, as a central host
/// </summary>
public class BeeManager : MonoBehaviour {
    private List<Bee> _bees = new List<Bee>();
    private int spawnQueue = 0;
    private Timer spawnTimer = new Timer(0.5f);
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private GameObject beeSpawn;

    private static BeeManager _instance = null;
    public delegate void BeeAssignedHandler(Building building);
    public event BeeAssignedHandler BeeAssigned;

    private int _cachedPopulation = 0;

    public List<Bee> Bees => _bees;

    public static BeeManager Instance {
        get {
            if (_instance == null) {
                try
                {
                    GameObject go = Resources.Load<GameObject>(ResourceLoad.BeeSingleton); //NOTE: This doesn't work. The prefab at the specified path doesn't exist. Even if it did, a queen bee building would spawn at a weird location
                    Instantiate(go);
                } catch
                {
                    return null; //doing this so that code in DisasterManager doesn't completely break
                }
            }

            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null) {
            Debug.LogWarning("2nd instance of bee manager being created, destroy");
            Destroy(gameObject);
            return;
        }

        _instance = this;

        Resource population = ResourceManagement.Instance.GetResource(ResourceType.Population);
        _cachedPopulation = Mathf.FloorToInt(population.CurrentResourceAmount);
        population.OnCurrentValueChanged += OnPopulationChange;
        spawnTimer.OnTimerFinish += SpawnBee;
        spawnTimer.Start();
    }

    private void Update() {
        spawnTimer.Tick(Time.deltaTime);
    }

    private void OnDestroy() {
        if (!ApplicationUtil.IsQuitting) {
            Resource population = ResourceManagement.Instance.GetResource(ResourceType.Population);
            population.OnCurrentValueChanged -= OnPopulationChange;

            if (!ApplicationUtil.IsLoading && CurrentSceneType.SceneType == SceneType.GameLevel) {
                GameUI.Instance.ShowGameOver();
            }
        }
    }

    public void OnPopulationChange(float populationNewValue) {
        int populationChange = Mathf.FloorToInt(populationNewValue - _cachedPopulation);
        if (populationChange > 0) {
            spawnQueue += populationChange;
        }
        _cachedPopulation += populationChange;
    }

    private void SpawnBee() {
        if (spawnQueue > 0) {
            GameObject go = Instantiate(beePrefab, beeSpawn.transform.position, beeSpawn.transform.rotation);
            Bee bee = go.GetComponent<Bee>();
            _bees.Add(bee);
            spawnQueue--;
            ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).ModifyCap(1);
        }
        spawnTimer.Reset(true);
    }

    public void AssignBeeToBuilding(Building building) {
        switch (building.BuildingType) {
            case BuildingType.Housing:
                foreach (Bee bee in _bees) {
                    if (bee.Home == null) {
                        bee.Home = building;
                        building.AssignBee(bee);
                        break;
                    }
                }
                break;
            
            default:
                foreach (Bee bee in _bees) {
                    if (bee.Work == null) {
                        bee.Work = building;
                        building.AssignBee(bee);
                        break;
                    }
                }
                break;
        }

        BeeAssigned?.Invoke(building);
    }

    public void UnassignBeeFromBuilding(Building building) {
        switch (building.BuildingType) {
            case BuildingType.Housing: {
                Bee unassignedBee = building.UnassignBee();
                unassignedBee.Home = null;
                break;
            }

            default: {
                Bee unassignedBee = building.UnassignBee();
                unassignedBee.Work = null;
                break;
            }
        }
    }

    /// <summary>
    /// gets called when a building has it's assigned bee's number change
    /// </summary>
    public void OnAssignedBeeChange(Building building) {
        switch (building.BuildingType) {
            case BuildingType.QueenBee:
                // fall through
            case BuildingType.Housing:
                foreach (Bee bee in _bees) {
                    if (bee.Home == null) {
                        bee.Home = building;
                        break;
                    }
                }
                break;
            
            default:
                foreach (Bee bee in _bees) {
                    if (bee.Work == null) {
                        bee.Work = building;
                        break;
                    }
                }
                break;
            
        }
        BeeAssigned?.Invoke(building);
    }

    public void OnLoad(List<Bee> bees) {
        spawnQueue = 0;
        _bees = bees;
        _cachedPopulation = bees.Count;
    }
}

/// <summary>
/// Made so when loading a save back in bees can get re-assigned correctly, this is pretty bad code, and the whole bee
/// manager wants redoing so it is easier to work with but a deadline looms and changing it too dramatically at this
/// point is rather scary as it may introduce more bugs
/// </summary>
public static class BeeAssign {
    public static void AssignBeeToBuilding(Building building, Bee bee) {
        switch (building.BuildingType) {
            case BuildingType.Housing:
                if (bee.Home == null) {
                    bee.Home = building;
                    building.AssignBee(bee);
                }
                break;

            default:
                if (bee.Work == null) {
                    bee.Work = building;
                    building.AssignBee(bee);
                }
                break;
        }
    }
}
