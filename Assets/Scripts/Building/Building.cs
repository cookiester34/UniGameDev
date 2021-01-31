using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [HideInInspector]
    public ResourceManagement resourceManagement;
    public ResourceType resourceType;
    public BuildingType buildingType;
    [SerializeField] private BuildingData buildingData;

    public BuildingData BuildingData => buildingData;

    [Header("Only if is a storage, or housing building")]
    public int resourceCapIncrease;

    [Header("Only if a production building")]
    public int resourceDrainAmount;
    [Tooltip("Cannot be 0")]
    public int resourceProductionTime;
    public int resourceProductionAmount;

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
