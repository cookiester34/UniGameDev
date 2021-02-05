using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : MonoBehaviour
{
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
        if (buildingTeir1 != null) {
            buildingTeir1.SetActive(buildingTeir == 0);
        }

        if (buildingTeir2 != null) {
            buildingTeir2.SetActive(buildingTeir == 1);
        }

        if (buildingTeir3 != null) {
            buildingTeir3.SetActive(buildingTeir == 2);
        }
    }

    /// <summary>
    /// Sets up how much the building consumes and starts the production of the building
    /// </summary>
    public void SetupBuilding()
    {
        if(buildingType == BuildingType.Storage || buildingType == BuildingType.Housing)
        {
            ResourceManagement.Instance.UpdateResourceCapAmount(resourceType, resourceCapIncrease);
        }
        else
        {
            ResourceManagement.Instance.UpdateResourceTickAmount(resourceType, resourceDrainAmount);
            InvokeRepeating(nameof(Production), 1, resourceProductionTime);
        }
    }

    /// <summary>
    /// if there is enough of the resource cost it will produce
    /// </summary>
    private void Production()
    {
        if(ResourceManagement.Instance.GetResourceCurrentAmount(resourceType) >= resourceDrainAmount)
            ResourceManagement.Instance.UpdateResourceCurrentAmount(resourceType, resourceProductionAmount);
    }
}
