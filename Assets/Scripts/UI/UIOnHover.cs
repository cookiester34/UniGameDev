using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;

    public bool playSoundOnHover;
    public AudioClip hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Enter");

        if (playSoundOnHover)
        {
            AudioManager.Instance.PlaySoundClip(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit");
    }

    void Update()
    {
        if (mouseOver)
        {
            //Debug.Log("Mouse Over");
        }
    }
}
