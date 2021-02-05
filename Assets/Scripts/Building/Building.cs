using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : MonoBehaviour
{
    [NonSerialized] public ResourceManagement resourceManagement;
    public ResourceType resourceType;
    public BuildingType buildingType;
    [SerializeField] private BuildingData buildingData;
    [HideInInspector]
    public int buildingTeir = 0;
    public GameObject buildingTeir1;
    public GameObject buildingTeir2;
    public GameObject buildingTeir3;

    public BuildingData BuildingData => buildingData;

    [Header("Only if is a storage, or housing building")]
    public int resourceCapIncrease;

    [Header("Only if a production building")]
    public int resourceDrainAmount;
    [Tooltip("Cannot be 0")]
    [Range(1, 100)]//just to stop it being set to 0;
    public int resourceProductionTime;
    public int resourceProductionAmount;

    private void Start()
    {
        if(buildingTeir == 0)
        {
            buildingTeir1.SetActive(true);
            buildingTeir2.SetActive(false);
            buildingTeir3.SetActive(false);
        }
        else if (buildingTeir == 1)
        {
            buildingTeir1.SetActive(false);
            buildingTeir2.SetActive(true);
            buildingTeir3.SetActive(false);
        }
        else if (buildingTeir == 2)
        {
            buildingTeir1.SetActive(false);
            buildingTeir2.SetActive(false);
            buildingTeir3.SetActive(true);
        }
    }

    /// <summary>
    /// Sets up how much the building consumes and starts the production of the building
    /// </summary>
    public void SetupBuilding()
    {
        if(buildingType == BuildingType.Storage || buildingType == BuildingType.Housing)
        {
            resourceManagement.UpdateResourceCapAmount(resourceType, resourceCapIncrease);
        }
        else
        {
            resourceManagement.UpdateResourceTickAmount(resourceType, resourceDrainAmount);
            InvokeRepeating(nameof(Production), 1, resourceProductionTime);
        }
    }

    /// <summary>
    /// if there is enough of the resource cost it will produce
    /// </summary>
    private void Production()
    {
        if(resourceManagement.GetResourceCurrentAmount(resourceType) >= resourceDrainAmount)
            resourceManagement.UpdateResourceCurrentAmount(resourceType, resourceProductionAmount);
    }
}
