using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
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
    
    public GameObject springEffect;
    public GameObject summerEffect;
    public GameObject autumnEffect;
    public GameObject winterEffect;

    [SerializeField] private Image seasonDisplay;
    [SerializeField] private Sprite springSprite;
    [SerializeField] private Sprite summerSprite;
    [SerializeField] private Sprite autumnSprite;
    [SerializeField] private Sprite winterSprite;

    private void Start() {
        SeasonChange += SeasonChanged;
        seasonTimer = seasonLength;
        UpdateSeason(Seasons.Winter);
    }

    private void Update()
    {
        if (seasonTimer <= 0)
        {
            seasonTimer = seasonLength;
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
                currentSeason = Seasons.Spring;
                break;
        }

        if (summerEffect != null) {
            summerEffect.SetActive(currentSeason == Seasons.Summer);
        }
        if (springEffect != null) {
            springEffect.SetActive(currentSeason == Seasons.Spring);
        }
        if (autumnEffect != null) {
            autumnEffect.SetActive(currentSeason == Seasons.Autumn);
        }
        if (winterEffect != null) {
            winterEffect.SetActive(currentSeason == Seasons.Winter);
        }

        AudioManager.Instance.PlaySound("SeasonChange");
        SeasonChange?.Invoke();
    }

    private void SeasonChanged() {
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
        currentSeason = season;
    }
}

public enum Seasons { Spring, Summer, Autumn, Winter}
