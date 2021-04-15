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
        OnWaspsDefeated += AudioManager.Instance.StartPeaceMusic;
        OnWaspsSpawn += AudioManager.Instance.StartCombatMusic;
        renderer = GetComponent<Renderer>();
    }
    #endregion

    #region OnSeasonChangeSpawnWave
    private void OnEnable()
    {
        SeasonManager.SeasonChange += SpawnWave;
        UpdateWaspGroups();
    }
    private void OnDisable()
    {
        SeasonManager.SeasonChange -= SpawnWave;
    }
    #endregion

    public Seasons enemySpawnSeason = Seasons.Autumn;

    [HideInInspector]
    public int waveNumber = 1;

    /// <summary>
    /// How many more wasps to spawn per wave
    /// </summary>
    private int waveHordeMultiplier = 3;
    public int liveWasps = 0;

    [Range(1,5)]
    public int numberOfEnemiesSpawnableMin;
    [Range(3, 10)]
    public int numberOfEnemiesSpawnableMax;

    public List<Transform> enemyBuildingsList = new List<Transform>();
    int enemyBuildingListOldCount;

    public GameObject enemyPrefab;

    public delegate void WaspEvent();
    public event WaspEvent OnWaspsDefeated;
    public event WaspEvent OnWaspsSpawn;

    private Renderer renderer;


    public class WaspGroup
    {
        public List<Transform> wasps = new List<Transform>();
    }

    public List<WaspGroup> waspGroupList = new List<WaspGroup>();


    private void FixedUpdate()
    {
        checkVisible();//not sure if there's a better place for this
    }

    void checkVisible()
    {
        if (CurrentSceneType.SceneType != SceneType.LevelEditor)
        {
            foreach (Transform i in enemyBuildingsList)
            {
                if (FogOfWarBounds.instance.IsWaspVisible(i.position))
                {
                    i.gameObject.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    i.gameObject.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    void UpdateWaspGroups()
    {
        
        enemyBuildingListOldCount = enemyBuildingsList.Count;
        waspGroupList.Clear();
        foreach (Transform i in enemyBuildingsList)
        {
            waspGroupList.Add(new WaspGroup());
        }
    }

    public void UpdateLiveWasps()
    {
        liveWasps--;
        if (liveWasps <= 0)
        {
            OnWaspsDefeated?.Invoke();
        }
    }

    void SpawnWave()
    {
        if (enemyBuildingListOldCount != enemyBuildingsList.Count)
            UpdateWaspGroups();
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
                    StartCoroutine(DelaySpawn(temp[i], i));
            }
            waveNumber++;

            foreach(WaspGroup i in waspGroupList) //go through all wasp groups and merge the smaller groups
            {
                if (i.wasps.Count < 3)
                {
                    foreach (WaspGroup t in waspGroupList)
                    {
                        if (t.wasps.Count < 3) 
                        {
                            foreach(Transform q in t.wasps)
                            {
                                i.wasps.Add(q);
                                q.GetComponent<WaspAI>().WaspGroupID = t.wasps[0].GetComponent<WaspAI>().WaspGroupID;
                                q.GetComponent<WaspAI>().masterWasp = false;
                                q.GetComponent<WaspAI>().masterWaspObject = null;
                            }
                            t.wasps.Clear();
                        }
                    }
                }
            }            
        }
    }

    IEnumerator DelaySpawn(Transform building, int group)
    {
        bool masterSet = false;
        Transform masterWasp = null;
        yield return new WaitForSeconds(1f);
        if (Random.Range(0f, 100f) < 80f) {
            float rangeModifier = waveNumber * waveHordeMultiplier;
            for (int i = 0; i < Random.Range(numberOfEnemiesSpawnableMin + rangeModifier, numberOfEnemiesSpawnableMax + rangeModifier); i++)
            {
                GameObject wasp = Instantiate(enemyPrefab, building.position, Quaternion.identity);
                waspGroupList[group].wasps.Add(wasp.transform);
                WaspAI waspAI = wasp.GetComponent<WaspAI>();
                waspAI.spawnManager = this;
                waspAI.WaspGroupID = group;
                if (!masterSet)
                {
                    waspAI.masterWasp = true;
                    masterWasp = wasp.transform;
                    masterSet = true;
                }
                else
                {
                    waspAI.masterWaspObject = masterWasp;
                    waspAI.masterWaspAI = masterWasp.GetComponent<WaspAI>();
                }
                liveWasps++;
            }
        }
        OnWaspsSpawn?.Invoke();
    }
}
