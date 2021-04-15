using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
    public bool flip = false;
    void FixedUpdate()
    {
        if (Camera.main != null)
        {
            if (!flip)
                transform.LookAt(Camera.main.transform, -Vector3.up);
            else
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
