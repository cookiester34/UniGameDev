using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UICancelButton : MonoBehaviour {
    [SerializeField] private GameObject cancelObject;
    private Button _button;
    
    private void Awake() {
        _button = GetComponent<Button>();
    }


    void CancelUI() {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
