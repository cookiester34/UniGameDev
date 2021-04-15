using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTickFloatUI : MonoBehaviour
{
    public bool allowText = true;
    public Transform canvas;
    public Text resourceInfoText;
    bool updateTextPos = false;
    float alpha = 1;
    Vector3 pos;
    float newY;
    public float heightStartPos = 0;
    Vector3 startPos;

    public List<ResourceSupplier> suppliers = new List<ResourceSupplier>();

    // Start is called before the first frame update
    void Start()
    {
        if (allowText)
        {
            ResourceManagement.Instance.resourceTickEvent.AddListener(TriggerResourceText);
            canvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (updateTextPos)
        {
            newY += 0.05f;
            canvas.position = new Vector3(pos.x, newY, pos.z);

            //alpha = Mathf.Lerp(1, 0, 3000);
            //resourceInfoText.color = new Color(resourceInfoText.color.r, resourceInfoText.color.g, resourceInfoText.color.b, alpha);

            //if ((newY - pos.y) >= 5)
            //{
            //    updateTextPos = false;
            //    //canvas.gameObject.SetActive(false);
            //}
        }
    }

    void TriggerResourceText()
    {
        updateTextPos = false;
        resourceInfoText.text = "";

        foreach (ResourceSupplier i in suppliers)
        {
            if (i != null)
            {
                if (i.GetProductionAmount() > 0.005f || i.GetProductionAmount() < -0.005f)
                {
                    if (i.GetProductionAmount() > 0)
                    {
                        resourceInfoText.text += "<color=green>" + "+ " + i.GetProductionAmount() + "</color>" + "\n";
                    }
                    else
                    {
                        resourceInfoText.text += "<color=red>" + i.GetProductionAmount() + "</color>" + "\n";
                    }
                    canvas.position = i.GetBuilding().position;
                    pos = canvas.position;
                    newY = pos.y + heightStartPos;
                    canvas.gameObject.SetActive(true);
                    alpha = 1;
                    updateTextPos = true;
                }
                if (startPos == null)
                    startPos = i.GetBuilding().position;
            }
        }
    }

    public void TriggerTextEventResourcePurchaseList(bool isPositive, List<ResourcePurchase> resourcePurchaseList)
    {
        updateTextPos = false;
        resourceInfoText.text = "";

        foreach (ResourcePurchase i in resourcePurchaseList)
        {
            if (isPositive)
            {
                resourceInfoText.text += "<color=green>" + "+ " + i.cost + "</color>" + "\n";
            }
            else
            {
                resourceInfoText.text += "<color=red>" + i.cost + "</color>" + "\n";
            }
        }
        canvas.position = startPos;
        pos = canvas.position;
        newY = pos.y + heightStartPos;
        canvas.gameObject.SetActive(true);
        alpha = 1;
        updateTextPos = true;
    }
}
