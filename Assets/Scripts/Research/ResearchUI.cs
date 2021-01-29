using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ResearchUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    /// <summary>
    /// Image component
    /// </summary>
    private Image _image;

    /// <summary>
    /// Button component
    /// </summary>
    private Button _button;

    /// <summary>
    /// Text component should be found in child
    /// </summary>
    private Text _text;

    /// <summary>
    /// The research object that will be used to determine what the button does
    /// </summary>
    [SerializeField] private ResearchObject researchObject;

    private void Awake() {
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

        if (_text == null) {
            _text = GetComponentInChildren<Text>(true);
            if (_text == null) {
                Debug.LogError(
                    "Something has gone wrong in setting up the button components, ensure there is a child with text component");
            } else {
                _text.gameObject.SetActive(false);
            }
        }

        if (researchObject != null) {
            _image.sprite = researchObject.UiSprite;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClick);
            _text.text = researchObject.ResearchName;
        }
    }

    /// <summary>
    /// Event for the button click
    /// </summary>
    private void OnClick() {
        ResearchManager.Instance.ResearchTopic(researchObject);
    }

    /// <summary>
    /// Displays the tooltip on hover
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData) {
        _text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the tooltip on hover exit
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData) {
        _text.gameObject.SetActive(false);
    }
}
