using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    BuildingData currentBuilding;
    GameObject tempBuilding;
    private bool canPlaceBuilding;
    private bool canDestroyBuilding;

    private bool canSelectBuilding;
    private GameObject selectedBuilding;
    private Building selectedBuildingData;

    public ResourceManagement resourceManagement;
    public LayerMask mask;
    //have to make sure building objects are in the same order as the enum
    public List<BuildingData> buildings = new List<BuildingData>();

    private static BuildingManager _instance;
    public static BuildingManager Instance => _instance;

    public GameObject upgradeButton;

    [Header("Options")]
    [Range(2, 5)]
    public int teir1UpgradeCost;
    [Range(2, 5)]
    public int teir2UpgradeCost;


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
    }

    /// <summary>
    /// This is a function for UI, the button needs to have a building pass script on it determining what building it will place
    /// </summary>
    /// <param name="buildingType"></param>
    public void PlaceBuilding(BuildingData buildingData) {
        if (buildingData == null) {
            canPlaceBuilding = false;
            NoBuildingFound(buildingData.BuildingType);
        } else {
            canPlaceBuilding = resourceManagement.CanUseResources(buildingData.ResourcePurchase);

            if (canPlaceBuilding) {
                currentBuilding = buildingData;
                tempBuilding = Instantiate(currentBuilding.BuildingType.GetPrefab(),
                    new Vector3(0, 0, 0), Quaternion.identity);
                tempBuilding.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void Update()
    {
        PlacingBuilding();
        DestroyingBuildings();
        SelectingBuilding();

        //should always be able to select buildings, unless placeing or destroying them
        if (!canDestroyBuilding && !canPlaceBuilding) 
            canSelectBuilding = true;
        else
            canSelectBuilding = false;

        //toggles the button for upgrading buildings
        if (selectedBuildingData != null)
            upgradeButton.SetActive(true);
        else
            upgradeButton.SetActive(false);
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
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
        } else {
            canPlaceBuilding = false;
            tempBuilding.transform.position = position;
            Building tempManager = tempBuilding.GetComponent<Building>();
            resourceManagement.UseResources(currentBuilding.ResourcePurchase);
            tempManager.resourceManagement = resourceManagement;
            tempBuilding.GetComponent<Collider>().enabled = true;
            tempManager.SetupBuilding();
        }
    }
    /// <summary>
    /// cancles the placement of the chosen building
    /// </summary>
    private void CanclePlacingBuilding()
    {
        canPlaceBuilding = false;
        Destroy(tempBuilding);
    }

    public void DestroyBuilding()
    {
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
                    if (hit.transform.CompareTag("Building"))
                    {
                        //When the building is removed remove all it's costs
                        if (hit.transform.GetComponent<Building>().buildingType == BuildingType.Storage || hit.transform.GetComponent<Building>().buildingType == BuildingType.Housing)
                        {
                            resourceManagement.UpdateResourceCapAmount(hit.transform.GetComponent<Building>().resourceType, -hit.transform.GetComponent<Building>().resourceCapIncrease);
                        }
                        else
                        {
                            resourceManagement.UpdateResourceTickAmount(hit.transform.GetComponent<Building>().resourceType, -hit.transform.GetComponent<Building>().resourceDrainAmount);
                        }
                        Destroy(hit.transform.gameObject, 0.2f);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                canDestroyBuilding = false;
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
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                canSelectBuilding = false;
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
                if (!resourceManagement.CanUseResource(temp[i]))//if doesn't have the resources can't upgrade
                {
                    canUse = false;
                }
            }
            if (canUse)
            {
                selectedBuildingData.buildingTeir++;
                resourceManagement.UseResources(temp);
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
    }
    #endregion
}
