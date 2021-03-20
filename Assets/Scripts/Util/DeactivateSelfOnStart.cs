using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very simple class to deactivate a game object on awake, may be that we need it to be awake so it is all set up
/// </summary>
public class DeactivateSelfOnStart : MonoBehaviour {
    void Start() {
        gameObject.SetActive(false);
    }
}
