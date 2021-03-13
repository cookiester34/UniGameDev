using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://forum.unity.com/threads/c-performance-tips.533831/ Vector3.Distance not so fast, hopefully these methods
/// will help give us some extra performance
/// </summary>
public static class FastMath {
    public static float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector;
        float distanceSquared;
 
        vector.x = a.x - b.x;
        vector.y = a.y - b.y;
        vector.z = a.z - b.z;
 
        distanceSquared = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
 
        return (float)System.Math.Sqrt(distanceSquared);
    }
 
    public static float SqrDistance(Vector3 a, Vector3 b) {
        float x = a.x - b.x;
        float y = a.y - b.y;
        float z = a.z - b.z;
        return x * x + y * y + z * z;
    }
}
