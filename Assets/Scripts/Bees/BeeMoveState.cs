using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeMoveState : BeeState {
    private NavMeshAgent _agent;
    private NavMeshPath _path;

    public BeeMoveState(BeeStateMachine stateMachine) : base(stateMachine) {
        _agent = stateMachine.Bee.Agent;
    }
    
    public override void Enter() {
        if (_stateMachine.TargetBuilding == null) {
            _stateMachine.ChangeState(BeeStates.Idle);
            return;
        }

        _agent.destination = _stateMachine.TargetBuilding.transform.position;
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
