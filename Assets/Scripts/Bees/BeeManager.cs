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
    [SerializeField] private GameObject beePrefab;

    private static BeeManager _instance = null;

    private int _cachedPopulation = 0;

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
        GetComponent<Building>().OnBuildingPlaced += delegate {
            population.ModifyAmount(10f);
        };
    }

    private void Start() {
        ResourceStorage[] suppliers = FindObjectsOfType<ResourceStorage>();
        foreach (ResourceStorage storage in suppliers) {
            if (storage.Resource.resourceType == ResourceType.Population 
                && storage.gameObject.GetComponent<Building>() != null) {
            }
        }

        foreach (Bee bee in _bees) {
            if (bee.Home == null) {
                var housing = BuildingManager.Instance.GetAllStorageBuildingsOfType(ResourceType.Population);
                bee.Home = housing[Random.Range(0, housing.Count)];
            }
        }
    }

    public void OnPopulationChange(float populationNewValue) {
        int populationChange = Mathf.FloorToInt(populationNewValue - _cachedPopulation);
        if (populationChange > 0) {
            for (int i = 0; i < populationChange; i++) {
                GameObject go = Instantiate(beePrefab, gameObject.transform);
                Bee bee = go.GetComponent<Bee>();
                var housing = BuildingManager.Instance.GetAllStorageBuildingsOfType(ResourceType.Population);
                bee.Home = housing[Random.Range(0, housing.Count)];
                _bees.Add(bee);
            }
        } else {
            for (int i = 0; i < populationChange; i++) {
                Bee bee = _bees[Random.Range(0, _bees.Count)];
                Destroy(bee);
            }
        }
        ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).OverrideCap((int)populationNewValue);
        _cachedPopulation += populationChange;
    }
}
