using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions {
    /// <summary>
    /// Deactivates the object for x seconds and then re-enables it 
    /// </summary>
    /// <param name="gameObject">The object to disable</param>
    /// <param name="seconds">How long ot disable it for</param>
    public static void Disable(this GameObject gameObject, float seconds) {
        GameObject go = new GameObject("disableObj", typeof(DisableObject));
        var disableObject = go.GetComponent<DisableObject>();
        disableObject.toDisable = gameObject;
        disableObject.Disable(seconds);
    }
}
