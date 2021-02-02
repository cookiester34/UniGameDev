using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper so that we can access saveload functions, put on prefab to access in buttons
/// </summary>
public class UnitySaveWrapper : MonoBehaviour {
    public void CreateSaveFromScene() {
        SaveLoad.CreateSaveFromScene("abc");
    }

    public void LoadSave() {
        SaveLoad.Load("abc");
    }
}
