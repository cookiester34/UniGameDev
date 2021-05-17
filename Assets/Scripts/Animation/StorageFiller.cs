using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StorageFiller : MonoBehaviour {
    [SerializeField] private Resource wax;
    [SerializeField] private Resource pollen;
    [SerializeField] private Resource honey;
    [SerializeField] private Resource jelly;
    private Animator _animator;
    private bool _update;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        _update = CurrentSceneType.IsGameLevel();
        if (!_update) {
            _animator.Play("New State 0", 0, 1f);
        }
    }

    private void Update() {
        if (_update) {
            float waxPercent = wax.CurrentResourceAmount / wax.ResourceCap;
            float honeyPercent = honey.CurrentResourceAmount / honey.ResourceCap;
            float jellyPercent = jelly.CurrentResourceAmount / jelly.ResourceCap;
            float pollenPercent = pollen.CurrentResourceAmount / pollen.ResourceCap;
            float percent = (waxPercent + honeyPercent + jellyPercent + pollenPercent) / 4f;
            _animator.Play("New State 0", 0, percent);
        }
    }
}
