using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeeStateMachine))]
public class Bee : MonoBehaviour {
    // Got stuff to put here or states, pathfinding etc.
    private Vector3 homePosition;
    private BeeStateMachine _stateMachine;

    private void Awake() {
        
    }
}
