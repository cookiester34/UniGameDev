using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHitchFixer : MonoBehaviour {

    private void Awake() {
        Preload();
    }

    private void Preload() {
        Resources.Load<GameObject>("Storage"); 
        Resources.Load<GameObject>("Housing");
        Resources.Load<GameObject>("Tower");
        Resources.Load<GameObject>("HoneyConverter");
        Resources.Load<GameObject>("WaxConverter");
        Resources.Load<GameObject>("JellyConverter");
        Resources.Load<GameObject>("QueenBeeBuilding");
        Resources.Load<GameObject>("EnemyBuilding");
    }
}
