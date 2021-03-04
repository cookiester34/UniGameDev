using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialProgress : MonoBehaviour
{
    public Image progressBar;
    float currentValue;
    public float fillTime;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (currentValue < 1f)
            {
            currentValue +=  Time.deltaTime * fillTime;
            }
            else
            {
                currentValue = 0;
            }
            progressBar.fillAmount = currentValue;
        }
    }
}
