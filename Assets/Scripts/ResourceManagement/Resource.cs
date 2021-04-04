using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resource : ScriptableObject {
    /// <summary>
    /// Delegate for the current value change event
    /// </summary>
    public delegate void CurrentValueChanged(float currentValue);
    public delegate void CapReached();
    
    /// <summary>
    /// Event to be fired whenever the value of the resource changes
    /// </summary>
    public event CurrentValueChanged OnCurrentValueChanged;
    public event CurrentValueChanged OnCapChanged;
    public event CapReached OnCapReached;
    
    [Header("Resource Options")]
    public ResourceType resourceType;

    /// <summary>
    /// The amount that the current should change by each tick, a positive value increases the amount, negative reduces
    /// </summary>
    [SerializeField] private float resourceTickAmount;
    [SerializeField] private int resourceStartingAmount;

    [SerializeField] private float currentResourceAmount;
    [SerializeField] private float startingTickAmount;

    [SerializeField] private int resourceCap;
    [SerializeField] private int startingCap;

    [SerializeField] private int resourceLowThreshold;

    public float CurrentResourceAmount => currentResourceAmount;

    public float ResourceTickAmount => resourceTickAmount;

    public int ResourceCap => resourceCap;

    public int ResourceStartingAmount => resourceStartingAmount;

    public float StartingTickAmount => startingTickAmount;

    public int StartingCap => startingCap;

    public int ResourceLowThreshold => resourceLowThreshold;

    void OnEnable() {
        //sets the initial value of this resource
        currentResourceAmount = resourceStartingAmount;
        resourceTickAmount = startingTickAmount;
        resourceCap = startingCap;
    }

    /// <summary>
    /// Copies another resource into this resource
    /// </summary>
    /// <param name="resource">Resource to copy into this one</param>
    public void CopySavedResource(SavedResource resource) {
        resourceCap = resource.Cap;
        resourceType = resource.Type;
        resourceStartingAmount = resource.StartingAmount;
        resourceTickAmount = resource.TickDrainAmount;
        currentResourceAmount = resource.CurrentResourceAmount;
        OnCurrentValueChanged?.Invoke(currentResourceAmount);
        OnCapChanged?.Invoke(resourceCap);
    }

    public void ModifyAmount(float value) {
        if (currentResourceAmount < resourceCap || value < 0) {
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

    /// <summary>
    /// Modifies the resource cap by the amount. If the new cap is lower than the current amount of resource than will
    /// additionally lower the amount of resource unless overflow is allowed
    /// </summary>
    /// <param name="amount">The amount to modify by</param>
    /// <param name="allowOverflow">Optional, used to allow the current amount to be greater than the cap,
    /// intended to be used where it will momentarily go down but then come back up higher</param>
    public void ModifyCap(int amount, bool allowOverflow = false) {
        resourceCap += amount;

        if (!allowOverflow && currentResourceAmount > resourceCap) {
            ModifyAmount(resourceCap - currentResourceAmount);
        }

        OnCapChanged?.Invoke(resourceCap);
    }

    public void OverrideCap(int amount)
    {
        resourceCap = amount;
        OnCapChanged?.Invoke(resourceCap);
    }

    /// <summary>
    /// Modifies the tick drain amount
    /// </summary>
    /// <param name="amount">The amount to modify it by</param>
    /// <param name="time">Over how long the amount should take effect</param>
    public void ModifyTickDrain(float amount, float time = 1) {
        resourceTickAmount += amount / time;
    }

    public bool ResourceCapReached()
    {
        if (currentResourceAmount >= resourceCap)
        {
            currentResourceAmount = Mathf.Clamp(currentResourceAmount, 0, resourceCap);
            OnCapReached?.Invoke();
            return true;
        }
        else
            return false;
    }

    public bool tickDrainAmountCap()
    {
        if (resourceTickAmount < 0)
        {
            resourceTickAmount = 0;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Gets the floor of the current value
    /// For use in the UI (We can't have half a resource)
    /// </summary>
    public int GetFloorCurrentAmount()
    {
        return Mathf.FloorToInt(currentResourceAmount);
    }

    /// <summary>
    /// Gets the current resource tick amount
    /// use for UI
    /// </summary>
    /// <returns></returns>
    public float GetResourceTickAmount()
    {
        return resourceTickAmount;
    }
}
