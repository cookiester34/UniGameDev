using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class to save a transform
/// </summary>
[Serializable]
public class SavedTransform {
    public Vector3 Position;
    public Quaternion Rotation;

    public SavedTransform(Vector3 position, Quaternion rotation) {
        Position = position;
        Rotation = rotation;
    }
    
    public SavedTransform(Transform transform) {
        Position = transform.position;
        Rotation = transform.rotation;
    }
}
