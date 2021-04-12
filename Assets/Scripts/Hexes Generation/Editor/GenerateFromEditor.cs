using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateHexMap))]
[CanEditMultipleObjects]
public class GenerateFromEditor : Editor
{
	
	void OnEnable() {

		
	}
	
	public override void OnInspectorGUI()
    {
        serializedObject.Update();
		DrawDefaultInspector();
		GenerateHexMap myScript = (GenerateHexMap)target;
		if (GUILayout.Button("Set Height to Terrain")) {
			myScript.SetGeneratorHeight();
		}
		if (GUILayout.Button("Generate")) {
			myScript.Generate();
		}
		if (GUILayout.Button("Destroy Grid")) {
			myScript.DestroyGrid();
		}
		if (GUILayout.Button("Toggle Hex Visibility")) {
			myScript.ToggleVisibility();
		}

		if (GUILayout.Button("Clear hex with no visual")) {
			List<BuildingFoundation> hexes = FindObjectsOfType<BuildingFoundation>().ToList();
			foreach (BuildingFoundation foundation in hexes) {
				if (foundation.transform.childCount < 1) {
					DestroyImmediate(foundation.gameObject);
				}
			}
		}
        serializedObject.ApplyModifiedProperties();
    }
}
