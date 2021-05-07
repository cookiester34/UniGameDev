using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    private float timeMoving = 0;
    [SerializeField] private Terrain terrain;
    [SerializeField] private LayerMask terrainMask;

    private Vector3 lastMousePos;

    void Update() {
		if (CurrentInputType.Instance.GetInputType() == InputType.Game) {
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");
			Vector2 panDirection = new Vector2(x, y);
			if (panDirection != Vector2.zero) {
				timeMoving += Time.unscaledDeltaTime;
				Pan(panDirection.normalized * Time.unscaledDeltaTime);
			} else {
				timeMoving = 0;
			}

			if (Settings.CanMousePan.Value) {
				AttemptMousePan();
			}

            if (Input.GetMouseButton(2)) {
                var difference = Input.mousePosition - lastMousePos;
                Pan(difference * Time.unscaledDeltaTime);
            }
        }

        lastMousePos = Input.mousePosition;
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
    /// Moves the targets x and z position to the specified position
    /// </summary>
    /// <param name="position">The position to move the camera to</param>
    public void PanToPosition(Vector2 position) {
        var transPosition = transform.position;
        transPosition.x = position.x;
        transPosition.z = position.y;
        transPosition.y = terrain.SampleHeight(transPosition);

        Bounds worldBounds = terrain.terrainData.bounds;
        worldBounds.center += terrain.transform.position;
        var newPos = worldBounds.ClosestPoint(transPosition);
        newPos.y = terrain.SampleHeight(newPos) + terrain.transform.position.y;
        transform.position = newPos;
    }

    /// <summary>
    /// Updates the target position by modifying its value
    /// </summary>
    /// <param name="inputDirection">The direction in which to move the camera in the x and z axis</param>
    private void Pan(Vector2 inputDirection) {
        inputDirection *= Settings.CameraPanSpeed.Value;
        if (Settings.CanMouseAccelerate.Value) {
            inputDirection *= ConvertTimeToExtraSpeedMultiplier();
        }

        Vector3 transformRight = cameraTransform.right;

        // Get the forward flattened for height
        Vector3 transformForwardsFlattened = cameraTransform.forward;
        transformForwardsFlattened.y = 0;
        transformForwardsFlattened.Normalize();

        Vector3 movement = (transformRight * inputDirection.x) + (transformForwardsFlattened * inputDirection.y);
        Vector3 newPos = transform.position + movement;
        newPos.y = terrain.SampleHeight(newPos);
        newPos.y += terrain.transform.position.y;

        if (terrain.terrainData.bounds.Contains(newPos - terrain.transform.position)) {
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
