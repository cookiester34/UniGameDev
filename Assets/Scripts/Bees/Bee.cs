using System;
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


    private float _soundLoopBasePitch;
    private float _soundLoopVolume;

    private int id;
    private static int idCounter;

    public int Id {
        get => id;
        set => id = value;
    }

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
        idCounter++;
        id = idCounter;

        _agent = GetComponent<NavMeshAgent>();

        _soundLoopBasePitch = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        _soundLoopVolume = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        transform.GetComponent<AudioSource>().volume = _soundLoopVolume;
        transform.GetComponent<AudioSource>().pitch = _soundLoopBasePitch;
    }

    private void OnDestroy() {
        if (!ApplicationUtil.IsQuitting) {
            ResourceManagement.Instance.GetResource(ResourceType.Population).ModifyAmount(-1);
            if (_work != null) {
                _work.UnassignBee(this);
            }

            if (_home != null) {
                _home.UnassignBee(this);
            }
        }
    }
}
