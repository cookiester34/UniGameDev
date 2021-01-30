using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    GameObject currentBuilding;
    GameObject tempBuilding;
    private bool canPlaceBuilding;
    private bool canDestroyBuilding;

    public ResourceManagement resourceManagement;
    public LayerMask mask;
    //have to make sure building objects are in the same order as the enum
    public List<GameObject> buildings = new List<GameObject>();

    /// <summary>
    /// This is a function for UI, the button needs to have a building pass script on it determining what building it will place
    /// </summary>
    /// <param name="building"></param>
    public void PlaceBuilding(BuildingPass building)
    {
        if (buildings[(int)building.buildingType] != null)
        {
            Building temp = buildings[(int)building.buildingType].GetComponent<Building>();
            if (resourceManagement.CanPurchase(temp.resourceCost, temp.buildingCost))
            {
                currentBuilding = buildings[(int)building.buildingType];
                tempBuilding = Instantiate(currentBuilding, new Vector3(0, 0, 0), Quaternion.identity);
                tempBuilding.GetComponent<SphereCollider>().enabled = false;
                canPlaceBuilding = true; 
            }
            else
            {
                canPlaceBuilding = false;
            }
        }
        else
        {
            canPlaceBuilding = false;
            NoBuildingFound(building.buildingType);
        }
    }

    private void Update()
    {
        PlacingBuilding();
        DestroyingBuildings();
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
            if (Physics.Raycast(ray, out hit, mask))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    tempBuilding.transform.position = hit.collider.GetComponent<Renderer>().bounds.center;

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                        PlaceBuilding(hit.collider.GetComponent<Renderer>().bounds.center);
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
    private void PlaceBuilding(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + new Vector3(0, 10, 0), Vector3.down, out hit, 20, mask))
        {
            if (hit.transform.CompareTag("Building"))
            {
                CanclePlacingBuilding();
                BuildingAlreadyThere();
            }
            else
            {
                canPlaceBuilding = false;
                tempBuilding.transform.position = position;
                Building tempManager = tempBuilding.AddComponent<Building>();
                tempManager.resourceManagement = resourceManagement;
                tempBuilding.GetComponent<SphereCollider>().enabled = true;
                tempManager.SetupBuilding();
            }
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

    private void DestroyingBuildings()
    {
        if (canDestroyBuilding)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, mask))
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
