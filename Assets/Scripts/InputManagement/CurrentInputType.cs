using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentInputType : MonoBehaviour
{
	private static CurrentInputType inst;
	private EventSystem eventSys;
	public InputType inputType = InputType.Game;
	
    public static CurrentInputType Instance {
        get {
            if (inst == null) {
                GameObject go = new GameObject();
				go.name = "CurrentInputTypeManager";
				go.AddComponent(typeof(CurrentInputType));
                //Instantiate(go);
            }
            return inst;
        }
    }

	void Awake() { //Note: This script shouldn't need a "DontDestroyOnLoad"
		if (inst) {
            Debug.LogWarning("Attempted to create 2nd instance of Current Input Type Manager");
            Destroy(this);
            return;
        }
        inst = this;
		//eventSys = GameObject.FindObjectOfType<EventSystem>();
		eventSys = EventSystem.current; //not sure if seperate reference "eventSys" is really needed. Will leave it as is for now
	}
	
	public InputType GetInputType() {
		return inputType;
	}

    // Update is called once per frame
    void Update()
    {
        if (eventSys && eventSys.currentSelectedGameObject) {
			if (eventSys.currentSelectedGameObject.GetComponent<InputField>()) {
				inputType = InputType.InputField;
			}
			else if (PauseMenu.Instance.GetIsPaused()) {
				inputType = InputType.Other;
			}
			else {
				inputType = InputType.Game;
			}
		}
		else if (PauseMenu.Instance.GetIsPaused())
        {
			inputType = InputType.Other;
        }
		else {
			inputType = InputType.Game;
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!BuildingManager.Instance.GetIsBuildingOrDestroying()) {
				//toggle pause menu
				PauseMenu.Instance.TogglePause();
			}
		}
    }
}
