using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyState : BuildingManagerState {
    public override void Enter() {
        buildingManager.selectedBuildingText.text = "Destroys buildings on click";
        buildingManager.selectedBuildingUI.sprite = buildingManager.DestroyUISprite;
        buildingManager.SetUIImage(true);
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
                    Building building = hit.transform.gameObject.GetComponent<Building>();
                    if (CurrentSceneType.SceneType == SceneType.GameLevel && building.BuildingType == BuildingType.QueenBee) {
                        UIEventAnnounceManager.Instance.AnnounceEvent("Cannot destroy the queen in a level", AnnounceEventType.Tutorial);
                        return;
                    }
                    
                    ResourceManagement.Instance.AddResources(building.GetRefundAmount());
                    var beforeDestroy = hit.collider.GetComponents<IBeforeDestroy>();
                    if (beforeDestroy != null && beforeDestroy.Length > 0) {
                        foreach (var destroy in beforeDestroy) {
                            destroy.BeforeDestroy();
                        }
                    }

                    GameObject dissolver = new GameObject("dissolver", typeof(Dissolver));
                    dissolver.GetComponent<Dissolver>().Setup(hit.transform.gameObject);
                    Object.Destroy(hit.transform.gameObject, 0.2f);
                    AudioManager.Instance.PlaySound("DestroyBuilding");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)  || Input.GetMouseButtonDown(1)) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        }
    }

    public DestroyState(BuildingManager buildingManager) : base(buildingManager) { }
}
