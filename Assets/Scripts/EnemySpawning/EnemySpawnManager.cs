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

    public Seasons enemySpawnSeason = Seasons.Autumn;

    [HideInInspector]
    public int waveNumber = 0;

    [Range(1,5)]
    public int numberOfEnemiesSpawnableMin;
    [Range(3, 10)]
    public int numberOfEnemiesSpawnableMax;

    public List<Transform> enemyBuildingsList = new List<Transform>();

    public GameObject enemyPrefab;

    void SpawnWave()
    {
        if (SeasonManager.Instance.GetCurrentSeason() == enemySpawnSeason)
        {
            List<Transform> temp = enemyBuildingsList;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] == null)
                {
                    enemyBuildingsList.Remove(temp[i]);
                }
                else
                    StartCoroutine(DelaySpawn(temp[i]));
            }
            waveNumber++;
        }
    }

    IEnumerator DelaySpawn(Transform building)
    {
        yield return new WaitForSeconds(1f);
        if (Random.Range(0f, 100f) < 80f)
        {
            for (int i = 0; i < Random.Range(numberOfEnemiesSpawnableMin, numberOfEnemiesSpawnableMax + waveNumber); i++)
            {
                Instantiate(enemyPrefab, building.position, Quaternion.identity);
            }
        }
    }
}
