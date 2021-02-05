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

    public bool CanBuild {
        get => _canBuild;
        set => _canBuild = value;
    }

    public Renderer Renderer => _renderer;

    private void Awake() {
        _hexPanel = GetComponent<HexPanel>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    /// <summary>
    /// Gets the central position for the building
    /// </summary>
    /// <param name="buildingSize">NUmber of tiles the building uses</param>
    /// <returns>The central position for the building</returns>
    public Vector3 BuildingPosition(int buildingSize) {
        Vector3 buildingCenter = Vector3.zero;
        if (buildingSize == 1) {
            buildingCenter = _renderer.bounds.center;
        } else {
            // Currently assuming size to b 2, which means it will build on this tile, its below right, and below
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

        if (buildTiles == 1) {
            canBuild = _canBuild;
            _canBuild = false;
        } else {
            
            HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
            HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
            canBuild = br.BuildingFoundation.CanBuild && ar.BuildingFoundation.CanBuild && BuildMulti(buildTiles - 1);
            br.BuildingFoundation.CanBuild = false;
            ar.BuildingFoundation.CanBuild = false;
        }

        return canBuild;
    }
}
