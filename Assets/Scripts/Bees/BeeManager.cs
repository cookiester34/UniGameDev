using System;
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
    public delegate void AssignedBeeUpdate();
    public static event AssignedBeeUpdate AssignedBeeUpdated;

    private int _cachedPopulation = 0;

    public List<Bee> Bees => _bees;

    public static BeeManager Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.BeeSingleton);
                Instantiate(go);
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
        }
    }

    public void OnPopulationChange(float populationNewValue) {
        int populationChange = Mathf.FloorToInt(populationNewValue - _cachedPopulation);
        if (populationChange > 0) {
            spawnQueue += populationChange;
        }
        ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).OverrideCap((int)populationNewValue);
        _cachedPopulation += populationChange;
    }

    private void SpawnBee() {
        if (spawnQueue > 0) {
            GameObject go = Instantiate(beePrefab, beeSpawn.transform.position, beeSpawn.transform.rotation);
            Bee bee = go.GetComponent<Bee>();
            _bees.Add(bee);
            spawnQueue--;
        }
        spawnTimer.Reset(true);
    }

    public void AssignBeeToBuilding(Building building) {
        switch (building.BuildingType) {
            case BuildingType.QueenBee:
            // fall through
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
    }

    public void UnassignBeeFromBuilding(Building building) {
        switch (building.BuildingType) {
            case BuildingType.QueenBee:
            // fall through
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
        AssignedBeeUpdated?.Invoke();
    }

    public void OnLoad(List<Bee> bees) {
        _bees = bees;
        _cachedPopulation = bees.Count;
    }
}
