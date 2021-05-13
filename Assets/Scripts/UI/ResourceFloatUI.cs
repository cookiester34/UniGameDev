using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceFloatUI : MonoBehaviour
{
    float alpha = 1;
    Vector3 pos;
    float newY;

    public Transform canvas;
    public Text resourceInfoText1;
    public Text resourceInfoText2;

    [HideInInspector]
    public float heightStartPos = 0;
    [HideInInspector]
    public Vector3 startPos;

    bool updateTextPos = false;

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
                //Destroy(this.gameObject);
            }
        }
    }
    public void TriggerResourceText(bool isPositive, List<ResourcePurchase> resourcePurchaseList)
    {
        updateTextPos = false;
        resourceInfoText1.text = "";
        resourceInfoText2.text = "";

        bool secondUI = false;
        foreach (ResourcePurchase i in resourcePurchaseList)
        {
            if (!secondUI)
            {
                if (isPositive)
                {
                    resourceInfoText1.text = "+ " + i.cost;
                    resourceInfoText1.color = Color.green;
                }
                else
                {
                    resourceInfoText1.text = i.cost.ToString();
                    resourceInfoText1.color = Color.red;
                }
            }
            else
            {
                if (isPositive)
                {
                    resourceInfoText2.text = "+ " + i.cost;
                    resourceInfoText2.color = Color.green;
                }
                else
                {
                    resourceInfoText2.text = i.cost.ToString();
                    resourceInfoText2.color = Color.red;
                }
            }

            secondUI = true;
            canvas.position = startPos;
            pos = canvas.position;
            newY = pos.y + heightStartPos;
            canvas.gameObject.SetActive(true);
            alpha = 1;
            updateTextPos = true;
        }
    }
}
