﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour {
    public delegate void BuildingSelected(bool selected);
    public event BuildingSelected OnBuildingSelected;

    BuildingData currentBuilding;
    GameObject tempBuilding;
    private bool canPlaceBuilding;
    private bool canDestroyBuilding;
	
	private int[] numBuildingTypes;

    private bool canSelectBuilding;
    private GameObject selectedBuilding;
    [HideInInspector]
    public  Building selectedBuildingData;
    public LayerMask mask;
	private EventSystem eventSys;
	private GameObject oldTempBuilding;
	
	private Image selectedBuildingUI;
	public Sprite destroyIcon; //need this since a building data reference is not passed when destroying buildings
	private Text selectedBuildingText;

    private static BuildingManager _instance = null;

    [Header("Options")]
    [Range(2, 5)]
    public int teir1UpgradeCost;
    [Range(2, 5)]
    public int teir2UpgradeCost;

    public static BuildingManager Instance {
        get {
            if (_instance == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.BuildingSingleton);
                Instantiate(go);
            }

            return _instance;
        }
    }


    /// <summary>
    /// Sets up singleton instance
    /// </summary>
    private void Awake() {
        if (_instance != null) {
            Debug.LogError("A 2nd instance of the building manager has been created, will now destroy");
            Destroy(this);
            return;
        }

        _instance = this;
		numBuildingTypes = new int[Enum.GetNames(typeof(BuildingType)).Length];
		eventSys = GameObject.FindObjectOfType<EventSystem>();
		selectedBuildingUI = GameObject.Find("SelectedBuildingUIImage").GetComponent<Image>();
		selectedBuildingText = selectedBuildingUI.gameObject.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// This is a function for UI, the button needs to have a building pass script on it determining what building it will place
    /// </summary>
    /// <param name="buildingType"></param>
    public void PlaceBuilding(BuildingData buildingData) 
    {
		if (tempBuilding) {
			oldTempBuilding = tempBuilding;
		}
        if (buildingData == null) 
        {
            canPlaceBuilding = false;
            NoBuildingFound(buildingData.BuildingType);
        } else {
			canDestroyBuilding = false;
			bool isInBuildingLimit = GetIsInBuildingLimit(buildingData);
            canPlaceBuilding = ResourceManagement.Instance.CanUseResources(buildingData.ResourcePurchase) && isInBuildingLimit;

            if (canPlaceBuilding) {
				selectedBuildingUI.sprite = buildingData.UiImage;
				selectedBuildingText.text = buildingData.Description;
                currentBuilding = buildingData;
                tempBuilding = Instantiate(currentBuilding.BuildingType.GetPrefab(),
                    new Vector3(0, 0, 0),
                    currentBuilding.BuildingType.GetPrefab().transform.rotation);
				if (oldTempBuilding == null) {
					oldTempBuilding = tempBuilding;
				}
                tempBuilding.GetComponent<Collider>().enabled = false;
            }
			else if (!isInBuildingLimit) {
				UIEventAnnounceManager.Instance.AnnounceEvent("Building limit reached for this building type!");
                AudioManager.Instance.PlaySound("Error");
			}
			else {
				UIEventAnnounceManager.Instance.AnnounceEvent("Not enough resources to place building!");
                AudioManager.Instance.PlaySound("Error");
            }
        }
		if (oldTempBuilding && !oldTempBuilding.activeSelf) {
			Destroy(oldTempBuilding);
		}
    }
	
	private bool GetIsInBuildingLimit(BuildingData buildingData) {
		int buildingTypeIndex = (int)buildingData.BuildingType;
		return buildingData.MaxInstances > numBuildingTypes[buildingTypeIndex];
	}

    private void Update() {
        PlacingBuilding();
        DestroyingBuildings();
        SelectingBuilding();

        //should always be able to select buildings, unless placeing or destroying them
        if (!canDestroyBuilding && !canPlaceBuilding) 
            canSelectBuilding = true;
        else
            canSelectBuilding = false;
    }

    /// <summary>
    /// moves the chosen building to the centre of the tile the mouse is on.
    /// </summary>
    private void PlacingBuilding() 
    {
        if (canPlaceBuilding)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
			if (eventSys.IsPointerOverGameObject()) { //this checks if the mouse is over a UI element
				tempBuilding.SetActive(false);
			}
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
				tempBuilding.SetActive(true);
                BuildingFoundation foundation = hit.collider.GetComponentInParent<BuildingFoundation>();
                if (foundation != null) {
                    Vector3 buildPosition = foundation.BuildingPosition(currentBuilding.BuildingSize);
                    tempBuilding.transform.position = buildPosition;

                    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        PlaceBuilding(buildPosition, foundation);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                CanclePlacingBuilding();
        }
    }

    /// <summary>
    /// final placement of the building when mouse is clicked
    /// </summary>
    /// <param name="position"></param>
    private void PlaceBuilding(Vector3 position, BuildingFoundation foundation)
    {
        if (!foundation.BuildMulti(currentBuilding.BuildingSize)) {
            CanclePlacingBuilding();
            BuildingAlreadyThere();
            AudioManager.Instance.PlaySound("Error");
        } else {
            canPlaceBuilding = false;
			numBuildingTypes[(int)currentBuilding.BuildingType]++;
            tempBuilding.transform.position = position;
            ResourceManagement.Instance.UseResources(currentBuilding.ResourcePurchase);
            tempBuilding.GetComponent<Collider>().enabled = true;
            tempBuilding.GetComponent<Building>()?.PlaceBuilding();
            AudioManager.Instance.PlaySound("PlaceBuilding");
            //this is here to allow for multiple buildings to be placed at once
            if (ResourceManagement.Instance.CanUseResources(currentBuilding.ResourcePurchase) && GetIsInBuildingLimit(currentBuilding)) {
				PlaceBuilding(currentBuilding);
			}
        }
    }
    /// <summary>
    /// cancles the placement of the chosen building
    /// </summary>
    private void CanclePlacingBuilding()
    {
        canPlaceBuilding = false;
		selectedBuildingUI.sprite = null;
		selectedBuildingText.text = "No building selected.";
        Destroy(tempBuilding);
    }

    public void DestroyBuilding()
    {
		selectedBuildingUI.sprite = destroyIcon;
		selectedBuildingText.text = "Destroy placed buildings";
		canPlaceBuilding = false;
        canDestroyBuilding = true;
    }

    /// <summary>
    /// allows the user to destroy a building
    /// </summary>
    private void DestroyingBuildings()
    {
        if (canDestroyBuilding)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    if (hit.transform.CompareTag("Building")) {
						numBuildingTypes[(int)hit.transform.GetComponent<Building>().BuildingData.BuildingType]--;
                        var beforeDestroy =  hit.collider.GetComponents<IBeforeDestroy>();
                        if (beforeDestroy != null && beforeDestroy.Length > 0) {
                            foreach (var destroy in beforeDestroy) {
                                destroy.BeforeDestroy();
                            }
                        }

                        // TODO: make the building clear itself from the building foundations it was placed on
                        Destroy(hit.transform.gameObject, 0.2f);
                        AudioManager.Instance.PlaySound("DestroyBuilding");
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                canDestroyBuilding = false;
				selectedBuildingUI.sprite = null;
				selectedBuildingText.text = "No building selected.";
			}
        }
    }

    /// <summary>
    /// allows the use to select a building
    /// </summary>
    private void SelectingBuilding()
    {
        if (!canSelectBuilding)
        {
            selectedBuilding = null;
            OnBuildingSelected?.Invoke(false);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.CompareTag("Building"))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        selectedBuilding = hit.transform.gameObject;
                        selectedBuildingData = selectedBuilding.GetComponent<Building>();
                        OnBuildingSelected?.Invoke(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                canSelectBuilding = false;
        }
    }

    /// <summary>
    /// for ui button to add a bee to the selected building
    /// </summary>
    public void AddBeeToBuilding()
    {
        if(selectedBuilding != null && selectedBuildingData != null)
        {
            BuildingData buildingData = selectedBuildingData.BuildingData;
            if (selectedBuildingData.assignedBees < buildingData.maxNumberOfWorkers)
            {
                Resource temp = ResourceManagement.Instance.GetResource(ResourceType.AssignedPop);
                if (temp != null)
                {
                    if (temp.CurrentResourceAmount < temp.ResourceCap)
                    {
                        selectedBuildingData.assignedBees++;
                        temp.ModifyAmount(1);
                    }
                }
                else
                    NoResourceFound(ResourceType.AssignedPop);
            }
        }
    }


    public void RemoveBeeFromBuilding()
    {
        if (selectedBuilding != null && selectedBuildingData != null)
        {
            if (selectedBuildingData.assignedBees > 0)
            {
                Resource temp = ResourceManagement.Instance.GetResource(ResourceType.AssignedPop);
                if (temp != null)
                {
                    selectedBuildingData.assignedBees--;
                    temp.ModifyAmount(-1);
                }
                else
                    NoResourceFound(ResourceType.AssignedPop);
            }
        }
    }

    /// <summary>
    /// goes through the resource costs of the selecetd building and applies a teirupgrade to the original cost
    /// </summary>
    public void UpgradeBuilding()
    {
        if(selectedBuildingData != null)
        {
            bool canUse = true;
            List<ResourcePurchase> temp = selectedBuildingData.BuildingData.ResourcePurchase;
            for(int i = 0; i < selectedBuildingData.BuildingData.ResourcePurchase.Count; i++)
            {
                if (selectedBuildingData.buildingTeir == 0)
                    temp[i].cost *= teir1UpgradeCost;
                else if (selectedBuildingData.buildingTeir == 1)
                    temp[i].cost *= teir2UpgradeCost;
                else if (selectedBuildingData.buildingTeir > 1)//if is max teir can't uprgrade
                    canUse = false;
                if (!ResourceManagement.Instance.CanUseResource(temp[i]))//if doesn't have the resources can't upgrade
                {
                    canUse = false;
                }
            }
            if (canUse)
            {
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

    #region Debugs
    private void NoBuildingFound(BuildingType buildingType)
    {
        Debug.LogWarning("No building of type: " + buildingType.ToString() + " :found");
    }
    private void BuildingAlreadyThere()
    {
        Debug.LogWarning("Building is already there");
		UIEventAnnounceManager.Instance.AnnounceEvent("Building already exists here.");
    }

    private void NoResourceFound(ResourceType resourceType)
    {
        Debug.LogWarning("No building of type: " + resourceType.ToString() + " :found");
    }

    private void NotEnoughResources()
    {
        Debug.LogWarning("Not Enough Resources To Make Purchase");
    }
    #endregion
}
