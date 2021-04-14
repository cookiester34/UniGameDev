using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemySpawnManager.Instance.enemyBuildingsList.Add(transform);
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
