using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    #region Instance
    private static EnemySpawnManager _instance = null;
    public static EnemySpawnManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("2nd instance of Enemy Spawn Manager being created, destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    #region OnSeasonChangeSpawnWave
    private void OnEnable()
    {
        SeasonManager.SeasonChange += SpawnWave;
    }
    private void OnDisable()
    {
        SeasonManager.SeasonChange -= SpawnWave;
    }
    #endregion

    private SeasonManager seasonManager;

    public Seasons enemySpawnSeason = Seasons.Autumn;

    [Range(1,5)]
    public int numberOfEnemiesSpawnableMin;
    [Range(3, 10)]
    public int numberOfEnemiesSpawnableMax;

    public List<Transform> enemyBuildingsList = new List<Transform>();

    public GameObject enemyPrefab;

    private void Start()
    {
        seasonManager = SeasonManager.Instance;
    }

    void SpawnWave()
    {
        if (seasonManager.GetCurrentSeason() == enemySpawnSeason)
        {
            foreach (Transform building in enemyBuildingsList)
            {
                if(building == null)
                {
                    enemyBuildingsList.Remove(building);//removing gives a weird error not sure why
                }
                else
                    StartCoroutine(DelaySpawn(building));
            }
        }
    }

    IEnumerator DelaySpawn(Transform building)
    {
        yield return new WaitForSeconds(1f);
        if (Random.Range(0f, 100f) < 60f)
        {
            //set to number of max + 1 because range int high value is exclusive
            for (int i = 0; i < Random.Range(numberOfEnemiesSpawnableMin, numberOfEnemiesSpawnableMax + 1); i++)
            {
                Instantiate(enemyPrefab, building.position, Quaternion.identity);
            }
        }
    }
}
