﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDynamic : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface[] navMeshSurfaces;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < navMeshSurfaces.Length; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }

   
}
