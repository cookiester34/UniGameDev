using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class UpdateGraphOnStart : MonoBehaviour {
    private GraphUpdateScene[] _updateScenes;

    void Start() {
        _updateScenes = GetComponentsInChildren<GraphUpdateScene>();
        foreach (var updateScene in _updateScenes) {
            updateScene.Apply();
        }
        NavMeshManagement.Instance.towersAffectingNavmesh.Add(transform);
    }
}
