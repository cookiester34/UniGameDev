﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : MonoBehaviour {
    public delegate void EmptyEvent();
    public event EmptyEvent OnBuildingPlaced;

    [SerializeField] private BuildingType buildingType;

    public ResourceType resourceType;
    [SerializeField] private BuildingData buildingData;
    [HideInInspector]
    public int buildingTeir = 0;
    public GameObject buildingTeir1;
    public GameObject buildingTeir2;
    public GameObject buildingTeir3;

    public BuildingData BuildingData => buildingData;

    public BuildingType BuildingType => buildingType;

    /// <summary>
    /// number of bees assigned to this building
    /// </summary>
    [HideInInspector]
    public int numAssignedBees;
    private List<Bee> _assignedBees;
    private List<BuildingFoundation> usedFoundations = new List<BuildingFoundation>();

    public List<BuildingFoundation> UsedFoundations {
        get => usedFoundations;
        set => usedFoundations = value;
    }

    public List<Bee> AssignedBees => _assignedBees;

    protected virtual void Start()
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
        BuildingManager.Instance.Buildings.Add(this);
    }

    public GameObject GetActiveBuilding()
    {
        switch (buildingTeir)
        {
            case 0:
                return buildingTeir1;
            case 1:
                return buildingTeir2;
            case 2:
                return buildingTeir3;
        }
        return null;
    }

    public void PlaceBuilding() {
        OnBuildingPlaced?.Invoke();
    }

    public void AssignBee(Bee bee) {
        if (_assignedBees == null) {
            _assignedBees = new List<Bee>();
        }

        if (_assignedBees.Contains(bee)) {
            Debug.LogWarning("Attempting to assign a bee that is already assigned to this building");
        }

        _assignedBees.Add(bee);
        numAssignedBees = _assignedBees.Count;
    }

    /// <summary>
    /// Attempts to unassign a bee from the building
    /// </summary>
    /// <param name="bee">A specific bee to remove, or null for a random one</param>
    /// <returns>The bee removed</returns>
    public Bee UnassignBee(Bee bee = null) {
        if (_assignedBees == null) {
            _assignedBees = new List<Bee>();
        }

        Bee beeUnassigned = bee;
        if (_assignedBees.Count < 1) {
            Debug.LogWarning("Attempting to unassign a bee when no bees are  assigned to this building");
            beeUnassigned = null;
        } else {
            if (beeUnassigned == null) {
                beeUnassigned = _assignedBees[0];
            }

            _assignedBees.Remove(beeUnassigned);
        }
        numAssignedBees = _assignedBees.Count;

        return beeUnassigned;
    }

    private void OnDestroy() {
        foreach (BuildingFoundation foundation in usedFoundations) {
            foundation.CanBuild = true;
        }
        BuildingManager.Instance.Buildings.Add(this);
    }
}
