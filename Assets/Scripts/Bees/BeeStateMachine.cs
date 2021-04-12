using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Bee))]
public class BeeStateMachine : MonoBehaviour {
    private Bee _bee;
    private BeeState _currentState = null;
    private Vector3 _targetPosition;

    public Bee Bee => _bee;

    public Vector3 TargetPosition {
        get => _targetPosition;
        set => _targetPosition = value;
    }

    #region states

    private BeeIdleState _idleState;
    private BeeMoveState _moveState;
    private BeeWorkState _workState;
    private BeeSleepState _sleepState;

    #endregion

    private void Awake() {
        _bee = GetComponent<Bee>();
        InitStates();
        _currentState = _idleState;
    }

    private void InitStates() {
        _idleState = new BeeIdleState(this);
        _moveState = new BeeMoveState(this);
        _workState = new BeeWorkState(this);
        _sleepState = new BeeSleepState(this);
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
            case BeeStates.Sleep:
                _currentState = _sleepState;
                break;
            case BeeStates.Work:
                _currentState = _workState;
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

    public bool NearBuilding(Building building) {
        bool nearBuilding = false;

        if (building != null) {
            if (Vector3.Distance(transform.position, building.transform.position) < 2.5f) {
                nearBuilding = true;
            }
        }

        return nearBuilding;
    }


    public void SleepTime(float timeSeconds) {
        gameObject.Disable(timeSeconds);
    }

    private void OnEnable() {
        if (_currentState is BeeWorkState workState) {
            workState.WakeUp();
        } else if (_currentState is BeeSleepState sleepState) {
            sleepState.WakeUp();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_targetPosition, 0.5f);
    }
}
