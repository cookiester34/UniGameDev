using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSleepState : BeeState {
    public BeeSleepState(BeeStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() {
        _stateMachine.SleepTime(5f);
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        
    }

    public override void Exit() {
        
    }

    public void WakeUp() {
        _stateMachine.TargetBuilding = _stateMachine.Bee.Work;
        _stateMachine.ChangeState(BeeStates.Move);
    }
}
