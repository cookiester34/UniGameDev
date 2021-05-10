using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialProgress : MonoBehaviour {
    public Image progressBar;
    [SerializeField] private GameObject progress;
    protected float currentValue;
    private float fillSpeed;
    private bool active;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (currentValue < 1f)
            {
                currentValue += Time.deltaTime * fillSpeed;
            }
            else
            {
                currentValue = 0;
            }
            progressBar.fillAmount = currentValue;
        }
    }

    public void Deactivate()
    {
        active = false;
        progressBar.fillAmount = 0;
        progress.SetActive(false);
    }

    public void Activate(float speed)
    {
        active = true;
        fillSpeed = speed;
        progress.SetActive(fillSpeed > 0);
    }
}
