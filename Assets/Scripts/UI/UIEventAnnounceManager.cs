using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIEventAnnounceManager : MonoBehaviour
{
	private static UIEventAnnounceManager inst;
	public UIAnnounceMessageBox messageBoxPrefab;
	public Vector2 startPoint; //acts as the first point that the newest message box will spawn at
	int maxMessageBoxes = 6; //change this depending on how screen space works in the final project
	List<UIAnnounceMessageBox> messageBoxes = new List<UIAnnounceMessageBox>(); //lists all active message boxes
	GameObject canvas;
	
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

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void InitCanvas() {
		canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		if (!canvas) {
			Debug.LogWarning("Event announcements require a UI canvas to work.");
			Destroy(this);
		}
	}
	
	public void AnnounceEvent(string announceText) {
		while (messageBoxes.Count >= maxMessageBoxes) {
			DismissMessage(messageBoxes[0]);
		}
		CreateMessageBox(announceText);
	}
	
	public void DismissMessage(UIAnnounceMessageBox msgBox) {
		messageBoxes.Remove(msgBox);
		Destroy(msgBox.gameObject);
	}
	
	void CreateMessageBox(string message) {
		ShiftMessageBoxes();
		UIAnnounceMessageBox msgInst = Instantiate(messageBoxPrefab);
		msgInst.transform.SetParent(canvas.transform);
		msgInst.GetComponent<RectTransform>().anchoredPosition = startPoint;
		msgInst.SetText(message);
		messageBoxes.Add(msgInst);
	}
	
	void ShiftMessageBoxes() {
		for (int i = 0; i < messageBoxes.Count; i++) {
			Vector2 pos = messageBoxes[i].GetComponent<RectTransform>().anchoredPosition;
			pos.y += 105;
			messageBoxes[i].GetComponent<RectTransform>().anchoredPosition = pos;
		}
	}
}
