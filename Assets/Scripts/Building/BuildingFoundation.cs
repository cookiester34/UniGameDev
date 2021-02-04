using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexPanel))]
public class BuildingFoundation : MonoBehaviour {
    private HexPanel _hexPanel;
    private bool _canBuild;

    private void Awake() {
        _hexPanel = GetComponent<HexPanel>();
    }

    public Vector3 BuildingPosition() {
        return GetComponent<Renderer>().bounds.center;
    }

    public bool CanBuild(int buildTiles) {
        bool canBuild = false;

        if (buildTiles == 1) {
            canBuild = _canBuild;
        } else {
            HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
            HexPanel b = _hexPanel.GetNeighbour(NeighbourDirection.Below);
            canBuild = br != null && b != null;
        }

        return canBuild;
    }
}
