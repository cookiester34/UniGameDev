using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using Util;

public class TooltipEnabler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string TooltipText;
    private bool pointerOver;
    [SerializeField] private float timeToShow;
    private Timer _timer;
    
    private void Awake() {
        _timer = new Timer(timeToShow);
        _timer.OnTimerFinish += ShowText;
    }

    private void Update() {
        if (pointerOver) {
            _timer.Tick(Time.deltaTime);
        }
    }

    private void ShowText() {
        Tooltip.Instance.SetText(TooltipText);
        Tooltip.Instance.Activate();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        pointerOver = true;
        _timer.Reset(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        pointerOver = false;
        Tooltip.Instance.Activate(false);
    }
}
