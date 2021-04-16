using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEventAnnounceManager : MonoBehaviour
{
	private static UIEventAnnounceManager inst;
	public UIAnnounceMessageBox messageBoxPrefab;
	int maxMessageBoxes = 5; //change this depending on how screen space works in the final project
	List<UIAnnounceMessageBox> messageBoxes = new List<UIAnnounceMessageBox>(); //lists all active message boxes
	private GameObject _eventFitter;

	[SerializeField] private Sprite miscSprite;
	[SerializeField] private Sprite tutSprite;
	[SerializeField] private Sprite alertSprite;

	public delegate void EventAnnouncement(AnnounceEventType eventType);
	public static event EventAnnouncement announcement;
	
	public static UIEventAnnounceManager Instance {
        get {
            if (inst == null) {
                GameObject go = Resources.Load<GameObject>(ResourceLoad.EventAnnounceSingleton);
                Instantiate(go);
            }
            return inst;
        }
    }
	
    // Start is called before the first frame update
	void Awake() {
		if (inst) {
            Debug.LogWarning("Attempted to create 2nd instance of Event Announce Manager");
            Destroy(this);
            return;
        }

        inst = this;
		InitCanvas();
	}

	void InitCanvas() {
		_eventFitter = GameObject.Find("EventFitter");

		if (_eventFitter == null) {
			Debug.LogWarning("Event announcements requires the event fitter to work. ");
			Destroy(gameObject);
		}
	}
	
	public void AnnounceEvent(string announceText, AnnounceEventType eventType) {
		while (messageBoxes.Count >= maxMessageBoxes) {
			DismissMessage(messageBoxes[0]);
		}
		CreateMessageBox(announceText, eventType);
		announcement?.Invoke(eventType);
	}
	
	public void DismissMessage(UIAnnounceMessageBox msgBox) {
		messageBoxes.Remove(msgBox);
		Destroy(msgBox.gameObject);
	}
	
	void CreateMessageBox(string message, AnnounceEventType eventType) {
		UIAnnounceMessageBox msgInst = Instantiate(messageBoxPrefab, _eventFitter.transform);
		msgInst.Setup(message, GetMatchingSprite(eventType));
		messageBoxes.Add(msgInst);
	}

	Sprite GetMatchingSprite(AnnounceEventType eventType) {
		Sprite sprite = null;
		switch (eventType) {
			case AnnounceEventType.Alert:
				sprite = alertSprite;
				break;
			case AnnounceEventType.Misc:
				sprite = miscSprite;
				break;
			case AnnounceEventType.Tutorial:
				sprite = tutSprite;
				break;
			default:
				Debug.LogError("No matching sprite found");
				break;
		}

		return sprite;
	}
}
