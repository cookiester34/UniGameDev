using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnnounceMessageBox : MonoBehaviour {
	[SerializeField] private Image eventTypeImage;
	public TMP_Text eventTextbox;
	float lifetime = 5f;

    void Start()
    {
        StartCoroutine(Expire());
    }

    public void Setup(string text, Sprite image) {
		eventTextbox.text = text;
		eventTypeImage.sprite = image;
    }
	
	IEnumerator Expire() {
		yield return new WaitForSeconds(lifetime);
		UIEventAnnounceManager.Instance.DismissMessage(this);
	}
}
