using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Research;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Building : MonoBehaviour {
    public delegate void EmptyEvent();
    public event EmptyEvent OnBuildingPlaced;

    [SerializeField] private BuildingType buildingType;

    [SerializeField] private BuildingData buildingData;
    private int buildingTier = 1;
    [FormerlySerializedAs("buildingTeir1")] public GameObject buildingTier1;
    [FormerlySerializedAs("buildingTeir2")] public GameObject buildingTier2;
    [FormerlySerializedAs("buildingTeir3")] public GameObject buildingTier3;

    public BuildingData BuildingData => buildingData;

    public BuildingType BuildingType => buildingType;
    public int BuildingTier => buildingTier;

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
        if (buildingTier1 != null) {
            buildingTier1.SetActive(buildingTier == 0);
        }

        if (buildingTier2 != null) {
            buildingTier2.SetActive(buildingTier == 1);
        }

        if (buildingTier3 != null) {
            buildingTier3.SetActive(buildingTier == 2);
        }
        BuildingManager.Instance.AddBuildingToBuildings(this);
    }

    public GameObject GetActiveBuilding()
    {
        switch (buildingTier)
        {
            case 0:
                return buildingTier1;
            case 1:
                return buildingTier2;
            case 2:
                return buildingTier3;
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

        if (!ApplicationUtil.IsQuitting) {
            BuildingManager.Instance.RemoveFromBuildings(this);
        }
    }

    public bool CanUpgrade() {
        return buildingData.CanUpgrade(buildingTier + 1);
    }

    public void Upgrade() {
        buildingTier++;
        buildingTier1.SetActive(buildingTier == 1);
        buildingTier2.SetActive(buildingTier == 2);
        buildingTier3.SetActive(buildingTier == 3);
    }
}
