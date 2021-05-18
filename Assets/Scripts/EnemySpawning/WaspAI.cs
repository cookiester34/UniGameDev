using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.AI;
using Pathfinding;

[RequireComponent(typeof(Health))]
public class WaspAI : MonoBehaviour
{
    public LayerMask mask;

    private GameObject _queenBeeBuilding;
    private GameObject _currentTarget;
    private Vector3 _previousDestination = Vector3.positiveInfinity;

    //nav
    //private NavMeshAgent _agent;
    IAstarAI _ai;

    //wasp Stats
    Health health;
    [Range(1, 10)]
    public int waspDamage = 1;
    [Range(1, 10)]
    public float attackRange = 5;
    [Range(0, 10)]
    public float detectionRange = 5f;
    private float oldDetectionRange;
    [Range(1, 5)]
    public float attacktimer = 2;
    private float actualTimer;
    
    private Collider[] sphereAlloc = new Collider[100];
    public int WaspGroupID;

    public EnemySpawnManager spawnManager;

    private List<Renderer> waspRenderers;

    private float _soundLoopBasePitch;
    private float _soundLoopVolume;

    public Animator animator;
    private static readonly int Attack = Animator.StringToHash("Attack");

    void Awake() 
    {
        waspRenderers = GetComponentsInChildren<Renderer>().ToList();
        health = GetComponentInParent<Health>();
        _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        //_agent = GetComponent<NavMeshAgent>();
        _ai = GetComponent<IAstarAI>();
        actualTimer = attacktimer;
        _soundLoopBasePitch = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        _soundLoopVolume = 0.7f + UnityEngine.Random.Range(-.05f, 0.05f);
        transform.GetComponent<AudioSource>().volume = _soundLoopVolume;
        transform.GetComponent<AudioSource>().pitch = _soundLoopBasePitch;
        oldDetectionRange = detectionRange;
        
        GetComponent<AIPath>().maxSpeed = Random.Range(2f, 2.4f);
    }

    private void SetupQueenBee()
    {
        if (_queenBeeBuilding == null) {
            _queenBeeBuilding = BeeManager.Instance.gameObject;
        }
    }

    private void Start() {
        SetupQueenBee();
    }


    void FixedUpdate() {
        bool visible = FogOfWarBounds.instance.IsVisible(transform.position);
        foreach (Renderer renderer1 in waspRenderers) {
            renderer1.enabled = visible;
        }
        
        sphereAlloc = Physics.OverlapSphere(transform.position, detectionRange, mask);
        if (sphereAlloc.Length > 0) {
            float closestDist = float.MaxValue;
            GameObject targetObject = null;
            foreach (Collider hitCollider in sphereAlloc) {
                Vector3 waspPos = transform.position;
                waspPos.y = 0;
                Vector3 targetPos = hitCollider.transform.position;
                targetPos.y = 0;
                float currentDistance = FastMath.SqrDistance(waspPos, targetPos);
                if (hitCollider.CompareTag("Bee") || hitCollider.CompareTag("Building")
                    && currentDistance < closestDist) {
                    if (hitCollider.gameObject.GetComponent<EnemyBuilding>() == null) {
                        targetObject = hitCollider.gameObject;
                        closestDist = currentDistance;
                    }
                }
            }

            if (targetObject != null && targetObject.transform != null) {
                SetDestination(targetObject);
            } else {
                SetDestination(_queenBeeBuilding);
            }
        } else {
            SetDestination(_queenBeeBuilding);
        }
        if (AttackDistance() && actualTimer <= 0) {
            animator.SetTrigger(Attack);
            Health targetHealth = _currentTarget.GetComponent<Health>();
            targetHealth.ModifyHealth(-waspDamage);
            actualTimer = attacktimer;
        }
        actualTimer -= Time.deltaTime;
        if (transform.position.y < 0)
        {
            Vector3 resetHeight = new Vector3(transform.position.x, 1, transform.position.z);
            transform.position = resetHeight;
        }
    }

    void SetDestination(GameObject destinationObject) 
    {
        if (destinationObject != null)
        {
            Vector3 destination = destinationObject.transform.position;
            //_agent.destination = destination;
            _ai.destination = destination;
            _previousDestination = destination;
            _currentTarget = destinationObject;
        }
    }

    public void TakeDamage(float damage) 
    {
        health.ModifyHealth(-damage);
    }

    private bool AttackDistance() {
        if (_currentTarget == null) {
            return false;
        }

        Vector3 pos1 = _currentTarget.transform.position;
        pos1.y = 0;
        Vector3 pos2 = transform.position;
        pos2.y = 0;
        return FastMath.SqrDistance(pos1, pos2) < attackRange * attackRange;
    }

    private void OnDestroy()
    {
        if(transform != null)
            spawnManager.waspGroupList[WaspGroupID].wasps.Remove(this.transform);
    }
}
