using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeMoveState : BeeState {
    private NavMeshAgent _agent;
    private NavMeshPath _path;
    private Vector3 _lastTarget;

    public BeeMoveState(BeeStateMachine stateMachine) : base(stateMachine) {
        _agent = stateMachine.Bee.Agent;
    }
    
    public override void Enter() {
        if (_stateMachine.TargetPosition == Vector3.zero || _stateMachine.TargetPosition == _lastTarget) {
            _stateMachine.ChangeState(BeeStates.Idle);
            return;
        }

        _agent.destination = _stateMachine.TargetPosition;
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        if (PathComplete()) {
            if (_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
                _stateMachine.ChangeState(BeeStates.Work);
            } else if (_stateMachine.NearBuilding(_stateMachine.Bee.Home)) {
                _stateMachine.ChangeState(BeeStates.Sleep);
            } else {
                _stateMachine.ChangeState(BeeStates.Idle);
            }
        }
    }

    public override void Exit() {
        _lastTarget = _stateMachine.TargetPosition;
    }
    
    private bool PathComplete() {
        if (!_agent.hasPath ||
            Vector3.Distance( _agent.destination, _agent.transform.position) <= 0.3f) {
            return true;
        }
 
        return false;
    }

}
