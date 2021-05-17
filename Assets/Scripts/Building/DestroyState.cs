using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyState : BuildingManagerState {
    public override void Enter() {
        
    }

    public override void Exit() {
        
    }

    public override void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingManager.buildingMask)) {
                if (hit.transform.CompareTag("Building")) {
                    Building building = hit.transform.gameObject.GetComponent<Building>();

                    // Check if it is a Enemy building allow its destruction in level editor
                    if (building == null) {
                        if (CurrentSceneType.IsLevelEditor()) {
                            var enemyBuilding = hit.transform.gameObject.GetComponent<EnemyBuilding>();
                            if (enemyBuilding != null) {
                                DissolveAndDestroy(hit);
                                return;
                            }
                        } else {
                            return;
                        }
                    }

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

                    DissolveAndDestroy(hit);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)  || Input.GetMouseButtonDown(1)) {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        }
    }

    private void DissolveAndDestroy(RaycastHit hit) {
        GameObject dissolver = new GameObject("dissolver", typeof(Dissolver));
        dissolver.GetComponent<Dissolver>().Setup(hit.transform.gameObject);
        Object.Destroy(hit.transform.gameObject, 0.2f);
        AudioManager.Instance.PlaySound("DestroyBuilding");
    }

    public DestroyState(BuildingManager buildingManager) : base(buildingManager) { }
}
