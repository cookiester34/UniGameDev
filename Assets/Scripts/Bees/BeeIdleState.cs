using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeIdleState : BeeState {
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
            possiblePositions.Add(_stateMachine.Bee.Home.transform.position);
            possiblePositions.Add(_stateMachine.Bee.Home.transform.position);
        }

        if (_stateMachine.Bee.Work != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
            possiblePositions.Add(_stateMachine.Bee.transform.position);
            possiblePositions.Add(_stateMachine.Bee.transform.position);
        }

        if (BuildingManager.Instance.Buildings.Count > 0) {
            possiblePositions.Add(BuildingManager.Instance.Buildings.Random().transform.position);
        }
        
        // Random direction
        Vector3 randomPosition =
            _stateMachine.transform.position + new Vector3(Random.Range(0f, 5f), 0f, Random.Range(0f, 5f));
        possiblePositions.Add(randomPosition);

        if (possiblePositions.Count > 0) {
            GoToPosition(possiblePositions.Random());
        }
    }

    private void GoToPosition(Vector3 position) {
        _stateMachine.TargetPosition = position;
        _stateMachine.ChangeState(BeeStates.Move);
    }

}
