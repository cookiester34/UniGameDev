using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : Building
{
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float damage = 2f;
    public float splashDamageDistance = 2f;
    [Tooltip("Keep lower than Damage")]
    public float splahDamageReduction = 1f;

    [Range(1,100)] //not sure on what the range values should be
    public int towerRange;
    public GameObject SphereRange;
    public GameObject SphereRangeVisual;

    public float baseFiringSpeed;
    public float firingSpeed;
    private float timer = 1f;
    private AudioSource fireSound;

    private Animator[] anim;

    [HideInInspector]
    public List<Transform> enemiesInRange = new List<Transform>();

    //set the collider object to the range of the tower
    protected override void Start() {
        base.Start();
        SphereRange.GetComponent<SphereCollider>().radius = towerRange;
        SphereRangeVisual.transform.localScale = new Vector3(towerRange * 2, towerRange * 2, towerRange * 2);
        fireSound = gameObject.transform.GetComponent<AudioSource>();
        anim = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0) {
            if (timer <= 0) 
            {
                if (numAssignedBees > 0) {
                    FireAtEnemies();
                }
                timer = firingSpeed;
            }
            //Quaternion _lookRotation = Quaternion.LookRotation((enemiesInRange[0].position - transform.position).normalized);
            ////over time
            //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
        }

        timer -= Time.deltaTime;
    }

    private void FireAtEnemies()
    {
        if (enemiesInRange[0] != null) {
            foreach (Animator animator in anim) {
                animator.SetTrigger("Attack");
            }
            transform.LookAt(enemiesInRange[0].position);
            transform.Rotate(new Vector3(0, -90, 0));
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            
            //anim.ResetTrigger("Attack");
        }
        else
        {
            enemiesInRange.RemoveAt(0);
        }
    }
    public void Shoot()
    {
        if (enemiesInRange.Count > 0) {
            AudioManager.Instance.ModulateAudioSource(fireSound);
            fireSound.Play();
            if (enemiesInRange[0] != null)
            {
                enemiesInRange[0].GetComponent<WaspAI>().TakeDamage(damage);


                ///for visual effect now
                GameObject temp = Instantiate(projectile, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                Vector3 dir = (transform.position + new Vector3(0, 1, 0) - enemiesInRange[0].position).normalized;
                temp.GetComponent<Rigidbody>().AddForce(-dir * projectileSpeed, ForceMode.Impulse);
                StartCoroutine(DestroyProjectile(temp, 3f));

                ///play splash effect here
                foreach (EnemySpawnManager.WaspGroup i in EnemySpawnManager.Instance.waspGroupList)
                {
                    foreach (Transform t in i.wasps)
                    {
                        if (Vector3.Distance(t.position, enemiesInRange[0].position) < splashDamageDistance)
                            t.GetComponent<WaspAI>().TakeDamage(damage - splahDamageReduction);
                    }
                }
            }
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
