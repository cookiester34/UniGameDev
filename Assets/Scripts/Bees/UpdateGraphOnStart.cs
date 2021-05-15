using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

/// <summary>
/// Small component that expects pathfinding modifiers to be on children to then add them and remove them as required
/// </summary>
public class UpdateGraphOnStart : MonoBehaviour {
    private GraphUpdateScene[] _updateScenes;

    void Start() {
        _updateScenes = GetComponentsInChildren<GraphUpdateScene>();
        NavMeshManagement.Instance.AddObjects(_updateScenes);
    }

    private void OnDestroy() {
        NavMeshManagement.Instance.RemoveObjects(_updateScenes);
    }
}
