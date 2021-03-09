using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    private float timeMoving = 0;
    private bool allowMousePan;
    [SerializeField] private float panSpeed;
    private Terrain _terrain;

    private void Awake() {
        _terrain = FindObjectOfType<Terrain>();
        panSpeed = Settings.CameraPanSpeed.Value;
        allowMousePan = Settings.CanMousePan.Value;
    }

    void Update() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 panDirection = new Vector2(x, y);
        if (panDirection != Vector2.zero) {
            timeMoving += Time.deltaTime;
            Pan(panDirection.normalized * Time.deltaTime);
        } else {
            timeMoving = 0;
        }

        if (allowMousePan) {
            AttemptMousePan();
        }

        // Vector3 currentPosition = transform.position;
        // currentPosition.y = _terrain.SampleHeight(currentPosition);
        // transform.position = currentPosition;
    }
    
    private void AttemptMousePan() {
        Vector2 mousePos = Input.mousePosition;
        float xNormalized = (Screen.width - mousePos.x) / Screen.width;
        float yNormalized = (Screen.height - mousePos.y) / Screen.height;
        float panX = 0;
        if (xNormalized > 0.99f) {
            panX = -0.1f;
        } else if (xNormalized < 0.01f) {
            panX = 0.1f;
        }
        float panY = 0;
        if (yNormalized > 0.99f) {
            panY = -0.1f;
        } else if (yNormalized < 0.01f) {
            panY = 0.1f;
        }
        Vector2 pan = new Vector2(panX, panY);
        if (pan != Vector2.zero) {
            Pan(pan);
        }
    }

    /// <summary>
    /// Updates the target position by modifying its value
    /// </summary>
    /// <param name="inputDirection">The direction in which to move the camera in the x and z axis</param>
    private void Pan(Vector2 inputDirection) {
        inputDirection *= panSpeed * ConvertTimeToExtraSpeedMultiplier();
        Vector3 transformRight = cameraTransform.right;

        // Get the forward flattened for height
        Vector3 transformForwardsFlattened = cameraTransform.forward;
        transformForwardsFlattened.y = 0;
        transformForwardsFlattened.Normalize();

        Vector3 movement = (transformRight * inputDirection.x) + (transformForwardsFlattened * inputDirection.y);
        Vector3 newPos = transform.position + movement;
        newPos.y = _terrain.SampleHeight(newPos) + _terrain.transform.position.y;
        if (_terrain.terrainData.bounds.Contains(newPos - _terrain.transform.position)) {
            transform.position = newPos;
        }
    }

    private float ConvertTimeToExtraSpeedMultiplier() {
        float extraSpeed = 1;

        if (timeMoving > 1) {
            extraSpeed = Mathf.Min(3, timeMoving);
        }

        return extraSpeed;
    }
}
