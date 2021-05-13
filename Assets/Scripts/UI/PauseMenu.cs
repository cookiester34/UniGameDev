using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class PauseMenu : MonoBehaviour
{
	private static PauseMenu inst;
	//public GameObject pauseObject;
	private bool _isPaused;
	public GameObject _mainContainer;
	public GameObject _saveLoadMenu;
	public GameObject _settingsMenu;
	public GameObject _sandboxMenu;
	private bool _hasSettingsLoaded;
	public SettingsPanel _settingsScript;

	public event Action Pause;
	public event Action UnPause;
	
    public static PauseMenu Instance {
        get {
            if (inst == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.PauseSingleton);
                Instantiate(go);
            }
            return inst;
        }
    }

	void Awake() { //Note: This script shouldn't need a "DontDestroyOnLoad"
		if (inst) {
            Debug.LogWarning("Attempted to create 2nd instance of Pause Manager");
            Destroy(this);
            return;
        }
        inst = this;
		
		//probably a more efficient way to handle this
		//_mainContainer = transform.Find("Container").gameObject;
		//_saveLoadMenu = transform.Find("SaveLoadPanel").gameObject;
		//_settingsMenu = transform.Find("SettingsPanel").gameObject;
		//_settingsScript = _settingsMenu.GetComponent<SettingsPanel>();
		_mainContainer.SetActive(false);
		_saveLoadMenu.SetActive(false);
		_settingsMenu.SetActive(false);
		_sandboxMenu.SetActive(false);
	}

	public bool GetIsPaused(){
		return _isPaused;
	}
	
	public void TogglePause() {
		_isPaused = !_isPaused;
		if (_isPaused) {
			Pause?.Invoke();
			Time.timeScale = 0f;
			_mainContainer.SetActive(true);
		}
		else {
			Time.timeScale = 1f; //will likely get overwritten by TimeManagement class (if it exists)
			_mainContainer.SetActive(false);
			_saveLoadMenu.SetActive(false);
			_settingsMenu.SetActive(false);
			_sandboxMenu.SetActive(false);
			if (_hasSettingsLoaded) {
				_settingsScript.SaveSettings();
			}
			UnPause?.Invoke();
		}
	}
	
	public void ToggleSaveLoad() {
		_saveLoadMenu.SetActive(!_saveLoadMenu.activeSelf);
		if (_settingsMenu.activeSelf) {
			_settingsMenu.SetActive(false);
		}
		
		if (_sandboxMenu.activeSelf) {
			_sandboxMenu.SetActive(false);
		}
	}
	
	public void ToggleSettings() {
		_settingsMenu.SetActive(!_settingsMenu.activeSelf);
		_hasSettingsLoaded = true;
		if (_saveLoadMenu.activeSelf) {
			_saveLoadMenu.SetActive(false);
		}

		if (_sandboxMenu.activeSelf) {
			_sandboxMenu.SetActive(false);
		}
	}

	public void ToggleSandbox() {
		_sandboxMenu.SetActive(!_sandboxMenu.activeSelf);
		
		if (_saveLoadMenu.activeSelf) {
			_saveLoadMenu.SetActive(false);
		}
		
		if (_settingsMenu.activeSelf) {
			_settingsMenu.SetActive(false);
		}
	}
	
	void OnDestroy() {
		Time.timeScale = 1f;
	}
}
