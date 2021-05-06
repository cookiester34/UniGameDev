using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour {
    public delegate void BuildingEvent(Building building);
    public event BuildingEvent OnBuildingSelected;
    public event BuildingEvent OnBuildingPlaced;

    #region states
    
    private BuildingManagerState _currentState;
    private BuildState _buildState;
    private SelectionState _selectionState;
    private DestroyState _destroyState;

    #endregion

    public int[] numBuildingTypes;

    /// <summary>
    /// Mask to use when placing, destroying, or selecting
    /// </summary>
    public LayerMask buildingMask;
    public LayerMask tileMask;

    // TODO: Two UI elements should receive an event when they need to update rather than manager telling them directly
    public Image selectedBuildingUI;
    public TMP_Text selectedBuildingText;

    public Sprite DestroyUISprite;

    /// <summary>
    /// List of buildings that have been placed in the level
    /// </summary>
    private List<Building> _buildings = new List<Building>();
    
    /// <summary>
    /// Prefab of particles to play when the building is placed
    /// </summary>
    [Header("Build State")]
    [SerializeField] private GameObject buildingPlaceParticles;

    private static BuildingManager _instance = null;

    public GameObject BuildingPlaceParticles => buildingPlaceParticles;
    public List<Building> Buildings => _buildings;

    public BuildingManagerState CurrentState => _currentState;

    public static BuildingManager Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.BuildingSingleton);
                Instantiate(go);
            }

            return _instance;
        }
    }


    /// <summary>
    /// Sets up singleton instance
    /// </summary>
    private void Awake() {
        if (_instance != null) {
            Debug.LogError("A 2nd instance of the building manager has been created, will now destroy");
            Destroy(this);
            return;
        }

        _instance = this;
		numBuildingTypes = new int[Enum.GetNames(typeof(BuildingType)).Length];
        GameObject go = GameObject.Find("SelectedBuildingUIImage");
        if (go != null) {
            selectedBuildingUI = go.GetComponent<Image>();
            selectedBuildingText = go.GetComponentInChildren<TMP_Text>();
        }
        InitStates();
        SetBuildMode(BuildingMode.Selection);
    }

    private void InitStates() {
        _buildState = new BuildState(this);
        _selectionState = new SelectionState(this);
        _destroyState = new DestroyState(this);
    }
	
	public bool GetIsBuildingOrDestroying() {
		return _currentState != _selectionState;
	}

    public void SetBuildMode(BuildingMode mode, BuildingData buildingData = null) {
        _currentState?.Exit();
        // Do stuff before leaving the mode
        switch (mode) {
            case BuildingMode.Selection:
                _currentState = _selectionState;
                break;
            case BuildingMode.Destroy:
                _currentState = _destroyState;
                break;
            case BuildingMode.Build:
                _currentState = _buildState;
                break;
        }
        
        _currentState?.Enter();
        if (_currentState is BuildState buildState) {
            buildState.SetupBuilding(buildingData);
        }
    }

    private void Update() {
        _currentState?.Update();
    }

    public void BuildingSelected(Building building) {
        OnBuildingSelected?.Invoke(building);
    }

    public void InvokeBuildingPlaced(Building building) {
        OnBuildingPlaced?.Invoke(building);
    }

    /// <summary>
    /// for ui button to add a bee to the selected building
    /// </summary>
    public void AddBeeToBuilding() {
        if (_currentState is SelectionState state) {
            state.AddBeeToBuilding();
        }
    }


    public void RemoveBeeFromBuilding() {
        if (_currentState is SelectionState state) {
            state.RemoveBeeFromBuilding();
        }
    }

    /// <summary>
    /// goes through the resource costs of the selecetd building and applies a teirupgrade to the original cost
    /// </summary>
    public void UpgradeBuilding() {
        if (_currentState is SelectionState state) {
            state.UpgradeBuilding();
        }
    }

    /// <summary>
    /// Adds the building to the list incrementing the number placed
    /// </summary>
    /// <param name="building">The building to add</param>
    public void AddBuildingToBuildings(Building building) {
        _buildings.Add(building);
        numBuildingTypes[(int)building.BuildingType]++;
    }

    /// <summary>
    /// Removes the building from the list updating the number placed
    /// </summary>
    /// <param name="building">Building to remove</param>
    public void RemoveFromBuildings(Building building) {
        _buildings.Remove(building);
        numBuildingTypes[(int)building.BuildingType]--;
    }

    public List<Building> GetAllStorageBuildingsOfType(ResourceType resourceType) {
        return _buildings.FindAll(building => {
            bool add = false;
            var storage = building.GetComponent<ResourceStorage>();
            if (storage != null && storage.Resource.resourceType == resourceType) {
                add = true;
            }

            return add;
        });
    }

    public List<Building> GetAllSupplierBuildingsOfType(ResourceType resourceType) {
        return _buildings.FindAll(building => {
            bool add = false;
            var supplier = building.GetComponent<ResourceSupplier>();
            if (supplier != null && supplier.Resource.resourceType == resourceType) {
                add = true;
            }

            return add;
        });
    }

    public void SetUIImage(bool visible) {
        if (selectedBuildingUI != null) {
            Color color = selectedBuildingUI.color;
            color.a = visible ? 255 : 0;
            selectedBuildingUI.color = color;
        }
    }

    #region Debugs
    private void NoBuildingFound(BuildingType buildingType)
    {
        Debug.LogWarning("No building of type: " + buildingType.ToString() + " :found");
    }
    public void BuildingAlreadyThere()
    {
        Debug.LogWarning("Building is already there");
		UIEventAnnounceManager.Instance.AnnounceEvent("Building already exists here.", AnnounceEventType.Misc);
    }

    public void NoResourceFound(ResourceType resourceType)
    {
        Debug.LogWarning("No building of type: " + resourceType.ToString() + " :found");
    }

    private void NotEnoughResources()
    {
        Debug.LogWarning("Not Enough Resources To Make Purchase");
    }
    #endregion
}

public enum BuildingMode {
    Selection,
    Build,
    Destroy
}
