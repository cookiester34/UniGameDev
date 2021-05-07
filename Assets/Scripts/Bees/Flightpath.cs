using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Flightpath : MonoBehaviour {
    [SerializeField] private Transform entryPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private List<Transform> flightPoints;
    private int _startIndex = 0;
    private int _endIndex;
    [SerializeField] private float flyTime;
    [SerializeField] private float entryTime;

    public Transform EntryPoint => entryPoint;
    public Transform ExitPoint => exitPoint;
    public List<Transform> FlightPoints => flightPoints;
    public int EndIndex => _endIndex;
    public float FlyTime => flyTime;
    public float EntryTime => entryTime;

    private void Awake() {
        if (flightPoints != null) {
            _endIndex = flightPoints.Count + 1;
        } else {
            _endIndex = 1;
        }
    }

    public Transform GetPoint(int index) {
        if (index == 0) {
            return entryPoint;
        }

        if (index == _endIndex) {
            return exitPoint;
        }

        return flightPoints[index - 1];
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        GUIStyle style = new GUIStyle {normal = {textColor = Color.cyan}};

        if (entryPoint != null) {
            Gizmos.DrawWireSphere(entryPoint.position, 0.2f);
            Handles.Label(entryPoint.position, "Entry", style);
        }

        if (exitPoint != null) {
            Gizmos.DrawWireSphere(exitPoint.position, 0.2f);
            Handles.Label(exitPoint.position, "Exit", style);
        }

        if (flightPoints != null && flightPoints.Count > 0) {
            int i = 0;
            foreach (Transform point in flightPoints) {
                Gizmos.DrawWireSphere(point.position, 0.2f);
                Handles.Label(point.position, i.ToString(), style);
                i++;
            }
        }
    }
}
