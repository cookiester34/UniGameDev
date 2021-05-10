using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonProgressBar : MonoBehaviour
{
    private RectTransform _rt;
    private float _halfWidth;
    private float _posX = 0f;
    public bool isDebug = false;
    public float dbgScrollSpeed = 1f;
    private float _scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (isDebug)
        {
            _scrollSpeed = dbgScrollSpeed;
        } else
        {
            _scrollSpeed = 100f / (float)SeasonManager.Instance.seasonLength;
        }
        _rt = GetComponent<RectTransform>();
        _halfWidth = _rt.sizeDelta.x / 2f;
        _posX = _rt.anchoredPosition.x;
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        _posX -= _scrollSpeed * Time.deltaTime;
        if (_posX <= -_halfWidth)
        {
            _posX += _halfWidth;
        }
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
    }
}
