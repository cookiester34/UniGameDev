using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToListAlterTower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NavMeshManagement.Instance.towersAffectingNavmesh.Add(this.transform);
    }
}
