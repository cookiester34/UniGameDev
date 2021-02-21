using System.Collections;
using System.Collections.Generic;
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
        serializedObject.ApplyModifiedProperties();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
