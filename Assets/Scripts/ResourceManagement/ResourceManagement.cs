using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    public List<Resources> resourceList = new List<Resources>();
    [Header("Options")]
    [Tooltip("This sets how often the resoources get updated in seconds")]
    [Range(0,30)]
    public int resourceTickTime;

    private void Start()
    {
        //will call the resourceTick every x seconds
        InvokeRepeating("ResourceTick", 1, resourceTickTime);
    }


    /// all functions below will provide a warning if the resource cannot be found

    /// <summary>
    /// to get the current resource amount, will return an int
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public int GetResourceCurrentAmount(string resourceName)
    {
        foreach (Resources i in resourceList)
        {
            if(i.resourceName == resourceName)
            {
                return i.currentResourceAmount;
            }
        }
        NoResourceFound(resourceName);
        return 0;
    }

    /// <summary>
    /// updates the current resource amount, you can provide a positive or negative value
    /// <para>can return an int value if needed</para>
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int UpdateResourceCurrentAmount(string resourceName, int updateValue)
    {
        foreach (Resources i in resourceList)
        {
            if (i.resourceName == resourceName)
            {
                i.currentResourceAmount += updateValue;
                return i.currentResourceAmount;
            }
        }
        NoResourceFound(resourceName);
        return 0;
    }

    /// <summary>
    /// can update the resource tick amount for when the tick event gets called
    /// <para>can return an int value if needed</para>
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int UpdateResourceTickAmount(string resourceName, int updateValue)
    {
        foreach (Resources i in resourceList)
        {
            if (i.resourceName == resourceName)
            {
                i.ResourceTickDrainAmount += updateValue;
                return i.ResourceTickDrainAmount;
            }
        }
        NoResourceFound(resourceName);
        return 0;
    }

    /// <summary>
    /// Returns a Dictionary of all current resource values
    /// <para>Key = resource name, value = current resource amount</para>
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, int> GetAllCurrentResourceValues()
    {
        Dictionary<string, int> temp = new Dictionary<string, int>();
        foreach(Resources i in resourceList)
        {
            temp.Add(i.resourceName, i.currentResourceAmount);
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


    #region DebugCalls
    private void NoResourceFound(string resourceName)
    {
        Debug.LogWarning("The resource: " + resourceName + " :was not found");
    }
    #endregion
}
