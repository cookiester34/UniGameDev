using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuildingButton : MonoBehaviour {

    private void Awake() {
        gameObject.SetActive(false);
        BuildingManager.Instance.OnBuildingSelected += gameObject.SetActive;
    }
}
