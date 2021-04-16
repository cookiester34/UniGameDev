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

    public bool masterWasp = false;
    public Transform masterWaspObject;
    public int WaspGroupID;
    public WaspAI masterWaspAI;

    public EnemySpawnManager spawnManager;

    private Renderer waspRenderer;

    private float _soundLoopBasePitch;
    private float _soundLoopVolume;

    void Awake() 
    {
        waspRenderer = GetComponentInChildren<Renderer>();
        health = GetComponentInParent<Health>();
        _queenBeeBuilding = GameObject.Find("QueenBeeBuilding(Clone)");
        _agent = GetComponent<NavMeshAgent>();
        actualTimer = attacktimer;
        _soundLoopBasePitch = 1f + UnityEngine.Random.Range(-.05f, 0.05f);
        _soundLoopVolume = 0.7f + UnityEngine.Random.Range(-.05f, 0.05f);
        transform.GetComponent<AudioSource>().volume = _soundLoopVolume;
        transform.GetComponent<AudioSource>().pitch = _soundLoopBasePitch;
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


    void FixedUpdate() 
    {
        if (FogOfWarBounds.instance.IsWaspVisible(transform.position))
        {
            waspRenderer.enabled = true;
        }
        else
        {
            waspRenderer.enabled = false;
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
                    //if (masterWasp)
                        SetDestination(targetObject);
                    //else
                    //{
                    //    transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, 0.05f);
                    //    transform.LookAt(targetObject.transform.position);
                    //    _currentTarget = targetObject;
                    //}
                }
            }
            else
            {
                //if (masterWasp)
                    SetDestination(_queenBeeBuilding);
                //else
                //    followMaster();
            }
        }
        else
        {
            if(masterWaspAI._currentTarget != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, masterWaspAI._currentTarget.transform.position, 0.05f);
                transform.LookAt(masterWaspAI._currentTarget.transform.position);
                _currentTarget = masterWaspAI._currentTarget;
            }
            else
            {
                followMaster();
            }
        }
        if (AttackDistance() && actualTimer <= 0) 
        {
            Health targetHealth = _currentTarget.GetComponent<Health>();
            targetHealth.ModifyHealth(-waspDamage);
            actualTimer = attacktimer;
        }
        actualTimer -= Time.deltaTime;
    }

    void followMaster()
    {
        if (masterWaspObject != null)
        {
            if (FastMath.SqrDistance(transform.position, masterWaspObject.position) > 4f)
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

    void SetDestination(GameObject destinationObject) 
    {
        if (destinationObject != null)
        {
            Vector3 destination = destinationObject.transform.position;
            if (destination != _previousDestination)
            {
                _agent.destination = destination;
                _previousDestination = destination;
                _currentTarget = destinationObject;
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
