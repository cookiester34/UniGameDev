﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BeeStateMachine))]
[RequireComponent(typeof(NavMeshAgent))]
public class Bee : MonoBehaviour {
    private Building _home;
    private Building _work;
    private BeeStateMachine _stateMachine;
    private NavMeshAgent _agent;

    public Building Home {
        get => _home;
        set => _home = value;
    }

    public Building Work {
        get => _work;
        set => _work = value;
    }

    public NavMeshAgent Agent {
        get => _agent;
        set => _agent = value;
    }

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float currentSpeed = transform.GetComponent<NavMeshAgent>().velocity.magnitude;
        float movementPitchComponent = currentSpeed / 30;
        float basePitch = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        float volume = 1f + UnityEngine.Random.Range(-0.5f, 0.05f);
        transform.GetComponent<AudioSource>().pitch = basePitch + movementPitchComponent;
        transform.GetComponent<AudioSource>().volume = volume;
    }
}
