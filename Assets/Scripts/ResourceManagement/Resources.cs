using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resources : ScriptableObject
{
    [Header("Resource Options")]
    public ResourceType resourceType;
    public int resourceStartingAmount;
    //[HideInInspector] decided to show for now while debugging
    public int ResourceTickDrainAmount;
    //[HideInInspector] decided to show for now while debugging
    public int currentResourceAmount;

    void start()
    {
        //sets the initial value of this resource
        currentResourceAmount = resourceStartingAmount;
    }
}
