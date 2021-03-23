using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtil {
    public static bool Raycast(Ray ray, out RaycastHit raycastHit, float distance, LayerMask mask, bool fogAllowed) {
        bool hit = false;
        if (Physics.Raycast(ray, out raycastHit, distance, mask)) {
            hit = fogAllowed || !FogOfWarBounds.instance.IsInFog(raycastHit.point);
        }

        return hit;
    }
}
