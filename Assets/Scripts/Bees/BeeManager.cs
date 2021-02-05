using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

/// <summary>
/// Bee manager will be used for adding bees/ removing bees, as a central host
/// </summary>
public class BeeManager : MonoBehaviour {
    private List<Bee> _bees = new List<Bee>();

    private static BeeManager _instance;

    public static BeeManager Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.BeeSingleton);
                Instantiate(go);
                _instance = go.GetComponent<BeeManager>();
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
        population.OnCurrentValueChanged += OnPopulationChange;
    }

    public void OnPopulationChange(int newValue) {
        
    }
}
