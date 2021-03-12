using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableUIOnBuildingSelect : MonoBehaviour 
{

    public Text buildingsAssignedBees;
    public Text buildingName;
    public Text buildingResource;

    List<ResourceSupplier> resources = new List<ResourceSupplier>();
    List<ResourceStorage> storage = new List<ResourceStorage>();

    public Button upgradeBuilding;
    public Button addBee;
    public Button removeBee;

    public GameObject toolTipObject;
    public Text toolTip;

    private Building selectedBuilding;
    public GameObject displayBonus;
    public Text bonusText;

    public List<Renderer> points = new List<Renderer>();
    private float minX, maxX, minY, maxY;

    Vector2 bottomCorner;
    Vector2 topCorner;

    Vector3 pos;
    Vector3 origScale;
    public LayerMask terrainMask;
    float uiPosEdit = 5;

    private void Awake() 
    {
        displayBonus.SetActive(false);
        gameObject.SetActive(false);
        BuildingManager.Instance.OnBuildingSelected += DataChanged;
        origScale = transform.localScale;
    }

    private void Update()
    {
        if (Vector3.Distance(pos, Camera.main.transform.position) > 17)
        {
            transform.position = new Vector3(Camera.main.transform.position.x + 1, Camera.main.transform.position.y - 1, Camera.main.transform.position.z + 1);
            transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
        }
        else
        {
            transform.position = pos;
            transform.localScale = origScale;
        }

        //note sure if we have controls grouped somewhere or not
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator waitToUpdateUI(Building building)
    {
        yield return new WaitForSeconds(0.1f);
        UpdateAssignedBeesUI(building);
    }

    private void UpdateAssignedBeesUI(Building building) 
    {
        toolTip.text = "";
        toolTipObject.SetActive(false);
        buildingResource.text = "";
        buildingsAssignedBees.text = "Assigned Bees: " + building.numAssignedBees + " / " + building.BuildingData.maxNumberOfWorkers + "\n" + "Unassigned Bees: " + 
            (ResourceManagement.Instance.GetResource(ResourceType.Population).CurrentResourceAmount - (int)ResourceManagement.Instance.GetResource(ResourceType.AssignedPop).CurrentResourceAmount);
        buildingName.text = building.BuildingType.ToString();
        foreach (ResourceSupplier i in resources)
        {
            buildingResource.text += "Supplying " + i.Resource.name + ": " + i.actualProductionAmount + "\n";
        }
        foreach (ResourceStorage i in storage)
        {
            buildingResource.text += "Max " + i.Resource.name + " storage: " + i.GetStorage() + "\n";
        }

        #region UI Cases
        if (building.buildingTeir >= 2 || building.BuildingType == BuildingType.QueenBee)
        {
            upgradeBuilding.interactable = false;
            toolTip.text += "This Building cannot be upgraded \n \n";
            toolTipObject.SetActive(true);
        }
        else
        {
            upgradeBuilding.interactable = true;
        }

        if(building.numAssignedBees >= building.BuildingData.maxNumberOfWorkers)
        {
            addBee.interactable = false;
            toolTip.text += "This building has the max assigned bees \n \n";
            toolTipObject.SetActive(true);
        }
        else
        {
            addBee.interactable = true;
        }

        if(building.numAssignedBees <= 0)
        {
            removeBee.interactable = false;
            toolTip.text += "This building has no Bees Assigned";
            toolTipObject.SetActive(true);
        }
        else
        {
            removeBee.interactable = true;
        }

        #endregion
    }

    private void DataChanged(Building building) {
        if (building != null) 
        {
            selectedBuilding = building;
            Camera camera = Camera.main;
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
            
            GameObject buildingObject = building.GetActiveBuilding();
            if (buildingObject != null)
            {
                pos = building.gameObject.transform.position + new Vector3(0, buildingObject.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y * 4f);
            }
            else
                pos = building.gameObject.transform.position + new Vector3(0, 5);

            transform.position = pos;
            gameObject.SetActive(true);

            resources.Clear();
            resources = new List<ResourceSupplier>(building.gameObject.GetComponents<ResourceSupplier>());
            storage.Clear();
            storage = new List<ResourceStorage>(building.gameObject.GetComponents<ResourceStorage>());

            //UpdateAssignedBeesUI(building);
            StartCoroutine(waitToUpdateUI(building));
        } 
        else 
        {
            gameObject.SetActive(false);
        }
    }

    public void UpgradeBuilding()
    {
        BuildingManager.Instance.UpgradeBuilding();
    }

    public void OnHoverUpgrade()
    {
        displayBonus.SetActive(true);
        switch (selectedBuilding.GetBuildingTeir()) 
        {
            case 1:
                bonusText.text = "Upgrading building will result in double production";
                break;
            case 2:
                bonusText.text = "Upgrading building will behave like 3 buildings";
                break;
            case 3:
                displayBonus.SetActive(false);
                break;
        }
    }

    public void OnHoverExitUpgrade()
    {
        displayBonus.SetActive(false);
        bonusText.text = "";
    }

    public void AddBee()
    {
        BuildingManager.Instance.AddBeeToBuilding();
    }

    public void OnHoverAddBee()
    {
        displayBonus.SetActive(true);
        bonusText.text = "Effect of adding bee " + (100f / selectedBuilding.BuildingData.maxNumberOfWorkers) + "% increase to production";
    }

    public void OnHoverExitAddBee()
    {
        displayBonus.SetActive(false);
        bonusText.text = "";
    }

    public void RemoveBeeFromBuilding()
    {
        BuildingManager.Instance.RemoveBeeFromBuilding();
    }

    public void OnHoverRemoveBee()
    {
        displayBonus.SetActive(true);
        bonusText.text = "Effect of removing bee " + (100f / selectedBuilding.BuildingData.maxNumberOfWorkers) + "% Decrease to production";
    }

    public void OnHoverExitRemoveBee()
    {
        displayBonus.SetActive(false);
        bonusText.text = "";
    }
}
