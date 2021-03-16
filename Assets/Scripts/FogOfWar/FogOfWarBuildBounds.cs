﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FogOfWarBuildBounds : MonoBehaviour
{
    public static FogOfWarBuildBounds instance;
    private void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public LayerMask mask;
    public List<CapsuleCollider> buildBounds = new List<CapsuleCollider>();

    /// <summary>
    /// provide the Starting position or the ray, the direction the shoot it and it's max length
    /// it will return the Gameobject it hits
    /// </summary>
    /// <param name="raycastPosition"></param>
    /// <param name="rayCastDirection"></param>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    public GameObject IsValidRaycastGameObject(Vector3 raycastPosition, Vector3 rayCastDirection, float maxDistance)
    {
        Ray ray = new Ray(raycastPosition, rayCastDirection);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance, mask);

        foreach(CapsuleCollider i in buildBounds)
        {
            if (i.bounds.Contains(hit.point))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
