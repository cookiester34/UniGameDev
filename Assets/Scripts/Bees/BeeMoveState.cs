﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Pathfinding;

public class BeeMoveState : BeeState {
    //private NavMeshAgent _agent;
    private IAstarAI _agent;

    public BeeMoveState(BeeStateMachine stateMachine) : base(stateMachine) {
        _agent = stateMachine.Bee.Agent;
    }
    
    public override void Enter() {
        if (_stateMachine.TargetPosition == Vector3.zero) {
            _stateMachine.ChangeState(BeeStates.Idle);
            return;
        }

        _agent.destination = _stateMachine.TargetPosition;
    }

    public override void Update() { }

    public override void PhysicsUpdate() {
        if (PathComplete()) {
            if (_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
                Flightpath path = _stateMachine.Bee.Work.Flightpath;
                if (path != null) {
                    _stateMachine.SetBuildFlightPath(path);
                    _stateMachine.ChangeState(BeeStates.Build);
                } else {
                    _stateMachine.ChangeState(BeeStates.Work);
                }
            } else if (_stateMachine.NearBuilding(_stateMachine.Bee.Home)) {
                Flightpath path = _stateMachine.Bee.Home.Flightpath;
                if (path != null) {
                    _stateMachine.SetBuildFlightPath(path);
                    _stateMachine.ChangeState(BeeStates.Build);
                } else {
                    _stateMachine.ChangeState(BeeStates.Sleep);
                }
            } else {
                _stateMachine.ChangeState(BeeStates.Idle);
            }
        }
    }

    public override void Exit() { }
    
    private bool PathComplete() {
        if ((!_agent.pathPending && !_agent.hasPath) ||
            Vector3.Distance( _agent.destination, _agent.position) <= 1.7f) {
            return true;
        }
 
        return false;
    }

}
