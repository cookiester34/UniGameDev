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

    public event Action SeasonChange;
    float seasonTimer;
    public int seasonLength;
    private float _halfSeasonLength;
    private int _currentYear = 0;
    private int _seasonParticleIndex = -1;

    public GameObject springEffect;
    public GameObject summerEffect;
    public GameObject autumnEffect;
    public GameObject winterEffect;

    [SerializeField] private ParticleSystem[] _seasonParticles;
    private float[] _targetEmissionRates = { 0f, 0f, 0f };

    [SerializeField] private Image seasonDisplay;
    [SerializeField] private Sprite springSprite;
    [SerializeField] private Sprite summerSprite;
    [SerializeField] private Sprite autumnSprite;
    [SerializeField] private Sprite winterSprite;

    [SerializeField] private TMP_Text _yearText;

    private void Start() {
        ResetParticles();
        SeasonChange += SeasonChanged;
        //SeasonChange += AudioManager.Instance.ChangeAmbienceTrack;
        seasonTimer = seasonLength;
        _halfSeasonLength = seasonTimer / 2f;
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
        {
            seasonTimer -= Time.deltaTime;
        }
        UpdateParticles();
    }

    void ResetParticles()
    {
        for (int i = 0; i < _seasonParticles.Length; i++)
        {
            var emissionVal = _seasonParticles[i].emission;
            if (_targetEmissionRates[i] == 0f)
            {
                _targetEmissionRates[i] = emissionVal.rateOverTimeMultiplier;
            }

            emissionVal.rateOverTimeMultiplier = 0f;
        }
    }

    void UpdateParticles()
    {
        if (_seasonParticleIndex >= 0)
        {
            var emissionVal = _seasonParticles[_seasonParticleIndex].emission;
            float distFromTargetEmission = Mathf.Abs(_halfSeasonLength - seasonTimer);
            float percentToTarget = distFromTargetEmission / _halfSeasonLength;
            float newEmissionRate = Mathf.Lerp(_targetEmissionRates[_seasonParticleIndex], 0f, percentToTarget);
            emissionVal.rateOverTimeMultiplier = newEmissionRate;
        }
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
        _halfSeasonLength = seasonTimer / 2f;
        ResetParticles();
        switch (currentSeason) {
            case Seasons.Spring:
                seasonDisplay.sprite = springSprite;
                _seasonParticleIndex = 0;
                break;
            case Seasons.Summer:
                seasonDisplay.sprite = summerSprite;
                _seasonParticleIndex = -1;
                break;
            case Seasons.Autumn:
                UIEventAnnounceManager.Instance.AnnounceEvent("Autumn begins, man the defenses", AnnounceEventType.Alert);
                seasonDisplay.sprite = autumnSprite;
                _seasonParticleIndex = 1;
                break;
            case Seasons.Winter:
                seasonDisplay.sprite = winterSprite;
                _seasonParticleIndex = 2;
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
