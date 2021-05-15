using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.SceneManagement;

public class NavMeshManagement : MonoBehaviour {
    #region Instance
    private static NavMeshManagement _instance = null;
    public static NavMeshManagement Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("2nd instance of Nav Mesh Management being created, destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    private List<GraphUpdateScene> _navmeshModifiers = new List<GraphUpdateScene>();
    private bool _canScan = true;
    private bool _scanQue = false;

    /// <summary>
    /// Adds objects to the navmesh modifiers and applies them 
    /// </summary>
    /// <param name="graphUpdates">The objects blockers to add and apply</param>
    public void AddObjects(GraphUpdateScene[] graphUpdates) {
        foreach (GraphUpdateScene graphUpdate in graphUpdates) {
            graphUpdate.Apply();
        }
        _navmeshModifiers.AddRange(graphUpdates);
    }

    /// <summary>
    /// Removes objects from the navmesh modifiers and causes a rescan to remove their affect on pathfinding
    /// </summary>
    /// <param name="graphUpdates">The objects blockers to remove</param>
    public void RemoveObjects(GraphUpdateScene[] graphUpdates) {
        foreach (GraphUpdateScene graphUpdate in graphUpdates) {
            _navmeshModifiers.Remove(graphUpdate);
        }

        ReScan();
    }

    /// <summary>
    /// Removes potential null entries and begins a rescan of the pathfinding
    /// </summary>
    private void ReScan() {
        _navmeshModifiers.RemoveAll(x => x == null);
        StartCoroutine(nameof(ScanAsync));
    }

    /// <summary>
    /// Exists so that if a scan fails it will then try again
    /// </summary>
    private void Update() {
        if (_scanQue && _canScan) {
            _scanQue = false;
            ReScan();
        }
    }
    
    IEnumerator ScanAsync()
    {
        if (_canScan)
        {
            Debug.Log("Scanning Graph");
            foreach (Progress progress in AstarPath.active.ScanAsync())
            {
                //Debug.Log("Scanning... " + progress.description + " - " + (progress.progress * 100).ToString("0") + "%");
                _canScan = false;
                yield return null;
            }
            _canScan = true;
        }
        else
            _scanQue = true;
    }
}
