using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BeeIdleState : BeeState {
    private Vector3 _lastDecidedLocation = Vector3.zero;

    public BeeIdleState(BeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        GoToRandomBuilding();
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        GoToRandomBuilding();
    }

    public override void Exit() {
        
    }

    /// <summary>
    /// Causes the bee to go to a semi random building, bee has a higher chance of going home or to work, and a smaller
    /// chance to go to some random building 
    /// </summary>
    private void GoToRandomBuilding() {
        List<Vector3> possiblePositions = new List<Vector3>();
        if (_stateMachine.Bee.Home != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Home)) {
            Vector3 position = _stateMachine.Bee.Home.transform.position;
            if (position != _lastDecidedLocation) {
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
            }
        }

        if (_stateMachine.Bee.Work != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
            Vector3 position = _stateMachine.Bee.Work.transform.position;
            if (position != _lastDecidedLocation) {
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
                possiblePositions.Add(position);
            }
        }

        if (BuildingManager.Instance.Buildings.Count > 0) {
            Building randBuild = BuildingManager.Instance.Buildings.Random();
            Vector3 position = randBuild.transform.position;
            if (position != _lastDecidedLocation) {
                possiblePositions.Add(position);
            }
        }

        // Random direction
        bool posX = Random.Range(0, 2) == 0;
        bool posZ = Random.Range(0, 2) == 0;
        var x = Random.Range(2.5f, 4f);
        if (!posX) {
            x *= -1;
        }
        var z = Random.Range(2.5f, 4f);
        if (!posZ) {
            z *= -1;
        }
        Vector3 randomPosition = _stateMachine.transform.position + new Vector3(x, 0f, z);
        if (FogOfWarBounds.instance.IsInFog(randomPosition))
        {
            if (NavMesh.SamplePosition(randomPosition, out var hit, 5f, NavMesh.AllAreas))
            {
                possiblePositions.Add(hit.position);
            }
        }

        if (possiblePositions.Count > 0) {
            GoToPosition(possiblePositions.Random());
        }
    }

    private void GoToPosition(Vector3 position) {
        _lastDecidedLocation = position;
        _stateMachine.TargetPosition = position;
        _stateMachine.ChangeState(BeeStates.Move);
    }

}
