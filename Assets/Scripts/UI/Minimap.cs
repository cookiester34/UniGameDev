using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
    ,IPointerClickHandler
{

    private RectTransform _rt;
    private Camera _minimapCam;
    private CameraTarget _camTarget;

    private float _xCentreOffset;
    private float _yCentreOffset;
    private float _xAdjust;
    private float _yAdjust;
    private Vector2 _centrePoint;
    private float _maxRadius = 125f;
    // Start is called before the first frame update
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _minimapCam = GameObject.Find("MinimapCam").GetComponent<Camera>();
        _camTarget = GameObject.FindObjectOfType<CameraTarget>();

        _xCentreOffset = _rt.sizeDelta.x / 2f;
        _yCentreOffset = _rt.sizeDelta.y / 2f;
        _xAdjust = _minimapCam.pixelWidth / _rt.sizeDelta.x;
        _yAdjust = _minimapCam.pixelHeight / _rt.sizeDelta.y;
        _centrePoint = new Vector2(_xCentreOffset * _xAdjust, _yCentreOffset * _yAdjust);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 position2d = new Vector2();
        RaycastHit hit;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, eventData.pressPosition, null, out position2d);
        position2d.x = (position2d.x + _xCentreOffset) * _xAdjust;
        position2d.y = (position2d.y + _yCentreOffset) * _yAdjust;

        if (isPointInCircle(position2d) && Physics.Raycast(_minimapCam.ScreenPointToRay(position2d), out hit))
        {
            //GameObject dbgCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //dbgCube.transform.position = hit.point;
            //reusing position2d here as it means another Vector2 doesn't need to be declared
            position2d.x = hit.point.x;
            position2d.y = hit.point.z;
            _camTarget.PanToPosition(position2d);
        }
    }

    bool isPointInCircle(Vector2 point)
    {
        return Vector2.Distance(point, _centrePoint) <= _maxRadius;
    }
}
