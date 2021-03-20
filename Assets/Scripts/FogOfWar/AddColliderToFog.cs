

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddColliderToFog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //add the colliders to the fog bounds
        FogOfWarBounds.instance.buildBounds.Add(GetComponent<CapsuleCollider>());
    }
}
