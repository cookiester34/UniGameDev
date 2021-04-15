using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SeasonUI : MonoBehaviour
{

    private Text _text;
    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        SeasonManager.SeasonChange += UpdateUI;
    }
    private void OnDisable()
    {
        SeasonManager.SeasonChange -= UpdateUI;
    }

    void UpdateUI()
    {
        _text.text =SeasonManager.Instance.GetCurrentSeason().ToString();
    }
}
