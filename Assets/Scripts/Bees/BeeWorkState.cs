using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeWorkState : BeeState {
    
    public BeeWorkState(BeeStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() {
        _stateMachine.SleepTime(10f);
    }

    public override void Update() {
    
    }

    public override void PhysicsUpdate() {
    
    }

    public override void Exit() {
        
    }

    public void WakeUp() {
        _stateMachine.TargetBuilding = _stateMachine.Bee.Home;
        _stateMachine.ChangeState(BeeStates.Move);
    }
}
