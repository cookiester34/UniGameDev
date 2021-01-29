using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resource : ScriptableObject
{
    [Header("Resource Options")]
    public ResourceType resourceType;
    public int resourceStartingAmount;
    //[HideInInspector] decided to show for now while debugging
    public int ResourceTickDrainAmount;
    //[HideInInspector] decided to show for now while debugging
    public int currentResourceAmount;

    void OnEnable()
    {
        //sets the initial value of this resource
        currentResourceAmount = resourceStartingAmount;
    }

    public void ModifyAmount(int value) {
        currentResourceAmount -= value;
    }

    public bool CanPurchase(int value) {
        return (currentResourceAmount - value > 0);
    }
    
}
