using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SelectionState : BuildingManagerState {
    private GameObject selectedBuilding;
    public Building selectedBuildingData;

    public override void Enter() {
        selectedBuilding = null;
        selectedBuildingData = null;
        buildingManager.SetUIImage(false);
    }

    public override void Exit() {
        EnableGlow(false);
        buildingManager.BuildingSelected(null);
        selectedBuilding = null;
        selectedBuildingData = null;
    }

    public override void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingManager.buildingMask)) {
                if (hit.transform.CompareTag("Building")) {
                    EnableGlow(false);
                    selectedBuilding = hit.transform.gameObject;
                    selectedBuildingData = selectedBuilding.GetComponent<Building>();
                    buildingManager.BuildingSelected(selectedBuildingData);
                    EnableGlow(true);
                }
            }
        } else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            EnableGlow(false);
            selectedBuilding = null;
            selectedBuildingData = null;
            buildingManager.BuildingSelected(null);
        }
    }
    
    public void AddBeeToBuilding() {
        if(selectedBuilding != null && selectedBuildingData != null)
        {
            if (selectedBuildingData.CanAssignBee())
            {
                Resource temp = ResourceManagement.Instance.GetResource(ResourceType.AssignedPop);
                if (temp != null)
                {
                    if (temp.CurrentResourceAmount < temp.ResourceCap)
                    {
                        BeeManager.Instance.AssignBeeToBuilding(selectedBuildingData);
                        temp.ModifyAmount(1);
                    }
                }
                else
                    buildingManager.NoResourceFound(ResourceType.AssignedPop);
            }
        }
    }

    public void RemoveBeeFromBuilding() {
        if (selectedBuilding != null && selectedBuildingData != null) {
            if (selectedBuildingData.numAssignedBees > 0) {
                Resource temp = ResourceManagement.Instance.GetResource(ResourceType.AssignedPop);
                if (temp != null) {
                    BeeManager.Instance.UnassignBeeFromBuilding(selectedBuildingData);
                    temp.ModifyAmount(-1);
                    buildingManager.BuildingSelected(selectedBuildingData);
                } else
                    buildingManager.NoResourceFound(ResourceType.AssignedPop);
            }
        }
    }

    public void UpgradeBuilding() {
        if (selectedBuildingData != null) {
            if (!selectedBuildingData.CanUpgrade()) {
                UIEventAnnounceManager.Instance.AnnounceEvent("Cannot upgrade prerequisites not met", AnnounceEventType.Misc);
                return;
            }

            List<ResourcePurchase> temp = new List<ResourcePurchase>();
            switch (selectedBuildingData.BuildingTier) {
                case 1:
                    temp = selectedBuildingData.BuildingData.Tier2Cost;
                    break;
                
                case 2:
                    temp = selectedBuildingData.BuildingData.Tier3Cost;
                    break;
                
                case 3:
                    temp = null;
                    break;
            }

            if (temp != null && ResourceManagement.Instance.UseResources(temp)) {
                PlayBuildingPlaceParticles(selectedBuildingData.transform);
                selectedBuildingData.Upgrade();
            }
        }
    }
    
    private void PlayBuildingPlaceParticles(Transform parent) {
        parent.DOShakeScale(0.5f, 0.5f);
        GameObject go = Object.Instantiate(buildingManager.BuildingPlaceParticles, parent, false);
        ParticleSystem particles = go.GetComponent<ParticleSystem>();
        particles.Play();
    }

    private void EnableGlow(bool enable) {
        if (selectedBuilding != null) {
            var glowEnabler = selectedBuilding.GetComponent<GlowEnabler>();
            if (glowEnabler != null) {
                glowEnabler.EnableGlow(enable);
            }
        }
    }

    public SelectionState(BuildingManager buildingManager) : base(buildingManager) { }
}
