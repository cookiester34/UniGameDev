using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateHexMap))]
[CanEditMultipleObjects]
public class GenerateFromEditor : Editor
{
	SerializedProperty width;
	SerializedProperty length;
	SerializedProperty defaultHex;
	SerializedProperty colours;
	
	void OnEnable() {

		
	}
	
	public override void OnInspectorGUI()
    {
        serializedObject.Update();
		DrawDefaultInspector();
		GenerateHexMap myScript = (GenerateHexMap)target;
		if (GUILayout.Button("Generate")) {
			myScript.Generate();
		}
		if (GUILayout.Button("Update (Doesn't work)")) {
			myScript.UpdateGrid();
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
