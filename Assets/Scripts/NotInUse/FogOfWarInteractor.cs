using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarInteractor : MonoBehaviour
{
    [SerializeField]
    float radius;

    private void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }
}
