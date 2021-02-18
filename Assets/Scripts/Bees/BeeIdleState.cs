using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeIdleState : BeeState {
    public BeeIdleState(BeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        GoHome();
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        GoHome();
    }

    public override void Exit() {
        
    }

    private void GoHome() {
        if (_stateMachine.Bee.Home != null) {
            _stateMachine.TargetBuilding = _stateMachine.Bee.Home;
            _stateMachine.ChangeState(BeeStates.Move);
        }
    }

}
