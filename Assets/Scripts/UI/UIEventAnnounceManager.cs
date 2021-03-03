using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEventAnnounceManager : MonoBehaviour
{
	private static UIEventAnnounceManager inst;
	public UIAnnounceMessageBox messageBoxPrefab;
	int maxMessageBoxes = 3; //change this depending on how screen space works in the final project
	List<UIAnnounceMessageBox> messageBoxes = new List<UIAnnounceMessageBox>(); //lists all active message boxes
	private GameObject _eventFitter;

	public delegate void EventAnnouncement();
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
	
    void Start()
    {
		
		//just going to leave these here in case they are needed to test again
		
		/*AnnounceEvent("This is a test message!");
		AnnounceEvent("This is also a test message!");
		AnnounceEvent("Another test message");
		AnnounceEvent("Test message 4");
		AnnounceEvent("Nearly done with these test messages");
		AnnounceEvent("Final test message");*/
    }

    void InitCanvas() {
		_eventFitter = GameObject.Find("EventFitter");

		if (_eventFitter == null) {
			Debug.LogWarning("Event announcements requires the event fitter to work. ");
			Destroy(gameObject);
		}
	}
	
	public void AnnounceEvent(string announceText) {
		while (messageBoxes.Count >= maxMessageBoxes) {
			DismissMessage(messageBoxes[0]);
		}
		CreateMessageBox(announceText);
		announcement?.Invoke();
	}
	
	public void DismissMessage(UIAnnounceMessageBox msgBox) {
		messageBoxes.Remove(msgBox);
		Destroy(msgBox.gameObject);
	}
	
	void CreateMessageBox(string message) {
		UIAnnounceMessageBox msgInst = Instantiate(messageBoxPrefab, _eventFitter.transform);
		msgInst.SetText(message);
		messageBoxes.Add(msgInst);
	}
}
