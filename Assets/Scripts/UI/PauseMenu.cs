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
		gameObject.SetActive(false);
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public bool GetIsPaused(){
		return _isPaused;
	}
	
	public void TogglePause() {
		_isPaused = !_isPaused;
		if (_isPaused) {
			Time.timeScale = 0f;
			gameObject.SetActive(true);
		}
		else {
			Time.timeScale = 1f;
			gameObject.SetActive(false);
		}
	}
}
