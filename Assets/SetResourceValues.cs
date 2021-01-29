using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResourceValues : MonoBehaviour
{
    private int pollenAmount;
    private int honeyAmount;
    private int royalJellyAmount;
    private int waxAmount;
    private int populationAmount;

    public Text pollenText;
    public Text honeyText;
    public Text jellyText;
    public Text waxText;
    public Text populationText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame. Gets and exposes the values of resources to whichever UI this script is attached to.
    void Update()
    {
        GetCurrentResourceAmounts();
        SetCurrentResourceAmounts();
    }

    // Gets the current value of our resources.
    public void GetCurrentResourceAmounts()
    {
        pollenAmount = Resources.Load<Resource>("Pollen").currentResourceAmount;
        honeyAmount = Resources.Load<Resource>("Honey").currentResourceAmount;
        royalJellyAmount = Resources.Load<Resource>("RoyalJelly").currentResourceAmount;
        waxAmount = Resources.Load<Resource>("Wax").currentResourceAmount;
        populationAmount = Resources.Load<Resource>("Population").currentResourceAmount;
    }

    // Sets public text with the values from their respective resources.
    public void SetCurrentResourceAmounts()
    {
        pollenText.text = "Pollen " + pollenAmount.ToString();
        honeyText.text = "Honey " + honeyAmount.ToString();
        jellyText.text = "Royal Jelly " + royalJellyAmount.ToString();
        waxText.text = "Wax " + waxAmount.ToString();
        populationText.text = "Population: " + populationAmount.ToString();
    }
}
