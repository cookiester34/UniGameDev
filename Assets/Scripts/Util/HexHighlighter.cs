using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// Quick attempt at highlighting hexes near mouse when in build mode
/// </summary>
public class HexHighlighter : MonoBehaviour {
    [SerializeField] private LayerMask terrain;
    [SerializeField] private LayerMask grid;
    private Collider[] colliders = new Collider[100];
    private int size = 0;

    void FixedUpdate() {
        if (BuildingManager.Instance.CurrentState is BuildState) {
            for (int i = 0; i < size; i++) {
                colliders[i].gameObject.GetComponentInParent<BuildingFoundation>().UpdateVisibleColour(0f, 1f);
            }

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var hit, float.MaxValue, terrain)) {
                Vector3 mouseTerrainPos = hit.point;
                size = Physics.OverlapSphereNonAlloc(mouseTerrainPos, 5f, colliders, grid);
                if (size > 0) {
                    for (int i = 0; i < size; i++) {
                        float sqrMagnitude = (mouseTerrainPos - colliders[i].transform.position).sqrMagnitude;
                        colliders[i].gameObject.GetComponentInParent<BuildingFoundation>()
                            .UpdateVisibleColour(25f - sqrMagnitude, 20f);
                    }
                }

            }
        }
    }
}
