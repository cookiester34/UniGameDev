using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractableTimer : MonoBehaviour
{
    public SelectedBuildingUI selectedBuildingUI;
    private Button _button;
    private bool disableButton = true;
    private bool Toggle = false;
    private bool lastBuildingCanUpgrade = true;

    private void Start()
    {
        _button = GetComponent<Button>();
    }
    public void OnClick()
    {
        StartCoroutine("ToggleButton");
        lastBuildingCanUpgrade = selectedBuildingUI._selectedBuilding.canUpgradeBuilding;
    }

    private void Update()
    {
        if (Toggle)
        {
            _button.interactable = disableButton;
        }
    }

    private void OnDisable()
    {
        _button.interactable = lastBuildingCanUpgrade;
        disableButton = lastBuildingCanUpgrade;
        Toggle = false;
    }

    IEnumerator ToggleButton()
    {
        disableButton = false;
        Toggle = true;
        yield return new WaitForSecondsRealtime(0.5f);
        disableButton = lastBuildingCanUpgrade;
        yield return new WaitForEndOfFrame();
        Toggle = false;
    }
}
