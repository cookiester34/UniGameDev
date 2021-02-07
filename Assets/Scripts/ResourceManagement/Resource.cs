using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resource : ScriptableObject {
    /// <summary>
    /// Delegate for the current value change event
    /// </summary>
    public delegate void CurrentValueChanged(int currentValue);
    
    /// <summary>
    /// Event to be fired whenever the value of the resource changes
    /// </summary>
    public event CurrentValueChanged OnCurrentValueChanged;
    
    [Header("Resource Options")]
    public ResourceType resourceType;
    public int resourceStartingAmount;
    //[HideInInspector] decided to show for now while debugging
    public int resourceTickDrainAmount;
    //[HideInInspector] decided to show for now while debugging
    public int currentResourceAmount;
    //[HideInInspector] decided to show for now while debugging
    public int resourceCap;

    void OnEnable() {
        //sets the initial value of this resource
        currentResourceAmount = resourceStartingAmount;
    }

    /// <summary>
    /// Copies another resource into this resource
    /// </summary>
    /// <param name="resource">Resource to copy into this one</param>
    public void CopySavedResource(SavedResource resource) {
        resourceCap = resource.Cap;
        resourceType = resource.Type;
        resourceStartingAmount = resource.StartingAmount;
        resourceTickDrainAmount = resource.TickDrainAmount;
        currentResourceAmount = resource.CurrentResourceAmount;
        OnCurrentValueChanged?.Invoke(currentResourceAmount);
    }

    public void ModifyAmount(int value) {
        if (currentResourceAmount < resourceCap) {
            currentResourceAmount += value;
            if (currentResourceAmount > resourceCap) {
                currentResourceAmount = resourceCap;
            }

            OnCurrentValueChanged?.Invoke(currentResourceAmount);
        }
    }

    public bool CanPurchase(int value) {
        return (currentResourceAmount - value >= 0);
    }

    public void ModifyCap(int amount) {
        resourceCap += amount;
    }

    public bool ResourceCapReached()
    {
        if (currentResourceAmount >= resourceCap)
        {
            currentResourceAmount = Mathf.Clamp(currentResourceAmount, 0, resourceCap);
            return true;
        }
        else
            return false;
    }

    public bool tickDrainAmountCap()
    {
        if (resourceTickDrainAmount < 0)
        {
            resourceTickDrainAmount = 0;
            return true;
        }
        else
            return false;
    }
}
