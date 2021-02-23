using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    void FixedUpdate() {
        if (Camera.main != null) {
            transform.LookAt(Camera.main.transform , -Vector3.up);
        }
    }
}
