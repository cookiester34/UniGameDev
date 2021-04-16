using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SeasonUI : MonoBehaviour {

    [SerializeField] private Sprite springSprite;
    [SerializeField] private Sprite summerSprite;
    [SerializeField] private Sprite autumnSprite;
    [SerializeField] private Sprite winterSprite;
    private Image _image;

    private void Awake() {
        _image = GetComponent<Image>();
        // UpdateUi();
    }

    // private void OnEnable() {
    //     SeasonManager.SeasonChange += UpdateUi;
    // }
    // private void OnDisable() {
    //     SeasonManager.SeasonChange -= UpdateUi;
    // }
    //
    // void UpdateUi() {
    //     switch (SeasonManager.Instance.currentSeason) {
    //         case Seasons.Spring:
    //             _image.sprite = springSprite;
    //             break;
    //         case Seasons.Summer:
    //             _image.sprite = summerSprite;
    //             break;
    //         case Seasons.Autumn:
    //             _image.sprite = autumnSprite;
    //             break;
    //         case Seasons.Winter:
    //             _image.sprite = winterSprite;
    //             break;
    //     }
    // }
}
