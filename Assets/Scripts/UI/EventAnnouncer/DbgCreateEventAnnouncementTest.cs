using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is purely being used for debug purposes and should not be used for any functionality meant for the final game

//currently it acts as a nice demo for the event announcement system
public class DbgCreateEventAnnouncementTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(TestMessages());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	IEnumerator TestMessages() {
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("This is a test message!", AnnounceEventType.Alert);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("This is also a test message!", AnnounceEventType.Alert);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("Another test message", AnnounceEventType.Misc);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("Test message 4", AnnounceEventType.Misc);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("Nearly done with these test messages", AnnounceEventType.Tutorial);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("Final test message", AnnounceEventType.Tutorial);
		yield return new WaitForSeconds(0.5f);
		UIEventAnnounceManager.Instance.AnnounceEvent("too many. First one should not appear anymore", AnnounceEventType.Tutorial);
	}
}
