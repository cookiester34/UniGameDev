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

    public bool masterWasp = false;
    public Transform masterWaspObject;
    public int WaspGroupID;
    public WaspAI masterWaspAI;

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
        if (_queenBeeBuilding == null) 
        {
            _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        }
    }

    private void Start() 
    {
        SetupQueenBee();

    }


    void FixedUpdate() {
        bool visible = FogOfWarBounds.instance.IsVisible(transform.position);
        foreach (Renderer renderer1 in waspRenderers) {
            renderer1.enabled = visible;
        }

        if (masterWasp)
        {
            sphereAlloc = Physics.OverlapSphere(transform.position, detectionRange, mask);
            if (sphereAlloc.Length > 0)
            {
                float closestDist = float.MaxValue;
                GameObject targetObject = null;
                foreach (Collider hitCollider in sphereAlloc)
                {
                    float currentDistance = FastMath.SqrDistance(transform.position, hitCollider.transform.position);
                    if (hitCollider.CompareTag("Bee") || hitCollider.CompareTag("Building")
                        && currentDistance < closestDist)
                    {
                        targetObject = hitCollider.gameObject;
                        closestDist = currentDistance;
                    }
                }

                if (targetObject != null && targetObject.transform != null)
                {
                    SetDestination(targetObject, false);
                }
            }
            else
            {
                if(_queenBeeBuilding != null)
                    SetDestination(_queenBeeBuilding, true);
                else
                    SetupQueenBee();
            }
        }
        else
        {
            if(masterWaspAI._currentTarget != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, masterWaspAI._currentTarget.transform.position, 0.05f);
                transform.LookAt(masterWaspAI._currentTarget.transform.position);
                transform.Rotate(new Vector3(0, -90, 0));
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                _currentTarget = masterWaspAI._currentTarget;
            }
            else
            {
                followMaster();
            }
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

    void followMaster()
    {
        if (masterWaspObject != null)
        {
            if (FastMath.SqrDistance(transform.position, masterWaspObject.position) > 9f)
            {
                transform.position = Vector3.MoveTowards(transform.position, masterWaspObject.position, 0.05f);
                transform.LookAt(masterWaspObject);
            }
        }
        else
        {
            masterWaspObject = spawnManager.waspGroupList[WaspGroupID].wasps[0];
            if (masterWaspObject == this.transform)
                masterWasp = true;
            else
                masterWaspAI = masterWaspObject.GetComponent<WaspAI>();
        }
    }

    void SetDestination(GameObject destinationObject, bool isQueen) 
    {
        if (destinationObject != null)
        {
            Vector3 destination = destinationObject.transform.position;
            if (destination != _previousDestination)
            {
                //_agent.destination = destination;
                _ai.destination = destination;
                _ai.SearchPath();
                _previousDestination = destination;
                if (!isQueen)
                    _currentTarget = destinationObject;
                else
                    _currentTarget = null;
            }
        }
    }

    public void TakeDamage(float damage) 
    {
        health.ModifyHealth(-damage);
    }

    private bool AttackDistance() 
    {
        return _currentTarget != null && FastMath.SqrDistance(_currentTarget.transform.position, transform.position) < attackRange * attackRange;
    }

    private void OnDestroy()
    {
        if(transform != null)
            spawnManager.waspGroupList[WaspGroupID].wasps.Remove(this.transform);
    }
}
