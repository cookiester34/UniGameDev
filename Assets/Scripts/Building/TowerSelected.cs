using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelected : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int TowerRadiusEnable = Shader.PropertyToID("TowerRadiusEnable");

    void Awake() {
        _renderer = GetComponent<Renderer>();
        BuildingManager.Instance.OnBuildingSelected += EnableTowerRadius;
        BuildingManager.Instance.UiBuildingSelected += EnableTowerRadius;
    }

    public void EnableTowerRadius(BuildingData buildingData) {
        if (buildingData == null) {
            EnableTowerRadius(false);
        } else if (buildingData.BuildingType.IsTower()) {
            EnableTowerRadius(true);
        }
    }

    public void EnableTowerRadius(Building building)
    {
        if(building == null)
        {
            EnableTowerRadius(false);
        }
        else if (building.BuildingType.IsTower())
        {
            EnableTowerRadius(true);
        }
    }

    private void EnableTowerRadius(bool enable)
    {
        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(TowerRadiusEnable, enable ? 1f : 0f);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void OnDestroy()
    {
        BuildingManager.Instance.OnBuildingSelected -= EnableTowerRadius;
        BuildingManager.Instance.UiBuildingSelected -= EnableTowerRadius;
    }
}
