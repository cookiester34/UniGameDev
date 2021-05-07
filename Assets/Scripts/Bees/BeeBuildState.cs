using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Path = DG.Tweening.Plugins.Core.PathCore.Path;

public class BeeBuildState : BeeState {
    private Flightpath _flightpath;
    private int _pathIndex = 0;
    private int _exitChance = 50;
    private IAstarAI _agent;
    private Vector3 _target;
    private Path _mainPath;

    public Flightpath Path {
        get => _flightpath;
        set => _flightpath = value;
    }

    public BeeBuildState(BeeStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter() {
        _agent = _stateMachine.Bee.Agent;
        _pathIndex = -1;
        _exitChance = 50;
        Vector3[] points = new Vector3[_flightpath.FlightPoints.Count];
        for (int i = 0; i < _flightpath.FlightPoints.Count; i++) {
            points[i] = _flightpath.FlightPoints[i].position;
        }
        _mainPath = new Path(PathType.CatmullRom, points, 2);
        Tween startTween = _stateMachine.gameObject.transform.DOMove(
            _flightpath.GetPoint(0).position, 1f);
        startTween.OnComplete(() => {
            Tween tween = _stateMachine.gameObject.transform.DOPath(_mainPath, 3f);
            tween.OnComplete(AdvancePath);
        });
    }

    public override void Update() {
        
    }

    public override void PhysicsUpdate() {
        // if (AtPoint()) {
        //     if (_pathIndex == _path.EndIndex) {
        //         _stateMachine.ChangeState(BeeStates.Idle);
        //         return;
        //     }
        //     _pathIndex++;
        // }
        //
        // if (_pathIndex == _path.EndIndex) {
        //     int random = Random.Range(0, 100);
        //     if (random < _exitChance) {
        //         _pathIndex = 0;
        //         _exitChance += 10;
        //     }
        // }
        //
        // _target = _path.GetPoint(_pathIndex).position;
        // Tween tween = _stateMachine.gameObject.transform.DOMove(_target, 1f);
        // tween.OnComplete(AdvancePath);
    }

    private void AdvancePath() {
        // int random = Random.Range(0, 100);
        // if (random > _exitChance) {
        //     // Tween toStart = _stateMachine.gameObject.transform.DOMove(_flightpath.GetPoint(0).position, 1f);
        //     // toStart.OnComplete(RestartPath);
        // } else {
        //     
        // }
        
        Tween finalTween = _stateMachine.gameObject.transform.DOMove(
            _flightpath.GetPoint(_flightpath.EndIndex).position, 1f);
        finalTween.OnComplete(() => _stateMachine.ChangeState(BeeStates.Idle));
    }

    private void RestartPath() {
        Tween pathTween = _stateMachine.gameObject.transform.DOPath(_mainPath, 3f);
        pathTween.OnComplete(AdvancePath);
    }

    public override void Exit() {
        _flightpath = null;
    }
}
