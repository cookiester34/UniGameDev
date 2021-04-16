using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTickFloatUI : MonoBehaviour
{
    public bool allowText = true;
    public Transform canvas;
    public Text resourceInfoText1;
    public Text resourceInfoText2;

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

            alpha -= 0.03f;
            resourceInfoText1.color = new Color(resourceInfoText1.color.r, resourceInfoText1.color.g, resourceInfoText1.color.b, alpha);
            resourceInfoText2.color = new Color(resourceInfoText2.color.r, resourceInfoText2.color.g, resourceInfoText2.color.b, alpha);

            if (alpha <= 0)
            {
                updateTextPos = false;
                //canvas.gameObject.SetActive(false);
            }
        }
    }

    void TriggerResourceText()
    {
        updateTextPos = false;
        resourceInfoText1.text = "";
        resourceInfoText2.text = "";

        bool secondUI = false;
        foreach (ResourceSupplier i in suppliers)
        {
            if (i != null)
            {
                if (i.GetProductionAmount() > 0.005f || i.GetProductionAmount() < -0.005f)
                {
                    if (!secondUI)
                    {
                        if (i.GetProductionAmount() > 0)
                        {
                            resourceInfoText1.text += "+ " + i.GetProductionAmount();
                            resourceInfoText1.color = Color.green;
                        }
                        else
                        {
                            resourceInfoText1.text += +i.GetProductionAmount();
                            resourceInfoText1.color = Color.red;
                        }
                    }
                    else
                    {
                        if (i.GetProductionAmount() > 0)
                        {
                            resourceInfoText2.color = Color.green;
                            resourceInfoText2.text += "+ " + i.GetProductionAmount();
                        }
                        else
                        {
                            resourceInfoText2.color = Color.red;
                            resourceInfoText2.text += +i.GetProductionAmount();
                        }
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
            secondUI = true;
        }
    }

    public void TriggerTextEventResourcePurchaseList(bool isPositive, List<ResourcePurchase> resourcePurchaseList)
    {
        updateTextPos = false;
        resourceInfoText1.text = "";
        bool secondUI = false;

        foreach (ResourcePurchase i in resourcePurchaseList)
        {
            if (!secondUI)
            {
                if (isPositive)
                {
                    resourceInfoText1.text += "+ " + i.cost;
                    resourceInfoText1.color = Color.green;
                }
                else
                {
                    resourceInfoText1.text += +i.cost;
                    resourceInfoText1.color = Color.red;
                }
            }
            else
            {
                if (isPositive)
                {
                    resourceInfoText2.text += "+ " + i.cost;
                    resourceInfoText2.color = Color.green;
                }
                else
                {
                    resourceInfoText2.text += +i.cost;
                    resourceInfoText2.color = Color.red;
                }
            }
            secondUI = true;
        }
        canvas.position = startPos;
        pos = canvas.position;
        newY = pos.y + heightStartPos;
        canvas.gameObject.SetActive(true);
        alpha = 1;
        updateTextPos = true;
    }
}
