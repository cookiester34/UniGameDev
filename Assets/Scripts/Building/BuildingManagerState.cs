using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingManagerState {
    protected BuildingManager buildingManager;

    public BuildingManagerState(BuildingManager buildingManager) {
        this.buildingManager = buildingManager;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();

}
