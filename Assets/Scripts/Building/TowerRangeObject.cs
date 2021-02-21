using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeObject : MonoBehaviour
{
    public TowerBuilding tower;
    //if an enemy enters range add them to the list
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            tower.enemiesInRange.Add(collision.transform);
        }
    }

    //if an enemy leaves range remove them from the list
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Enemy") && tower.enemiesInRange.Contains(collision.transform))
        {
            tower.enemiesInRange.Remove(collision.transform);
        }
    }
}
