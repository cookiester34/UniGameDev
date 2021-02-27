using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just makes sure the static applicationUtil is constructed
/// </summary>
public class QuittingObj : MonoBehaviour {
    private void Awake() {
        if (ApplicationUtil.IsQuitting) { }
    }
}
