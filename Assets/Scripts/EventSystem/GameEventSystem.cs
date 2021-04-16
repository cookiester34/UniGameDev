using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public GameObject SnowStorm;
    public static GameEventSystem current;
    Seasons currentSeason;
    Seasons oldSeason;
    bool SnowStormEnabled;
    bool meteorEnabled;
    float SnowStormTime;
    float meteorTime;

    private void Awake()
    {
        current = this;
    }


    public event Action OnSnowStormTrigger;
    public void snowStormTrigger()
    {
        if(OnSnowStormTrigger != null)
        {
            Debug.Log("Trigger Activated for SnowStorm");
            OnSnowStormTrigger();

        }
    }



    // Start is called before the first frame update
    void Start()
    {
        oldSeason = SeasonManager.Instance.GetCurrentSeason();
        GameEventSystem.current.OnSnowStormTrigger += TriggerSnowStorm;
    }

    // Update is called once per frame
    void Update()
    {


        currentSeason = SeasonManager.Instance.GetCurrentSeason();
        if (oldSeason != currentSeason)
        {
            Debug.Log("Current Season" + currentSeason);
            oldSeason = currentSeason;

            switch (currentSeason)
            {
                case Seasons.Spring:
                    //At beginning of winter
                    //SnowStormTime = UnityEngine.Random.Range(0, SeasonManager.Instance.seasonLength);
                    //SnowStormEnabled = true;
                    break;
                case Seasons.Summer:
                    //At beginning of winter
                    meteorTime = UnityEngine.Random.Range(0, SeasonManager.Instance.seasonLength);
                    meteorEnabled = true;
                    break;
               case Seasons.Autumn:
                    //At beginning of winter
                    //SnowStormTime = UnityEngine.Random.Range(0, SeasonManager.Instance.seasonLength);
                    //SnowStormEnabled = true;
                    break;
                case Seasons.Winter:
                    //At beginning of winter
                    SnowStormTime = UnityEngine.Random.Range(0, SeasonManager.Instance.seasonLength);
                    SnowStormEnabled = true;
                    Debug.Log("SnowStormTime" + SnowStormTime);
                    break;

            }
        }

        //Everytime
        if(SnowStormEnabled && ((SnowStormTime -= Time.deltaTime) == 0))
        {
            SnowStormEnabled = false;
            //OnSnowStormTrigger;
        }

        if (meteorEnabled && ((meteorTime -= Time.deltaTime) == 0))
        {
            meteorEnabled = false;
            //OnMeteorTrigger;
        }

    }

    void TriggerSnowStorm()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(SnowStorm, new Vector3(0, 0, 0), Quaternion.identity);
    }
    
}
