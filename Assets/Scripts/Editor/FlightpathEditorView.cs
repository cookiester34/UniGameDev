using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Flightpath))]
public class FlightpathEditorView : MonoBehaviour {
    private Flightpath _flightpath;
    private void Awake() {
        _flightpath = GetComponent<Flightpath>();
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        GUIStyle style = new GUIStyle {normal = {textColor = Color.cyan}};
        Vector3 pos;

        if (_flightpath.EntryPoint != null) {
            pos = _flightpath.EntryPoint.position;
            Gizmos.DrawWireSphere(pos, 0.2f);
            Handles.Label(pos, "Entry", style);
        }

        if (_flightpath.ExitPoint != null) {
            pos = _flightpath.ExitPoint.position;
            Gizmos.DrawWireSphere(pos, 0.2f);
            Handles.Label(pos, "Exit", style);
        }

        if (_flightpath.FlightPoints != null && _flightpath.FlightPoints.Count > 0) {
            int i = 0;
            foreach (Transform point in _flightpath.FlightPoints) {
                pos = point.position;
                Gizmos.DrawWireSphere(pos, 0.2f);
                Handles.Label(pos, i.ToString(), style);
                i++;
            }
        }
    }
}
