using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    public List<Resources> resourceList = new List<Resources>();
    [Header("Options")]
    [Range(0,10)]
    public float resourceTickTime;

    private void Start()
    {
        InvokeRepeating("ResourceTick", 1, resourceTickTime);
    }

    //to get the current resource amount
    public int GetResourceCurrentAmount(string resourceName)
    {
        for(int i=0; i < resourceList.Count; i++)
        {
            if(resourceList[i].resourceName == resourceName)
            {
                return resourceList[i].currentResourceAmount;
            }
        }
        NoResourceFound(resourceName);
        return 0;
    }

    //to update the current resource amount, can add and take
    public int UpdateResourceAmount(string resourceName, int updateValue)
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            if (resourceList[i].resourceName == resourceName)
            {
                resourceList[i].currentResourceAmount += updateValue;
                return resourceList[i].currentResourceAmount;
            }
        }
        NoResourceFound(resourceName);
        return 0;
    }

    public void ResourceTick()
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceList[i].currentResourceAmount -= resourceList[i].ResourceTickDrainAmount;
        }
    }


    #region DebugCalls
    void NoResourceFound(string resourceName)
    {
        Debug.LogWarning("The resource: " + resourceName + " :was not found");
    }
    #endregion
}
