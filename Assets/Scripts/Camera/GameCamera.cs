using System;
using UnityEngine;

namespace CameraNameSpace {
    /// <summary>
    /// The main camera to be used in the game scene.
    /// </summary>
    public class GameCamera : MonoBehaviour {
        /// <summary>
        /// The offset from the target position at which the camera will sit
        /// </summary>
        [Tooltip("The offset at which the camera will stay above its target position")]
        [SerializeField] private Vector3 offset;
        
        /// <summary>
        /// A value to change to increase/decrease the speed of panning
        /// </summary>
        [Tooltip("The speed at which the camera will pan")]
        [SerializeField] private int panSpeed;

        private float timeMoving = 0;
        
        /// <summary>
        /// The position that the camera will attempt to target
        /// </summary>
        private Vector3 _targetPosition;

        /// <summary>
        /// Mask for the terrain
        /// </summary>
        [SerializeField] private LayerMask terrainMask;
        
        private Vector3 MAX_ZOOM = new Vector3(5f, 15f, 5f);
        private Vector3 MIN_ZOOM = new Vector3(-5f, 5f, -5f);

        public Vector3 TargetPositon {
            get => _targetPosition;
            set => _targetPosition = value;
        }

        /// <summary>
        /// Use current position for its beginning target
        /// </summary>
        private void Awake() {
            _targetPosition = transform.position;
            UpdateHeight();
        }

        /// <summary>
        /// Gets the inputs to determine whether to pan
        /// </summary>
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

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(scroll) > 0.05f) {
                Vector3 newOffset = offset + (transform.forward * (scroll * Time.deltaTime * 500f));
                if (newOffset.x > MIN_ZOOM.x && newOffset.y > MIN_ZOOM.y && newOffset.z > MIN_ZOOM.z &&
                    newOffset.x < MAX_ZOOM.x && newOffset.y < MAX_ZOOM.y && newOffset.z < MAX_ZOOM.z ) {
                    offset = newOffset;
                }
            }
        }

        /// <summary>
        /// Moves the camera if required, as well as its target Y height
        /// </summary>
        void FixedUpdate() {
            if (transform.position != _targetPosition) {
                UpdateHeight();
                transform.position =
                    Vector3.Lerp(transform.position, _targetPosition + offset, 1f);
            }
        }

        /// <summary>
        /// Uses a ray cast to get the terrain that is being looked at, uses its height to set the target positions
        /// height
        /// </summary>
        private void UpdateHeight() {
            Vector3 forwardsPoint = CastRay(transform.forward);
            _targetPosition.y = forwardsPoint.y;
        }

        /// <summary>
        /// Casts a ray in a direction returning the raycast hit point, if the raycast fails returns Vector3.zero
        /// </summary>
        /// <param name="direction">The direction to fire the ray in</param>
        /// <returns>The point hit by the raycast if succesful, else Vector3.zero</returns>
        private Vector3 CastRay(Vector3 direction) {
            Vector3 hitPoint = new Vector3();
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, terrainMask)) {
                hitPoint = hitInfo.point;
            }

            return hitPoint;
        }

        /// <summary>
        /// Updates the target position by modifying its value
        /// </summary>
        /// <param name="inputDirection">The direction in which to move the camera in the x and z axis</param>
        private void Pan(Vector2 inputDirection) {
            inputDirection *= panSpeed * ConvertTimeToExtraSpeedMultiplier();
            Vector3 transformRight = transform.right;
            
            // Get the forward flattened for height
            Vector3 transformForwardsFlattened = transform.forward;
            transformForwardsFlattened.y = 0;
            transformForwardsFlattened.Normalize();

            Vector3 movement = (transformRight * inputDirection.x) + (transformForwardsFlattened * inputDirection.y);
            _targetPosition += movement;
        }

        private float ConvertTimeToExtraSpeedMultiplier() {
            float extraSpeed = 1;

            if (timeMoving > 1) {
                extraSpeed = Mathf.Min(3, timeMoving);
            }

            return extraSpeed;
        }
    }
}
