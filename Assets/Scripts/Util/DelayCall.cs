using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Small util class so coroutines can be used in non mono classes
/// </summary>
public class DelayCall : MonoBehaviour {
    private Action _action;
    private float _delay;

    public void DelayedCall(float delay, Action action) {
        _delay = delay;
        _action = action;
        StartCoroutine(nameof(Call));
    }

    IEnumerator Call() {
        yield return new WaitForSecondsRealtime(_delay);
        _action.Invoke();
        Destroy(gameObject, 0.1f);
    }
}
