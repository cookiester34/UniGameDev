using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeeState {
    protected BeeStateMachine _stateMachine;

    protected BeeState(BeeStateMachine stateMachine) {
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// Use to do things when the state is entered
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Unity update
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Unity fixed update
    /// </summary>
    public abstract void PhysicsUpdate();

    /// <summary>
    /// Use to do when state is exited, free resources etc.
    /// </summary>
    public abstract void Exit();
}
