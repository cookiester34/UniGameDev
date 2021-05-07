using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairState : BuildingManagerState
{
    public override void Enter()
    {
        buildingManager.selectedBuildingText.text = "Repair buildings on click";
        buildingManager.selectedBuildingUI.sprite = buildingManager.DestroyUISprite;
        buildingManager.SetUIImage(true);
    }

    public override void Exit()
    {
        buildingManager.selectedBuildingText.text = "No building selected.";
        buildingManager.selectedBuildingUI.sprite = null;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingManager.buildingMask))
            {
                if (hit.transform.CompareTag("Building"))
                {
                    ResourcePurchase resourcePurchase = new ResourcePurchase(ResourceType.Pollen, 10);
                    Health health = hit.collider.gameObject.GetComponent<Health>();
                    health.HealForCost(50, resourcePurchase);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            buildingManager.SetBuildMode(BuildingMode.Selection);
        }
    }

    public RepairState(BuildingManager buildingManager) : base(buildingManager) { }
}
