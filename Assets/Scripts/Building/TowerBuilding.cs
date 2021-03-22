using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : Building
{
    public GameObject projectile;
    public float projectileSpeed = 10f;

    [Range(1,100)] //not sure on what the range values should be
    public int towerRange;
    public GameObject SphereRange;

    public float baseFiringSpeed;
    public float firingSpeed;
    private float timer = 1f;
    private AudioSource fireSound;

    [HideInInspector]
    public List<Transform> enemiesInRange = new List<Transform>();

    //set the collider object to the range of the tower
    protected override void Start() {
        base.Start();
        SphereRange.GetComponent<SphereCollider>().radius = towerRange;
        fireSound = gameObject.transform.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0 && timer <= 0) {
            if (numAssignedBees > 0) {
                FireAtEnemies();
            }
            timer = firingSpeed;
        }

        timer -= Time.deltaTime;
    }

    private void FireAtEnemies()
    {
        if (enemiesInRange[0] != null) {
            AudioManager.Instance.ModulateAudioSource(fireSound);
            fireSound.Play();
            GameObject temp = Instantiate(projectile, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Vector3 dir = (transform.position + new Vector3(0, 1, 0) - enemiesInRange[0].position).normalized;
            temp.GetComponent<Rigidbody>().AddForce(-dir * projectileSpeed, ForceMode.Impulse);
            StartCoroutine(DestroyProjectile(temp, 5f));
        }
        else
        {
            enemiesInRange.RemoveAt(0);
        }
    }

    public override void AssignBee(Bee bee) {
        base.AssignBee(bee);
        firingSpeed = baseFiringSpeed * (BuildingData.maxNumberOfWorkers + 1 - numAssignedBees);
    }

    public override Bee UnassignBee(Bee bee = null) {
        Bee returnBee = base.UnassignBee(bee);
        firingSpeed = baseFiringSpeed * (BuildingData.maxNumberOfWorkers + 1 - numAssignedBees);
        return returnBee;
    }

    IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(projectile != null)
            Destroy(projectile);
    }
}
