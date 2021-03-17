﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconHeightAdjustment : MonoBehaviour
{
	private float _height = 40f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
		pos.y = _height;
		transform.position = pos;
    }
}