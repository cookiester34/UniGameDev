using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resource : ScriptableObject {
    /// <summary>
    /// Delegate for the current value change event
    /// </summary>
    public delegate void CurrentValueChanged();
    
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

    public void ModifyAmount(int value) {
        currentResourceAmount -= value;
        OnCurrentValueChanged?.Invoke();
    }

    public bool CanPurchase(int value) {
        return (currentResourceAmount - value >= 0);
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
