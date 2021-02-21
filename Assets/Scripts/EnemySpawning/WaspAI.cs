using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaspAI : MonoBehaviour
{
    private List<Transform> targetsInRange = new List<Transform>();

    private GameObject queenBeeBuilding;

    //nav
    private NavMeshAgent _agent;

    //wasp Stats
    [Range(1, 10)]
    public int waspDamage = 1;
    [Range(10, 100)]
    public float waspHealth = 10;
    [Range(1, 10)]
    public float attackRange = 5;
    [Range(10, 50)]
    public float detectionRange = 20;
    [Range(1, 5)]
    public float attacktimer = 2;
    private float actualTimer;

    private bool isDead = false;

    void Awake()
    {
        queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        _agent = GetComponent<NavMeshAgent>();
        GetComponent<SphereCollider>().radius = detectionRange;
        actualTimer = attacktimer;
    }


    void Update()
    {
        if(queenBeeBuilding == null)
            queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");

        if (NearTarget())
        {
            _agent.destination = targetsInRange[0].position;
            if (AttackDistance(targetsInRange[0]))
            {
                if (actualTimer <= 0)// assuming bees use the same health script
                {
                    targetsInRange[0].GetComponent<Health>().ModifyHealth(-waspDamage);
                    actualTimer = attacktimer;
                }
            }
        }
        else
        {
            _agent.destination = queenBeeBuilding.transform.position;
        }
        if (actualTimer >= 0)
            actualTimer -= Time.deltaTime;
        if (isDead)
        {
            _agent.speed = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        waspHealth -= damage;
        if(waspHealth <= 0)
        {
            isDead = true;
            //do some effect here maybe
            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*other.CompareTag("Bee")|| */ other.CompareTag("Building"))
        {
            if (other != null)
            {
                if (other.GetComponent<Health>().CurrentHealth > 0) // assuming bees use the same health script
                    targetsInRange.Add(other.transform);
            }
        }
    }

    private bool NearTarget()
    {
        if (targetsInRange.Count > 0)
        {
            if (targetsInRange[0] == null || targetsInRange[0].GetComponent<Health>().CurrentHealth <= 0)
            {
                targetsInRange.RemoveAt(0);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
            return false;
    }

    private bool AttackDistance(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) < attackRange)
            return true;
        else
            return false;
    }
}
