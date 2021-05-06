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
    }
    #endregion

    //#region OnSeasonChangeSpawnWave
    //private void OnEnable()
    //{
    //    SeasonManager.SeasonChange += TriggerDisaster;
    //}
    //private void OnDisable()
    //{
    //    SeasonManager.SeasonChange -= TriggerDisaster;
    //}
    //#endregion

    public int disasterChance = 30;
    public float disasterRadios = 10f;
    public float distanceFromQueen = 2f;
    public int minNumberOfBuildingsToTrigger = 5;

    private float timer;
    public float timeBetweenDisasters = 25f;

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            timer = timeBetweenDisasters;
            TriggerDisaster();
        }
        else
            timer -= Time.deltaTime;
    }

    void TriggerDisaster()
    {
        if (Random.Range(0, 100) < disasterChance)
        {
            if(BuildingManager.Instance.Buildings.Count > minNumberOfBuildingsToTrigger)
                spawnMeteor();
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
}
