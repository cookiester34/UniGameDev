using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class LoadPanelHolder : MonoBehaviour {
    /// <summary>
    /// Prefab to instantiate for saves
    /// </summary>
    [SerializeField] private GameObject loadPanelPrefab;
    
    /// <summary>
    /// Rect transform component, used to update layout
    /// </summary>
    private RectTransform _rectTransform;

    private void Awake() {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        if (_rectTransform == null) {
            Debug.LogError("The LoadPanelHolder has been placed on "
                           + gameObject.name + " which has no rect transform, cannot force layout rebuild");
        }

        if (loadPanelPrefab == null) {
            Debug.LogError("Load Panel Holder is missing the prefab to use for new load panels");
        }

        SaveLoad.OnSaveAdded += AddLoadPanel;
        SaveLoad.CheckExistingSaves();
    }

    /// <summary>
    /// Adds a load panel to the scrollable so that the save may be loaded
    /// </summary>
    /// <param name="savename">Name of the save</param>
    void AddLoadPanel(string savename) {
        GameObject go = Instantiate(loadPanelPrefab, transform);
        LoadPanel loadPanel = go.GetComponent<LoadPanel>();
        loadPanel.SetText(savename);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
