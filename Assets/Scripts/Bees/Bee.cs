using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Pathfinding;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BeeStateMachine))]
//[RequireComponent(typeof(NavMeshAgent))]
public class Bee : MonoBehaviour {
    private Building _home;
    private Building _work;
    private BeeStateMachine _stateMachine;
    //private NavMeshAgent _agent;
    private IAstarAI _agent;


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

    public IAstarAI Agent {
        get => _agent;
        set => _agent = value;
    }

    private void Awake() {
        idCounter++;
        id = idCounter;

        _agent = GetComponent<IAstarAI>();

        _soundLoopBasePitch = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        _soundLoopVolume = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        transform.GetComponent<AudioSource>().volume = _soundLoopVolume;
        transform.GetComponent<AudioSource>().pitch = _soundLoopBasePitch;

        _agent.height = Random.Range(0.2f, 2f);
        //_agent.baseOffset = _agent.height / 2f;

        var scale = Random.Range(0.6f, 0.9f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Start() {
        _home = GetValidHouse();
        if (_home != null) {
            _home.AssignBee(this);
        }
    }

    private void Update()
    {
        if(transform.position.y < 0)
        {
            Vector3 resetHeight = new Vector3(transform.position.x, 1, transform.position.z);
            transform.position = resetHeight;
        }
    }

    private Building GetValidHouse() {
        Building validHouse = null;
        var housing = BuildingManager.Instance.GetAllStorageBuildingsOfType(ResourceType.Population);
        foreach (var building in housing) {
            if (building.CanAssignBee()) {
                validHouse = building;
                break;
            }
        }

        return validHouse;
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
