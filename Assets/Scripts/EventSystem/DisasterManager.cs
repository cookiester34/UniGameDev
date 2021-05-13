using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    #region Instance
    private static DisasterManager _instance = null;
    public void Start()
    {
        if(_instance != null)
        {
            Debug.LogWarning("2nd instance of Disaster Manager being created, destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
        SetupQueenBee();
        disasterChance = baseDisasterChance;
        InvokeRepeating(nameof(TriggerDisaster), 30, 30);
    }
    #endregion

    #region OnSeasonChangeSpawnWave
    private void OnEnable()
    {
        SeasonManager.Instance.SeasonChange += SnowStorm;
        snowStormEffect.SetActive(false);
    }
    private void OnDisable()
    {
        SeasonManager.Instance.SeasonChange -= SnowStorm;
    }
    #endregion

    private int disasterChance = 30;
    public int baseDisasterChance = 30;
    public int minNumberOfBuildingsToTrigger = 5;

    public GameObject snowStormEffect;

    void TriggerDisaster()
    {
        if (Random.Range(0, 100) < disasterChance)
        {
            if(BuildingManager.Instance.Buildings.Count > minNumberOfBuildingsToTrigger)
                spawnMeteor();
            disasterChance = baseDisasterChance;
        }
        else
        {
            disasterChance += 5;
        }
    }

    #region meteor
    public GameObject vfx;
    public Transform startPoint;
    private GameObject _queenBeeBuilding;

    private void SetupQueenBee()
    {
        if (_queenBeeBuilding == null)
        {
            _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        }
    }

    Vector3 PickRandomBuildingPos()
    {
        GameObject building = BuildingManager.Instance.Buildings[Random.Range(0, BuildingManager.Instance.Buildings.Count)].gameObject;
        if (building != _queenBeeBuilding)
            return building.transform.position;
        else
            return PickRandomBuildingPos();
    }

    void spawnMeteor()
    {
        if (startPoint == null)
        {
            return;
        }
        SetupQueenBee();

        var startPos = startPoint.position;
        GameObject objVFX = Instantiate(vfx, startPos, Quaternion.identity) as GameObject;
        Destroy(objVFX, 6);

        var endPos = PickRandomBuildingPos() + new Vector3(0,0.5f,0);

        RotateTo(objVFX, endPos);
    }

    void RotateTo(GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
    #endregion

    #region SnowStorm
    void SnowStorm()
    {
        if (Random.Range(0, 100) < disasterChance)
        {
            if (SeasonManager.Instance.GetCurrentSeason() == Seasons.Winter)
                StartCoroutine(nameof(SnowStormCycle));
        }
    }

    IEnumerator SnowStormCycle()
    {
        snowStormEffect.SetActive(true);
        while (true)
        {
            foreach (Building i in BuildingManager.Instance.Buildings)
            {
                i.GetComponent<Health>().ModifyHealth(-0.05f);
            }
            if (BeeManager.Instance)
            {
                foreach (Bee i in BeeManager.Instance.Bees)
                {
                    i.GetComponent<Health>().ModifyHealth(-0.01f);
                }
            }
            if (SeasonManager.Instance.GetCurrentSeason() != Seasons.Winter)
                break;
            yield return new WaitForSeconds(1);
        }
        snowStormEffect.SetActive(false);
    }
    #endregion
}
