using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyState : BuildingManagerState {
    public override void Enter() {
        buildingManager.selectedBuildingText.text = "Destroys buildings on click";
        buildingManager.selectedBuildingUI.sprite = buildingManager.DestroyUISprite;
    }

    public override void Exit() {
        buildingManager.selectedBuildingText.text = "No building selected.";
        buildingManager.selectedBuildingUI.sprite = null;
    }

    public override void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingManager.buildingMask)) {
                if (hit.transform.CompareTag("Building")) {
                    Building destroyedBuilding = hit.transform.GetComponent<Building>();
                    buildingManager.numBuildingTypes[(int) destroyedBuilding.BuildingData.BuildingType]--;
                    var beforeDestroy = hit.collider.GetComponents<IBeforeDestroy>();
                    if (beforeDestroy != null && beforeDestroy.Length > 0) {
                        foreach (var destroy in beforeDestroy) {
                            destroy.BeforeDestroy();
                        }
                    }

                    Object.Destroy(hit.transform.gameObject, 0.2f);
                    AudioManager.Instance.PlaySound("DestroyBuilding");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        }
    }

    public DestroyState(BuildingManager buildingManager) : base(buildingManager) { }
}
