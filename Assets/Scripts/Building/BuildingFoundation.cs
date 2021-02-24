using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Foundation that can be built on, handles whether the tile can be built on for differing sizes
/// </summary>
[RequireComponent(typeof(HexPanel))]
public class BuildingFoundation : MonoBehaviour {
    private HexPanel _hexPanel;
    private Renderer _renderer;

    [SerializeField] private bool _canBuild = true;

    private static Color canBuildColor = new Color(0.2f, 0.8f, 0.2f, 0.6f);
    private static Color cannotBuildColor = new Color(0.8f, 0.2f, 0.2f, 0.6f);
    private static Color invisibleColor = new Color(0f, 0f, 0f, 0f);
    private static Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    private MaterialPropertyBlock _propBlock;

    public bool CanBuild {
        get => _canBuild;
        set {
            _canBuild = value;
            UpdateVisibleColour();
        }
    }

    private void Awake() {
        _hexPanel = GetComponent<HexPanel>();
        _renderer = GetComponentInChildren<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        UpdateVisibleColour();
    }

    /// <summary>
    /// Gets the central position for the building
    /// </summary>
    /// <param name="buildingSize">Number of tiles the building uses</param>
    /// <returns>The central position for the building</returns>
    public Vector3 BuildingPosition(int buildingSize) {
        Vector3 buildingCenter = Vector3.zero;
        List<BuildingFoundation> foundations = GetFoundations(buildingSize);
        foreach (BuildingFoundation foundation in foundations) {
            buildingCenter += foundation._renderer.bounds.center;
        }

        buildingCenter /= foundations.Count;
        return buildingCenter;
    }

    /// <summary>
    /// Checks if the tiles are available for building and changes them to not available if they can
    /// </summary>
    /// <param name="buildTiles">Size of tiles that the building will take</param>
    /// <returns>Whether the building can be built</returns>
    public bool BuildMulti(int buildTiles, bool updateBuildStatus = true) {
        bool canBuild = false;
        List<BuildingFoundation> foundations = GetFoundations(buildTiles);

        switch (buildTiles) {
            case 1:
                canBuild = _canBuild;
                if (updateBuildStatus) {
                    CanBuild = false;
                }

                break;

            case 2:
                if (foundations.Count == 3) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;

            case 3:
                if (foundations.Count == 7) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;
        }

        return canBuild;
    }

    public List<BuildingFoundation> GetFoundations(int buildTiles) {
        List<BuildingFoundation> foundations = new List<BuildingFoundation>();
        switch (buildTiles) {
            case 1:
                foundations.Add(this);
                break;
            
            case 2:
                foundations.Add(this);
                HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                if (ar != null) {
                    foundations.Add(ar.BuildingFoundation);
                }

                if (br != null) {
                    foundations.Add(br.BuildingFoundation);
                }

                break;

            case 3:
                foundations.Add(this);
                foreach (HexPanel neighbour in _hexPanel.GetNeighbours()) {
                    foundations.Add(neighbour.BuildingFoundation);
                }
                break;
        }

        return foundations;
    }


    private void UpdateVisibleColour() {
        UpdateVisibleColour(_canBuild ? canBuildColor : cannotBuildColor);
    }

    private void UpdateVisibleColour(Color color) {
        if (_renderer == null) {
            return;
        }

        if (_propBlock == null) {
            _propBlock = new MaterialPropertyBlock();
        }

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
