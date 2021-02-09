﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class UiHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        /// <summary>
        /// Image component
        /// </summary>
        private Image _image;

        /// <summary>
        /// Button component
        /// </summary>
        protected Button _button;

        /// <summary>
        /// Text component should be found in child
        /// </summary>
        private Text _tooltipText;

        [Tooltip("The scriptable object must implement IUiClickableHover, used to call its methods")]
        [SerializeField] protected ScriptableObject _scriptableObject;
        private IUiClickableHover _clickableHover;

        protected virtual void Awake() {
            Setup();
        }

        private void OnValidate() {
            Setup();
        }

        /// <summary>
        /// Sets up required components, additionally reads data from the research object to set up image, text etc.
        /// </summary>
        private void Setup() {
            if (_image == null) {
                _image = GetComponent<Image>();
            }

            if (_button == null) {
                _button = GetComponent<Button>();
            }

            if (_tooltipText == null) {
                _tooltipText = GetComponentInChildren<Text>(true);
                if (_tooltipText == null) {
                    Debug.LogError(
                        "Something has gone wrong in setting up the button components, ensure there is a child with text component");
                } else {
                    _tooltipText.gameObject.SetActive(false);
                }
            }

            if (_scriptableObject == null) {
                Debug.LogWarning(
                    "The scriptable object is null, nothing to use to setup the ui from: " + gameObject.name);
                return;
            }

            if (!(_scriptableObject is IUiClickableHover)) {
                Debug.LogError(
                    "An incompatible scriptable is being used, the scriptable must implement IUiClickableHover "
                    + gameObject.name);
            } else {
                _clickableHover = (IUiClickableHover) _scriptableObject;
                _image.sprite = _clickableHover.GetSprite();
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(_clickableHover.OnClick);
                _tooltipText.text = _clickableHover.GetHoverText();
            }
        }

        /// <summary>
        /// Displays the tooltip on hover
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData) {
            _tooltipText.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the tooltip on hover exit
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData) {
            _tooltipText.gameObject.SetActive(false);
        }
    }
}
