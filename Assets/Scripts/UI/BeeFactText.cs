using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeeFactText : MonoBehaviour {
    void Start() {
        GetComponent<TMP_Text>().text = BeeFacts.GetFact();
    }
}
