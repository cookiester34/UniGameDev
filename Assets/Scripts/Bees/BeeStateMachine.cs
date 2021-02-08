using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Bee))]
public class BeeStateMachine : MonoBehaviour {
    private Bee _bee;
    private BeeState _currentState = null;

    public Bee Bee => _bee;

    #region states

    private BeeIdleState _idleState;
    private BeeMoveState _moveState;

    #endregion

    private void Awake() {
        _bee = GetComponent<Bee>();
        InitStates();
        _currentState = _idleState;
    }

    private void InitStates() {
        _idleState = new BeeIdleState(this);
        _moveState = new BeeMoveState(this);
    }

    public void ChangeState(BeeStates state) {
        _currentState?.Exit();

        switch (state) {
            case BeeStates.Idle:
                _currentState = _idleState;
                break;
            case BeeStates.Move:
                _currentState = _moveState;
                break;

            default:
                Debug.LogError("Cannot switch to state, not implemented");
                break;
        }
        
        _currentState?.Enter();
    }

    private void Update() {
        _currentState?.Update();
    }

    private void FixedUpdate() {
        _currentState?.PhysicsUpdate();
    }
}
