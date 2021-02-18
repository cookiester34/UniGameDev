﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeMoveState : BeeState {
    private NavMeshAgent _agent;
    private NavMeshPath _path;
    private float moveSpeed = 3;

    public BeeMoveState(BeeStateMachine stateMachine) : base(stateMachine) {
        _agent = stateMachine.Bee.Agent;
    }
    
    public override void Enter() {
        if (_stateMachine.TargetBuilding == null) {
            _stateMachine.ChangeState(BeeStates.Idle);
            return;
        }

        _agent.destination = _stateMachine.TargetBuilding.transform.position;
        _agent.speed = moveSpeed;
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        if (PathComplete()) {
            _stateMachine.ChangeState(BeeStates.Idle);
        }
    }

    public override void Exit() {
        
    }
    
    private bool PathComplete()
    {
        if (!_agent.hasPath 
            || Vector3.Distance( _agent.destination, _agent.transform.position) <= _agent.stoppingDistance) {
            return true;
        }
 
        return false;
    }

}
