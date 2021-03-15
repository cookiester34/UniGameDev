using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Component to hold all the UI bottom buttons that manages which ones are active at a given time.
    /// </summary>
    public class UIBottomBar : MonoBehaviour {
        List<UIBottomBarButton> _buttons = new List<UIBottomBarButton>();
        [SerializeField] private UIBottomBarButton beginActive;

        private void Awake() {
            _buttons = GetComponentsInChildren<UIBottomBarButton>().ToList();
            ForceBeginActive();
        }

        /// <summary>
        /// Forces the component into the state where the beginActive is active and the rest are not
        /// </summary>
        private void ForceBeginActive() {
            if (beginActive != null) {
                foreach (UIBottomBarButton button in _buttons) {
                    button.ActivateContents(button == beginActive, true);
                }
            }
        }

        /// <summary>
        /// Changes the active state of the buttons contents
        /// </summary>
        /// <param name="activeButton"></param>
        public void MakeContentsActive(UIBottomBarButton activeButton) {
            foreach (UIBottomBarButton button in _buttons) {
                button.ActivateContents(button == activeButton, true);
            }
        }

        public void HideContents(UIBottomBarButton activeButton) {
            if (activeButton.ContentsActive()) {
                activeButton.ActivateContents(false);
            }
        }
    }
}
