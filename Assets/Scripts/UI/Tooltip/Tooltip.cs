using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    public static Tooltip Instance => _instance;
    private static Tooltip _instance;

    Camera cam;
    Vector3 min, max;
    RectTransform rect;
    [SerializeField] private Text _text;
    float offset = 1f;
 

    void Awake() {
        _instance = this;
        cam = Camera.main;
        rect = GetComponent<RectTransform>();
        min = new Vector3(0, 0, 0);
        max = new Vector3(cam.pixelWidth, cam.pixelHeight, 0);
        gameObject.SetActive(false);
    }

    void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        //get the tooltip position with offset
        Vector3 position = new Vector3(Input.mousePosition.x + rect.rect.width, Input.mousePosition.y - (rect.rect.height / 2 + offset), 0f);
        //clamp it to the screen size so it doesn't go outside
        transform.position = new Vector3(Mathf.Clamp(position.x, min.x + rect.rect.width/2, max.x - rect.rect.width/2), Mathf.Clamp(position.y, min.y + rect.rect.height / 2, max.y - rect.rect.height / 2), transform.position.z);
    }

    public void SetText(string text) {
        _text.text = text;
    }

    public void Activate(bool active = true) {
        UpdatePosition();
        gameObject.SetActive(active);
    }
}
