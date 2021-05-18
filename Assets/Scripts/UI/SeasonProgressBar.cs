using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonProgressBar : MonoBehaviour
{
    private RectTransform _rt;
    private float _halfWidth;
    private float _posX = 0f;
    public float singleSeasonWidth = 278f;
    private float _halfSeasonWidth;
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
            _scrollSpeed = singleSeasonWidth / (float)SeasonManager.Instance.seasonLength;
            SeasonManager.Instance.SeasonChange += HandleSeasonChange;
        }
        _rt = GetComponent<RectTransform>();
        _halfWidth = (_rt.sizeDelta.x / 2f) * _rt.localScale.x;
        _halfSeasonWidth = singleSeasonWidth / 2f;
        _posX = _rt.anchoredPosition.x;
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
        HandleSeasonChange();
    }

    // Update is called once per frame
    void Update()
    {
        _posX -= _scrollSpeed * Time.deltaTime;
        UpdatePosition();
    }

    void HandleSeasonChange()
    {
        _scrollSpeed = singleSeasonWidth / (float)SeasonManager.Instance.seasonLength;
        //Debug.Log(SeasonManager.Instance.GetCurrentSeason().ToString());
        Seasons currentSeason = SeasonManager.Instance.GetCurrentSeason();
        switch (currentSeason)
        {
            case Seasons.Spring:
                ForceScrollToPosition(-((singleSeasonWidth * 4f) - _halfSeasonWidth));
                break;
            case Seasons.Summer:
                ForceScrollToPosition(-(singleSeasonWidth - _halfSeasonWidth));
                break;
            case Seasons.Autumn:
                ForceScrollToPosition(-((singleSeasonWidth * 2f) - _halfSeasonWidth));
                break;
            case Seasons.Winter:
                ForceScrollToPosition(-((singleSeasonWidth * 3f) - _halfSeasonWidth));
                break;
        }
    }

    void ForceScrollToPosition(float pos)
    {
        _posX = pos;
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (_posX <= -_halfWidth)
        {
            _posX += _halfWidth;
        }
        _rt.anchoredPosition = new Vector2(_posX, _rt.anchoredPosition.y);
    }
}
