using System;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Button which can control the active state of some game object
    /// </summary>
    public class UIBottomBarButton : MonoBehaviour {
        /// <summary>
        /// The contents which the button should modify the active state of
        /// </summary>
        [SerializeField] private GameObject content;

        private void Awake() {
            if (content == null) {
                Debug.LogError("Bar component has not been set on the UI bottom bar button");
            }
        }

        /// <summary>
        /// Set the the buttons associated contents to the specified active state, if contents are set to active but it
        /// is already active, it will instead hide the components if force is false
        /// </summary>
        /// <param name="active">The state to set the contents to</param>
        /// <param name="force">Whether the active should be used as is, disallows hiding the contents if active is
        /// called whilst already active</param>
        public void ActivateContents(bool active, bool force = false) {
			//BuildingManager.Instance.SetBuildMode(BuildingMode.Selection);
            if (force) {
                content.SetActive(active);
            } else {
                if (content.activeSelf) {
                    content.SetActive(false);
                } else {
                    content.SetActive(active);
                }
            }
        }

        public bool ContentsActive() {
            return content.activeSelf;
        }
    }
}
