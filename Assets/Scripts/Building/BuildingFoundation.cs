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

    private bool _canBuild = true;

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
        switch (buildingSize) {
            case 1:
                buildingCenter = _renderer.bounds.center;
                break;

            case 2:
                // Currently assuming size to be 2, which means it will build on this tile, its below right, and below
                HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);

                if (br != null && ar != null) {
                    BuildingFoundation brFoundation = br.BuildingFoundation;
                    BuildingFoundation bFoundation = ar.BuildingFoundation;

                    if (br != null && ar != null) {
                        buildingCenter = (BuildingPosition(1) + bFoundation.BuildingPosition(1)
                                                              + brFoundation.BuildingPosition(1)) / 3;
                    }
                }
                break;

            case 3:
                List<HexPanel> neighbours = _hexPanel.GetNeighbours();
                buildingCenter = Vector3.zero;
                foreach (HexPanel neighbour in neighbours) {
                    if (neighbour.BuildingFoundation != null) {
                        buildingCenter += neighbour.BuildingFoundation._renderer.bounds.center;
                    }
                }

                buildingCenter /= neighbours.Count;
                break;
            
            default:
                Debug.LogError("Building size of: " + buildingSize + " not supported");
                break;
        }

        return buildingCenter;
    }

    /// <summary>
    /// Checks if the tiles are available for building and changes them to not available if they can
    /// </summary>
    /// <param name="buildTiles">Size of tiles that the building will take</param>
    /// <returns>Whether the building can be built</returns>
    public bool BuildMulti(int buildTiles) {
        bool canBuild = false;

        switch (buildTiles) {
            case 1:
                canBuild = _canBuild;
                _canBuild = false;
                UpdateVisibleColour();
                break;

            case 2:
                HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                canBuild = _canBuild && br.BuildingFoundation.CanBuild && ar.BuildingFoundation.CanBuild;
                if (canBuild) {
                    br.BuildingFoundation.CanBuild = false;
                    ar.BuildingFoundation.CanBuild = false;
                    _canBuild = false;
                }
                UpdateVisibleColour();

                break;

            case 3:
                List<HexPanel> neighbours = _hexPanel.GetNeighbours();
                canBuild = neighbours.Count == 6 && CanBuild;

                if (canBuild) {
                    foreach (HexPanel neighbour in _hexPanel.GetNeighbours()) {
                        canBuild = neighbour.BuildingFoundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }
                }

                if (canBuild) {
                    _canBuild = false;
                    foreach (HexPanel neighbour in _hexPanel.GetNeighbours()) {
                        neighbour.BuildingFoundation.CanBuild = false;
                    }
                }
                UpdateVisibleColour();
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
                HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                foundations.Add(ar.BuildingFoundation);
                foundations.Add(br.BuildingFoundation);
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
            _renderer = GetComponentInChildren<Renderer>();
        }

        if (_propBlock == null) {
            _propBlock = new MaterialPropertyBlock();
        }

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
