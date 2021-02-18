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
    [SerializeField] private GameObject QueenBeeBuilding;

    private static BeeManager _instance = null;
    public delegate void AssignedBeeUpdate();
    public static event AssignedBeeUpdate AssignedBeeUpdated;

    private float _cachedPopulation = 0;

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
        _cachedPopulation = population.CurrentResourceAmount;
        population.OnCurrentValueChanged += OnPopulationChange;
    }

    public void OnPopulationChange(float populationNewValue) {
        QueenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        float populationChange = populationNewValue - _cachedPopulation;
        if (populationChange > 0) {
            for (int i = 0; i < populationChange; i++) {
                GameObject go = Instantiate(beePrefab, QueenBeeBuilding.transform);
                Bee bee = go.GetComponent<Bee>();
                _bees.Add(bee);
            }
        } else {
            for (int i = 0; i < populationChange; i++) {
                Bee bee = _bees[Random.Range(0, _bees.Count)];
                Destroy(bee);
            }
        }
        ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).OverrideCap((int)populationNewValue);
        _cachedPopulation = populationNewValue;
    }

    /// <summary>
    /// gets called when a building has it's assigned bee's number change
    /// </summary>
    public void OnAssignedBeeChange()
    {
        AssignedBeeUpdated();
    }
}
