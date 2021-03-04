using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class WaspAI : MonoBehaviour
{
    public LayerMask mask;

    private List<Transform> targetsInRange = new List<Transform>();
    private Transform _currentTarget;

    private GameObject _queenBeeBuilding;

    //nav
    private NavMeshAgent _agent;

    //wasp Stats
    Health health;
    [Range(1, 10)]
    public int waspDamage = 1;
    [Range(1, 10)]
    public float attackRange = 5;
    [Range(10, 50)]
    public float detectionRange = 20;
    [Range(1, 5)]
    public float attacktimer = 2;
    private float actualTimer;

    private void OnValidate() {
        GetComponent<SphereCollider>().radius = detectionRange;
    }

    void Awake() {
        health = GetComponentInParent<Health>();
        _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        _agent = GetComponent<NavMeshAgent>();
        actualTimer = attacktimer;
    }

    private bool SetupQueenBee() {
        bool setup = true;
        if (_queenBeeBuilding == null) {
            _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
            if (_queenBeeBuilding == null) {
                setup = false;
            }
        }

        return setup;
    }

    private void Start() {
        if (SetupQueenBee()) {
            _agent.destination = _queenBeeBuilding.transform.position;
            targetsInRange.Add(_queenBeeBuilding.transform);
        }
        UpdateTarget();
    }


    void FixedUpdate() {
        if (NearTarget()) {
            if (AttackDistance()) {
                if (actualTimer <= 0) {
                    Health targetHealth = _currentTarget.GetComponent<Health>();
                    targetHealth.ModifyHealth(-waspDamage);
                    if (targetHealth.CurrentHealth <= 0) {
                        targetsInRange.Remove(_currentTarget);
                        UpdateTarget();
                    }
                    actualTimer = attacktimer;
                }
            }
        }

        if (actualTimer >= 0) {
            actualTimer -= Time.deltaTime;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 7f, mask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != null && (hitCollider.CompareTag("Bee") || hitCollider.CompareTag("Building")))
            {
                if (hitCollider.GetComponent<Health>().CurrentHealth > 0)
                {
                    targetsInRange.Add(hitCollider.transform);
                    UpdateTarget();
                }
            }
        }

        targetsInRange.RemoveAll(x => Vector3.Distance(transform.position, x.position) > 7.1f);
    }

    public void TakeDamage(float damage) {
        health.ModifyHealth(-damage);
    }

    private void UpdateTarget() {
        _currentTarget = GetClosestTarget();
        if (_currentTarget != null) {
            _agent.destination = _currentTarget.position;
        }
    }

    private Transform GetClosestTarget() {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        
        // prioritise queen bee if close
        if (Vector3.Distance(_queenBeeBuilding.transform.position, transform.position) < 5f) {
            closestTarget = _queenBeeBuilding.transform;
        } else {
            foreach (Transform t in targetsInRange) {
                if (t != null) {
                    if (Vector3.Distance(transform.position, t.position) < closestDistance) {
                        closestTarget = t;
                    }
                }
            }
        }

        return closestTarget;
    }

    private void OnTriggerExit(Collider other) {
        if (other!= null && other.name != "QueenBeeBuilding(Clone)" &&
            (other.CompareTag("Bee") || other.CompareTag("Building"))) {
            targetsInRange.Remove(other.transform);
            UpdateTarget();
        }
    }

    private bool NearTarget() {
        bool nearTarget = false;
        if (targetsInRange.Count > 0) {
            if (targetsInRange[0] == null || targetsInRange[0].GetComponent<Health>().CurrentHealth <= 0) {
                targetsInRange.RemoveAt(0);
            } else {
                nearTarget = true;
            }
        }

        return nearTarget;
    }

    private bool AttackDistance() {
        return _currentTarget != null && Vector3.Distance(_currentTarget.position, transform.position) < attackRange;
    }
}
