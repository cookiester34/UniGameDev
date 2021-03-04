using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SelectionState : BuildingManagerState {
    private GameObject selectedBuilding;
    public Building selectedBuildingData;
    public int tier1UpgradeCost = 1;
    public int tier2UpgradeCost = 1;
    public override void Enter() {
        selectedBuilding = null;
        selectedBuildingData = null;
        buildingManager.SetUIImage(false);
    }

    public override void Exit() {
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
                    selectedBuilding = hit.transform.gameObject;
                    selectedBuildingData = selectedBuilding.GetComponent<Building>();
                    buildingManager.BuildingSelected(selectedBuildingData);
                }
            }
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            selectedBuilding = null;
            selectedBuildingData = null;
            buildingManager.BuildingSelected(null);
        }
    }
    
    public void AddBeeToBuilding() {
        if(selectedBuilding != null && selectedBuildingData != null)
        {
            BuildingData buildingData = selectedBuildingData.BuildingData;
            if (selectedBuildingData.numAssignedBees < buildingData.maxNumberOfWorkers)
            {
                Resource temp = ResourceManagement.Instance.GetResource(ResourceType.AssignedPop);
                if (temp != null)
                {
                    if (temp.CurrentResourceAmount < temp.ResourceCap)
                    {
                        BeeManager.Instance.AssignBeeToBuilding(selectedBuildingData);
                        temp.ModifyAmount(1);
                        buildingManager.BuildingSelected(selectedBuildingData);
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
        if(selectedBuildingData != null) {
            bool canUse = true;
            List<ResourcePurchase> temp = selectedBuildingData.BuildingData.ResourcePurchase.ToList();
            for(int i = 0; i < selectedBuildingData.BuildingData.ResourcePurchase.Count; i++) {
                buildingManager.BuildingSelected(selectedBuildingData);
                if (selectedBuildingData.buildingTeir == 0)
                    temp[i].cost *= tier1UpgradeCost;
                else if (selectedBuildingData.buildingTeir == 1)
                    temp[i].cost *= tier2UpgradeCost;
                else if (selectedBuildingData.buildingTeir > 1)//if is max teir can't uprgrade
                    canUse = false;
                if (!ResourceManagement.Instance.CanUseResource(temp[i]))//if doesn't have the resources can't upgrade
                {
                    canUse = false;
                }
            }
            if (canUse) {
                PlayBuildingPlaceParticles(selectedBuildingData.transform);
                selectedBuildingData.buildingTeir++;
                ResourceManagement.Instance.UseResources(temp);
                if (selectedBuildingData.buildingTeir == 1)
                {
                    selectedBuildingData.buildingTeir1.SetActive(false);
                    selectedBuildingData.buildingTeir2.SetActive(true);
                }
                else if(selectedBuildingData.buildingTeir == 2)
                {
                    selectedBuildingData.buildingTeir2.SetActive(false);
                    selectedBuildingData.buildingTeir3.SetActive(true);
                }
            }
        }
    }
    
    private void PlayBuildingPlaceParticles(Transform parent) {
        parent.DOShakeScale(0.5f, 0.5f);
        GameObject go = Object.Instantiate(buildingManager.BuildingPlaceParticles, parent, false);
        ParticleSystem particles = go.GetComponent<ParticleSystem>();
        particles.Play();
    }

    public SelectionState(BuildingManager buildingManager) : base(buildingManager) { }
}
