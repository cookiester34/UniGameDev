using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            possiblePositions.Add(_stateMachine.Bee.Home.transform.position);
            possiblePositions.Add(_stateMachine.Bee.Home.transform.position);
            possiblePositions.Add(_stateMachine.Bee.Home.transform.position);
        }

        if (_stateMachine.Bee.Work != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
            possiblePositions.Add(_stateMachine.Bee.transform.position);
            possiblePositions.Add(_stateMachine.Bee.transform.position);
            possiblePositions.Add(_stateMachine.Bee.transform.position);
            possiblePositions.Add(_stateMachine.Bee.transform.position);
            possiblePositions.Add(_stateMachine.Bee.transform.position);
        }

        if (BuildingManager.Instance.Buildings.Count > 0) {
            Building randBuild = BuildingManager.Instance.Buildings.Random();
            possiblePositions.Add(randBuild.transform.position);
            Debug.Log("Somehow, somehow this debug log stops gameobject.transform from doing a stack " +
                      "overflow. I dont know how its possible that this overflow even happens from the call to" +
                      " gameobject.transform but apparently it is, id love to get rid of this silly debug log but" +
                      " it seems if I do then the weirdest stack overflow will come back to haunt me. If someone" +
                      " can fix this great and thanks, I however dont seem to be capable, I cant find anyone else" +
                      " that has had this issue either");
        }
        
        // Random direction
        Vector3 randomPosition =
            _stateMachine.transform.position + new Vector3(Random.Range(-2.5f, 2.5f), 0f, Random.Range(-2.5f, 2.5f));
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
