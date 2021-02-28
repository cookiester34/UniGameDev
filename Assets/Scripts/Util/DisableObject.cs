using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small util script that can be used to disable and object for x seconds and then reenable
/// </summary>
public class DisableObject : MonoBehaviour {
    public GameObject toDisable;

    public void Disable(float seconds) {
        StartCoroutine(DisableRoutine(seconds));
    }

    private IEnumerator DisableRoutine(float seconds) {
        toDisable.SetActive(false);
        yield return new WaitForSeconds(seconds);
        toDisable.SetActive(true);
        Destroy(gameObject);
    }
}
