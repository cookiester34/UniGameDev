using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour {
    [SerializeField] private Animator mainAnim;
    private static readonly int StartProperty = Animator.StringToHash("Start");
    [SerializeField] private GameObject content;
    private bool _oneShot = false;

    private void Start() {
        content.SetActive(false);
        if (!_oneShot && EntryTracker.VisitedMainMenu) {
            _oneShot = true;
            mainAnim.SetTrigger(StartProperty);
            mainAnim.speed = 100f;
            content.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (!_oneShot && Input.anyKey) {
            _oneShot = true;
            content.SetActive(true);
            mainAnim.SetTrigger(StartProperty);
            EntryTracker.VisitedMainMenu = true;
        }
    }
}
