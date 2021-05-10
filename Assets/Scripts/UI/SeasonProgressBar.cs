using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonProgressBar : MonoBehaviour
{
    private RectTransform _rt;
    private float _halfWidth;
    private float _posX = 0f;
    public float dbgScrollSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _halfWidth = _rt.sizeDelta.x / 2f;
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        _posX -= dbgScrollSpeed * Time.deltaTime;
        if (_posX <= -_halfWidth)
        {
            _posX += _halfWidth;
        }
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
    }
}
