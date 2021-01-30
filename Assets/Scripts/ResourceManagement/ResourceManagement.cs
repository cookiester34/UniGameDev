using System.Collections.Generic;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    public List<Resource> resourceList = new List<Resource>();
    [Header("Options")]
    [Tooltip("This sets how often the resoources get updated in seconds")]
    [Range(0,30)]
    public int resourceTickTime;

    /// <summary>
    /// Singleton instance
    /// </summary>
    private static ResourceManagement _instance;
    
    /// <summary>
    /// Public access to singleton instance
    /// </summary>
    public static ResourceManagement Instance => _instance;

    /// <summary>
    /// Handles setting up of the singleton
    /// </summary>
    private void Awake() {
        if (_instance != null) {
            SecondInstance();
            Destroy(this);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        //will call the resourceTick every x seconds
        InvokeRepeating(nameof(ResourceTick), 1, resourceTickTime);
    }

    /// <summary>
    /// Given a list of resourcePurchases determines if they can be purchased
    /// </summary>
    /// <param name="resourcePurchases">The resources to attempt to purchase something with</param>
    /// <returns>True if all the resources can be purchases, false if they cannot</returns>
    public bool CanUseResources(List<ResourcePurchase> resourcePurchases) {
        bool canUse = true;
        foreach (ResourcePurchase resource in resourcePurchases) {
            canUse = CanUseResource(resource);
            if (!canUse) {
                break;
            }
        }

        return canUse;
    }

    /// <summary>
    /// Given a resourcePurchase determines if the resource can be purchased
    /// </summary>
    /// <param name="resourcePurchase">The resource to purchase from</param>
    /// <returns>True if the resource can be purchased from, else false</returns>
    public bool CanUseResource(ResourcePurchase resourcePurchase) {
        Resource matchingResource = GetResource(resourcePurchase.resourceType);
        return matchingResource.CanPurchase(resourcePurchase.cost);
    }

    /// <summary>
    /// Uses multiple resources to make a purchase if the purchase can be made
    /// </summary>
    /// <param name="resourcePurchases">The resources to purchase from</param>
    /// <returns>True if the purchase was successful, else false</returns>
    public bool UseResources(List<ResourcePurchase> resourcePurchases) {
        bool canBeUsed = CanUseResources(resourcePurchases);
        if (canBeUsed) {
            foreach (ResourcePurchase resource in resourcePurchases) {
                UseResource(resource);
            }
        }

        return canBeUsed;
    }

    /// <summary>
    /// Uses a resource to make a purchase if the purchase can be made
    /// </summary>
    /// <param name="resourcePurchase">The resource to purchase from</param>
    /// <returns>True if the purchase was successful, else false</returns>
    public bool UseResource(ResourcePurchase resourcePurchase) {
        bool canUse = CanUseResource(resourcePurchase);
        Resource matchingResource = GetResource(resourcePurchase.resourceType);

        if (matchingResource != null) {
            matchingResource.ModifyAmount(resourcePurchase.cost);
        }

        return canUse;
    }


    /// all functions below will provide a warning if the resource cannot be found

    /// <summary>
    /// to get the current resource amount, will return an int
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public int GetResourceCurrentAmount(ResourceType resourceType)
    {
        foreach (Resource i in resourceList)
        {
            if(i.resourceType == resourceType)
            {
                return i.currentResourceAmount;
            }
        }
        NoResourceFound(resourceType);
        return 0;
    }

    /// <summary>
    /// updates the current resource amount, you can provide a positive or negative value
    /// <para>can return an int value if needed</para>
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int UpdateResourceCurrentAmount(ResourceType resourceType, int updateValue)
    {
        foreach (Resource i in resourceList)
        {
            if (i.resourceType == resourceType)
            {
                i.currentResourceAmount += updateValue;
                return i.currentResourceAmount;
            }
        }
        NoResourceFound(resourceType);
        return 0;
    }

    /// <summary>
    /// can update the resource tick amount for when the tick event gets called
    /// <para>can return an int value if needed</para>
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int UpdateResourceTickAmount(ResourceType resourceType, int updateValue)
    {
        foreach (Resource i in resourceList)
        {
            if (i.resourceType == resourceType)
            {
                i.ResourceTickDrainAmount += updateValue;
                return i.ResourceTickDrainAmount;
            }
        }
        NoResourceFound(resourceType);
        return 0;
    }

    /// <summary>
    /// Returns a Dictionary of all current resource values
    /// <para>Key = resource name, value = current resource amount</para>
    /// </summary>
    /// <returns></returns>
    public Dictionary<ResourceType, int> GetAllCurrentResourceValues()
    {
        Dictionary<ResourceType, int> temp = new Dictionary<ResourceType, int>();
        foreach(Resource i in resourceList)
        {
            temp.Add(i.resourceType, i.currentResourceAmount);
        }
        return temp;
    }

    /// <summary>
    /// this is the event that ticks every resourceTickTime to adjust all resource counts
    /// </summary>
    private void ResourceTick()
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceList[i].currentResourceAmount -= resourceList[i].ResourceTickDrainAmount;
        }
    }

    /// <summary>
    /// Using a resourceType finds the correct resource, may return null if that resource is not stored
    /// </summary>
    /// <param name="resourceType">The resource type to find</param>
    /// <returns>The matching resource, or null if it was not found</returns>
    private Resource GetResource(ResourceType resourceType) {
        Resource matchingResource = 
            resourceList.Find(resource => resource.resourceType == resourceType);

        if (matchingResource == null) {
            NoResourceFound(resourceType);
        }

        return matchingResource;
    }


    #region DebugCalls
    private void NoResourceFound(ResourceType resourceName) 
    {
        Debug.LogWarning("The resource: " + resourceName.ToString() + " :was not found");
    }

    private void SecondInstance() {
        Debug.LogWarning("Attempted to create 2nd instance of Resource Manager");
    }
    #endregion
}
