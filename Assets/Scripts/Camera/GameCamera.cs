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
        [SerializeField] private Transform target;

        /// <summary>
        /// The position that the camera will attempt to target
        /// </summary>
        private Vector3 _targetPosition;

        private float rotateSpeed = 0f;
        private float rotateStrength;

        /// <summary>
        /// Mask for the terrain
        /// </summary>
        [SerializeField] private LayerMask terrainMask;
        
        private Vector3 MAX_ZOOM = new Vector3(0f, 15f, 0f);
        private Vector3 MIN_ZOOM = new Vector3(0f, 5f, 0f);

        public Vector3 TargetPositon {
            get => _targetPosition;
            set => _targetPosition = value;
        }

        /// <summary>
        /// Use current position for its beginning target
        /// </summary>
        private void Awake() {
            _targetPosition = transform.position;
            rotateStrength = Settings.CameraRotateStrength.Value;
            UpdateHeight();
        }

        /// <summary>
        /// Gets the inputs to determine whether to pan
        /// </summary>
        void Update() {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(scroll) > 0.05f) {
                Vector3 newOffset = offset + (transform.forward * (scroll * Time.deltaTime * 500f));
                if (newOffset.y > MIN_ZOOM.y && newOffset.y < MAX_ZOOM.y) {
                    offset = newOffset;
                }
            }

            if (Input.GetKey(KeyCode.Q)) {
                rotateSpeed = rotateStrength;
            } else if (Input.GetKey(KeyCode.E)) {
                rotateSpeed = -rotateStrength;
            } else {
                rotateSpeed = 0f;
            }
            
        }

        /// <summary>
        /// Moves the camera if required, as well as its target Y height
        /// </summary>
        void FixedUpdate() {
            Vector3 oldPosition = transform.position;
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
            offset += transform.position - oldPosition;
            transform.position = Vector3.Lerp(transform.position, target.position + offset, 1f);
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
    }
}
