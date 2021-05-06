﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FogOfWarBounds : MonoBehaviour
{
    public static FogOfWarBounds instance;
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

        var tempList = buildBounds;
        foreach(CapsuleCollider i in tempList)
        {
            if(i == null)
            {
                buildBounds.Remove(i);
                continue;
            }
            if (i.bounds.Contains(hit.point))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public bool IsInFog(Vector3 position) {
        bool inFog = true;

        if (CurrentSceneType.SceneType == SceneType.LevelEditor) {
            return false;
        }

        foreach(CapsuleCollider i in buildBounds) {
            if (i != null) {
                if (i.bounds.Contains(position)) {
                    inFog = false;
                    break;
                }
            }
        }

        return inFog;
    }

    /// <summary>
    /// Pass wasp position to check if it is visible
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsVisible(Vector3 position)
    {
        if (CurrentSceneType.SceneType == SceneType.LevelEditor)
        {
            return true;
        }

        var tempList = buildBounds;
        foreach (CapsuleCollider i in tempList.ToList())
        {
            if (i == null)
            {
                buildBounds.Remove(i);
                continue;
            }
            if (i.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }
}
