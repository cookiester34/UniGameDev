using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class WaspAI : MonoBehaviour
{
    public LayerMask mask;

    private GameObject _queenBeeBuilding;
    private GameObject _currentTarget;
    private Vector3 _previousDestination = Vector3.positiveInfinity;

    //nav
    private NavMeshAgent _agent;

    //wasp Stats
    Health health;
    [Range(1, 10)]
    public int waspDamage = 1;
    [Range(1, 10)]
    public float attackRange = 5;
    [Range(10, 50)]
    public float detectionRange = 5f;
    [Range(1, 5)]
    public float attacktimer = 2;
    private float actualTimer;
    
    private Collider[] sphereAlloc = new Collider[100];

    void Awake() {
        health = GetComponentInParent<Health>();
        _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        _agent = GetComponent<NavMeshAgent>();
        actualTimer = attacktimer;
    }

    private void SetupQueenBee() {
        if (_queenBeeBuilding == null) {
            _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        }
    }

    private void Start() {
        SetupQueenBee();
    }


    void FixedUpdate() {
        var colliders = Physics.OverlapSphere(transform.position, detectionRange, mask);
        if (colliders.Length > 0) {
            float closestDist = float.MaxValue;
            GameObject targetObject = null;
            foreach (Collider hitCollider in colliders) {
                float currentDistance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (hitCollider.CompareTag("Bee") || hitCollider.CompareTag("Building")
                    && currentDistance < closestDist) {
                    targetObject = hitCollider.gameObject;
                    closestDist = currentDistance;
                }
            }

            if (targetObject != null && targetObject.transform != null) {
                SetDestination(targetObject);
            }
        } else {
            SetDestination(_queenBeeBuilding);
        }

        if (AttackDistance() && actualTimer <= 0) {
            Health targetHealth = _currentTarget.GetComponent<Health>();
            targetHealth.ModifyHealth(-waspDamage);
            if (targetHealth.CurrentHealth <= 0) {
            }
            actualTimer = attacktimer;
        }

        actualTimer -= Time.deltaTime;
    }

    void SetDestination(GameObject destinationObject) {
        Vector3 destination = destinationObject.transform.position;
        if (destination != _previousDestination) {
            _agent.destination = destination;
            _previousDestination = destination;
            _currentTarget = destinationObject;
        }
    }

    public void TakeDamage(float damage) {
        health.ModifyHealth(-damage);
    }

    private bool AttackDistance() {
        return _currentTarget != null && Vector3.Distance(_currentTarget.transform.position, transform.position) < attackRange;
    }
}
