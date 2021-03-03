using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Util;

public class SeasonManager : MonoBehaviour
{
    #region Singleton
    private static SeasonManager _instance = null;
    public static SeasonManager Instance
    {
        get
        {
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

    public Seasons currentSeason;
    public delegate void SeasonChanged();
    public static event SeasonChanged SeasonChange;
    float seasonTimer;
    public int seasonLength;

    public GameObject springEffect;
    public GameObject summerEffect;
    public GameObject autumnEffect;
    public GameObject winterEffect;

    private void Start()
    {
        currentSeason = Seasons.Spring;
        seasonTimer = seasonLength;
        if (SeasonChange != null)
            SeasonChange();
    }

    private void Update()
    {
        if (seasonTimer <= 0)
        {
            seasonTimer = seasonLength;
            UpdateSeason(currentSeason);
            if (SeasonChange != null)
                SeasonChange();
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
                UIEventAnnounceManager.Instance.AnnounceEvent("Autumn begins, man the defenses");
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
