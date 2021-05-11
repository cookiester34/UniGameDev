using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Util;

public class SeasonManager : MonoBehaviour
{
    #region Singleton
    private static SeasonManager _instance = null;
    public static SeasonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.SeasonSingleton);
                Instantiate(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("2nd instance of Season manager being created, destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    private Seasons currentSeason;

    public Seasons CurrentSeason {
        get => currentSeason;
        set {
            currentSeason = value;
            SeasonChange?.Invoke();
        }
    }
    
    public static event Action SeasonChange;
    float seasonTimer;
    public int seasonLength;
    private int _currentYear = 0;
    
    public GameObject springEffect;
    public GameObject summerEffect;
    public GameObject autumnEffect;
    public GameObject winterEffect;

    [SerializeField] private Image seasonDisplay;
    [SerializeField] private Sprite springSprite;
    [SerializeField] private Sprite summerSprite;
    [SerializeField] private Sprite autumnSprite;
    [SerializeField] private Sprite winterSprite;

    [SerializeField] private TMP_Text _yearText;

    private void Start() {
        SeasonChange += SeasonChanged;
        seasonTimer = seasonLength;
        UpdateSeason(Seasons.Winter);
    }

    private void Update()
    {
        if (_yearText)
        {
            _yearText.text = "Year " + _currentYear.ToString();
        }
        if (seasonTimer <= 0)
        {
            //seasonTimer = seasonLength;
            UpdateSeason(currentSeason);
        }
        else
            seasonTimer -= Time.deltaTime;
    }

    void UpdateSeason(Seasons season)
    {
        switch (season)
        {
            case Seasons.Spring:
                currentSeason = Seasons.Summer;
                break;
            case Seasons.Summer:
                currentSeason = Seasons.Autumn;
                break;
            case Seasons.Autumn:
                currentSeason = Seasons.Winter;
                break;
            case Seasons.Winter:
                _currentYear++;
                currentSeason = Seasons.Spring;
                break;
        }

        SeasonChangeHandler();
    }

    private void SeasonChanged() {
        seasonTimer = seasonLength;
        switch (currentSeason) {
            case Seasons.Spring:
                seasonDisplay.sprite = springSprite;
                break;
            case Seasons.Summer:
                seasonDisplay.sprite = summerSprite;
                break;
            case Seasons.Autumn:
                UIEventAnnounceManager.Instance.AnnounceEvent("Autumn begins, man the defenses", AnnounceEventType.Alert);
                seasonDisplay.sprite = autumnSprite;
                break;
            case Seasons.Winter:
                seasonDisplay.sprite = winterSprite;
                break;
        }
    }

    public Seasons GetCurrentSeason()
    {
        return currentSeason;
    }
    public void SetCurrentSeason(Seasons season)
    {
        if (currentSeason == season)
        {
            _currentYear++;
        } else
        {
            int seasonDiff = season - currentSeason;
            if (seasonDiff < 0)
            {
                _currentYear++;
            }
        }
        currentSeason = season;
        SeasonChangeHandler();
    }

    void SeasonChangeHandler()
    {
        if (summerEffect != null)
        {
            summerEffect.SetActive(currentSeason == Seasons.Summer);
        }
        if (springEffect != null)
        {
            springEffect.SetActive(currentSeason == Seasons.Spring);
        }
        if (autumnEffect != null)
        {
            autumnEffect.SetActive(currentSeason == Seasons.Autumn);
        }
        if (winterEffect != null)
        {
            winterEffect.SetActive(currentSeason == Seasons.Winter);
        }

        AudioManager.Instance.PlaySound("SeasonChange");
        SeasonChange?.Invoke();
    }

    public int GetCurrentYear()
    {
        return _currentYear;
    }
}

public enum Seasons { Spring, Summer, Autumn, Winter}
