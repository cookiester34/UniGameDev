using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : Building
{
    [Range(1,100)]
    public int towerAmmoCost;
    public GameObject projectile;
    private float projectileSpeed = 5f;

    [Range(1,100)] //not sure on what the range values should be
    public int towerRange;
    public GameObject sphereFiringRange;

    [Range(1,10)]
    public float baseFiringSpeed;
    public float firingSpeed;
    private float timer = 1f;

    private List<Transform> enemiesInRange = new List<Transform>();

    //set the collider object to the range of the tower
    private void Start()
    {
        sphereFiringRange.transform.localScale = new Vector3(towerRange, 1, towerRange);
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0 && timer <= 0)
        {
            if (assignedBees > 0)
            {
                FireAtEnemies();
                firingSpeed = baseFiringSpeed / BuildingData.maxNumberOfWorkers * assignedBees;
            }
            timer = firingSpeed;
        }
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
    }

    private void FireAtEnemies()
    {
        if (ResourceManagement.Instance.UseResource(new ResourcePurchase(resourceType, towerAmmoCost))) {

            GameObject temp = Instantiate(projectile, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Vector3 dir = (transform.position + new Vector3(0, 1, 0) - enemiesInRange[0].position).normalized;
            temp.GetComponent<Rigidbody>().AddForce(dir * projectileSpeed, ForceMode.Impulse);
            StartCoroutine(DestroyProjectile(temp, 5f));
        }
    }

    IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(projectile != null)
            Destroy(projectile);
    }

    //if an enemy enters range add them to the list
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.transform);
        }
    }

    //if an enemy leaves range remove them from the list
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && enemiesInRange.Contains(collision.transform))
        {
            enemiesInRange.Remove(collision.transform);
        }
    }
}
