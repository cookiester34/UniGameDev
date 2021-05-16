using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractableTimer : MonoBehaviour
{
    private Button _button;
    private bool disableButton = true;
    private bool Toggle = false;
    public void OnClick(Button button)
    {
        _button = button;
        StartCoroutine("ToggleButton");
    }

    private void Update()
    {
        if (Toggle)
        {
            _button.interactable = disableButton;
        }
    }

    IEnumerator ToggleButton()
    {
        disableButton = false;
        Toggle = true;
        yield return new WaitForSecondsRealtime(0.5f);
        disableButton = true;
        yield return new WaitForEndOfFrame();
        Toggle = false;
    }
}
