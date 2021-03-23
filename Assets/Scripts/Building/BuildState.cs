﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildState : BuildingManagerState {
    GameObject tempBuilding;
    private BuildingData currentBuilding;
    private MaterialPropertyBlock _propBlock;

    public override void Enter() {
        if (_propBlock == null) {
            _propBlock = new MaterialPropertyBlock();
        }

        buildingManager.SetUIImage(true);
    }

    public override void Exit() {
        GameObject.Destroy(tempBuilding);
        buildingManager.selectedBuildingText.text = "No building is selected";
        buildingManager.selectedBuildingUI.sprite = null;
        BuildingFoundation.Hide();
    }

    public override void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (EventSystem.current.IsPointerOverGameObject()) {
            //this checks if the mouse is over a UI element
            tempBuilding.SetActive(false);
        } else if (PhysicsUtil.Raycast(ray, out hit, Mathf.Infinity, buildingManager.tileMask, false)) {
            tempBuilding.SetActive(true);
            BuildingFoundation foundation = hit.collider.GetComponentInParent<BuildingFoundation>();
            if (foundation != null) {
                Vector3 buildPosition = foundation.BuildingPosition(currentBuilding.BuildingShape);
                tempBuilding.transform.position = buildPosition;

                if (Input.GetMouseButtonDown(0)) {
                    PlaceBuilding(buildPosition, foundation);
                } else {
                    bool canBuild = foundation.BuildMulti(currentBuilding.BuildingShape, false);
                    UpdateBuildingShader(true, canBuild);
                }
            } else {
                Debug.Log("Foundation was null");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        }
    }

    public void SetupBuilding(BuildingData buildingData) {
        if (buildingData == null) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        } else {
            bool isInBuildingLimit = GetIsInBuildingLimit(buildingData);
            bool canUseResources = ResourceManagement.Instance.CanUseResources(buildingData.Tier1Cost);

            if (!canUseResources) {
                UIEventAnnounceManager.Instance.AnnounceEvent("Not enough resources to place building!");
                buildingManager.SetBuildMode(BuildingMode.Selection);
            } else if (!isInBuildingLimit) {
                UIEventAnnounceManager.Instance.AnnounceEvent("Building limit reached for this building type!");
                buildingManager.SetBuildMode(BuildingMode.Selection);
            } else {
                buildingManager.selectedBuildingUI.sprite = buildingData.UiImage;
                buildingManager.selectedBuildingText.text = buildingData.Description;
                currentBuilding = buildingData;
                var buildingModel = currentBuilding.BuildingType.GetModel();
                tempBuilding = GameObject.Instantiate(buildingModel,
                    new Vector3(0, 0, 0), buildingModel.transform.rotation);
            }
        }
    }
    
    /// <summary>
    /// final placement of the building when mouse is clicked
    /// </summary>
    /// <param name="position"></param>
    private void PlaceBuilding(Vector3 position, BuildingFoundation foundation)
    {
        if (!foundation.BuildMulti(currentBuilding.BuildingShape)) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
            buildingManager.BuildingAlreadyThere();
        } else {
            tempBuilding.transform.position = position;
            ResourceManagement.Instance.UseResources(currentBuilding.Tier1Cost);

            Object.Destroy(tempBuilding);
            var prefab = currentBuilding.BuildingType.GetPrefab();
            GameObject go = Object.Instantiate(prefab, position, prefab.transform.rotation);
            PlayBuildingPlaceParticles(go.transform);

            Building placedBuilding = go.GetComponent<Building>();
            if (placedBuilding != null) {
                placedBuilding.UsedFoundations = foundation.GetFoundations(currentBuilding.BuildingShape);
                placedBuilding.PlaceBuilding();
            }
            UpdateBuildingShader(false, false);

            AudioManager.Instance.PlaySound("PlaceBuilding");
            tempBuilding = null;
            if (ResourceManagement.Instance.CanUseResources(currentBuilding.Tier1Cost) &&
                GetIsInBuildingLimit(currentBuilding)) {
                SetupBuilding(currentBuilding);
            } else {
                buildingManager.SetBuildMode(BuildingMode.Selection);
            }
        }
    }
    
    private bool GetIsInBuildingLimit(BuildingData buildingData) {
        int buildingTypeIndex = (int)buildingData.BuildingType;
        return buildingData.MaxInstances > buildingManager.numBuildingTypes[buildingTypeIndex];
    }
    
    private void PlayBuildingPlaceParticles(Transform parent) {
        parent.DOShakeScale(0.5f, 0.5f);
        GameObject go = Object.Instantiate(buildingManager.BuildingPlaceParticles, parent, false);
        ParticleSystem particles = go.GetComponent<ParticleSystem>();
        particles.Play();
    }

    private void UpdateBuildingShader(bool useShader, bool canBuild) {
        Renderer[] renderers = tempBuilding.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers) {
            renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("CanBuild", canBuild ? 1f : 0f);
            _propBlock.SetFloat("UseShader", useShader ? 1f : 0f);
            renderer.SetPropertyBlock(_propBlock);
        }
    }

    public BuildState(BuildingManager buildingManager) : base(buildingManager) { }
}

