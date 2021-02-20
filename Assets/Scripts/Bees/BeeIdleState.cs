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
        List<Building> possibleBuildings = new List<Building>();
        if (_stateMachine.Bee.Home != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Home)) {
            possibleBuildings.Add(_stateMachine.Bee.Home);
            possibleBuildings.Add(_stateMachine.Bee.Home);
        }

        if (_stateMachine.Bee.Work != null && !_stateMachine.NearBuilding(_stateMachine.Bee.Work)) {
            possibleBuildings.Add(_stateMachine.Bee.Work);
            possibleBuildings.Add(_stateMachine.Bee.Work);
        }

        if (BuildingManager.Instance.Buildings.Count > 0) {
            possibleBuildings.Add(BuildingManager.Instance.Buildings.Random());
        }

        if (possibleBuildings.Count > 0) {
            GoToBuilding(possibleBuildings.Random());
        }
    }

    private void GoToBuilding(Building building) {
        if (building != null) {
            _stateMachine.TargetBuilding = building;
            _stateMachine.ChangeState(BeeStates.Move);
        }
    }

}
