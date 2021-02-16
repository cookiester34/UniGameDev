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
    
    /// <summary>
    /// Event to be fired whenever the value of the resource changes
    /// </summary>
    public event CurrentValueChanged OnCurrentValueChanged;
    public event CurrentValueChanged OnCapChanged;
    
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

    public float CurrentResourceAmount => currentResourceAmount;

    public float ResourceTickAmount => resourceTickAmount;

    public int ResourceCap => resourceCap;

    public int ResourceStartingAmount => resourceStartingAmount;

    public float StartingTickAmount => startingTickAmount;

    public int StartingCap => startingCap;

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
}
