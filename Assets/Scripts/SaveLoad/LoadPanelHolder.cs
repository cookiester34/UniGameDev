﻿using System;
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

    [SerializeField] private RectTransform levelsTitle;
    [SerializeField] private RectTransform playersTitle;
    
    /// <summary>
    /// Rect transform component, used to update layout
    /// </summary>
    private RectTransform _rectTransform;

    private List<LoadPanel> _loadPanels = new List<LoadPanel>();

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
    /// Adds a load panel to the scrollable so that the save may be loaded if there is no loadPanel with its name
    /// </summary>
    /// <param name="savename">Name of the save</param>
    void AddLoadPanel(Save save) {
        LoadPanel loadPanel = _loadPanels.Find(panel => panel.Savename == save.name);
        if (loadPanel == null) {
            // TODO: differentiate between a player save and in game save for the prefab, an in game save should not support delete
            RectTransform parent = save.playerMade ? playersTitle : levelsTitle;
            GameObject go = Instantiate(loadPanelPrefab, parent);
            loadPanel = go.GetComponent<LoadPanel>();
            loadPanel.IncludeDeleteButton = save.playerMade;
            _loadPanels.Add(loadPanel);
            loadPanel.SetText(save.name);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
    }

    private void OnDestroy() {
        SaveLoad.OnSaveAdded -= AddLoadPanel;
    }
}
