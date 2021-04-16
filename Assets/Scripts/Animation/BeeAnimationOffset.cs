using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeeAnimationOffset : MonoBehaviour {
    private Animator _animator;
    private static readonly int FlyOffset = Animator.StringToHash("FlyOffset");

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    void Start() {
        _animator.SetFloat(FlyOffset, Random.Range(0f, 1f));
    }
}
