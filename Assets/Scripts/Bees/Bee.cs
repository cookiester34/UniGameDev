using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BeeStateMachine))]
[RequireComponent(typeof(NavMeshAgent))]
public class Bee : MonoBehaviour {
    private Building _home;
    private BeeStateMachine _stateMachine;
    private NavMeshAgent _agent;

    private List<BeeTask> tasks = new List<BeeTask>();

    public Building Home {
        get => _home;
        set => _home = value;
    }

    public NavMeshAgent Agent {
        get => _agent;
        set => _agent = value;
    }

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
    }

    
    private void AddTask() {
        
    }
}

public class BeeTask {
    private bool permanent = false;
    private Vector3 position;
    private Building _building;
}
