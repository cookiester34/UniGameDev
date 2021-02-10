using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnnounceMessageBox : MonoBehaviour
{
	public Image thisImage;
	public Text eventTextbox;
	float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Expire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetText(string text) {
		eventTextbox.text = text;
	}
	
	IEnumerator Expire() {
		yield return new WaitForSeconds(lifetime);
		UIEventAnnounceManager.Instance.DismissMessage(this);
	}
}
