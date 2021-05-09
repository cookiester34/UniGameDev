using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for an image and text to update when the building manager sends out an event that the selected building data
/// has changed
/// </summary>
[RequireComponent(typeof(Image))]
public class SelectedBuildingIcon : MonoBehaviour {
    private Image selectedImage;
    private TMP_Text selectedText;

    private void Awake() {
        selectedImage = GetComponent<Image>();
        selectedText = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable() {
        BuildingManager.Instance.UiBuildingSelected += UpdateUI;  
    }

    private void OnDisable() {
        BuildingManager.Instance.UiBuildingSelected -= UpdateUI;
    }

    private void UpdateUI(BuildingData buildingData) {
        Color color = selectedImage.color;
        color.a = buildingData == null ? 0 : 255;
        selectedImage.color = color;
        
        if (buildingData == null) {
            selectedText.text = "No building is currently selected";
            selectedImage.sprite = null;
        } else {
            selectedText.text = buildingData.Description;
            selectedImage.sprite = buildingData.UiImage;
        }
    }
}
