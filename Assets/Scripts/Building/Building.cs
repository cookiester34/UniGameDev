using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : MonoBehaviour {
    public delegate void BuildingPlaced();
    public event BuildingPlaced OnBuildingPlaced;

    public ResourceType resourceType;
    [SerializeField] private BuildingData buildingData;
    [HideInInspector]
    public int buildingTeir = 0;
    public GameObject buildingTeir1;
    public GameObject buildingTeir2;
    public GameObject buildingTeir3;

    public BuildingData BuildingData => buildingData;

    /// <summary>
    /// number of bees assigned to this building
    /// </summary>
    [HideInInspector]
    public int assignedBees;

    private void Start()
    {
        if (buildingTeir1 != null) {
            buildingTeir1.SetActive(buildingTeir == 0);
        }

        if (buildingTeir2 != null) {
            buildingTeir2.SetActive(buildingTeir == 1);
        }

        if (buildingTeir3 != null) {
            buildingTeir3.SetActive(buildingTeir == 2);
        }
    }

    public void PlaceBuilding() {
        OnBuildingPlaced?.Invoke();
    }
}
