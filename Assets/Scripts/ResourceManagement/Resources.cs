using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/New Resource")]
public class Resources : ScriptableObject
{
    [Header("Resource Options")]
    public string resourceName;
    public int resourceStartingAmount;
    public int ResourceTickDrainAmount;
    //[HideInInspector] decided to show for now while debugging
    public int currentResourceAmount;

    void start()
    {
        currentResourceAmount = resourceStartingAmount;
    }
}
