using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class NewEnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Stunned,
        Patrol,
        Chase,
        Search,
        Attack
    }

    public EnemyState currentState = EnemyState.Patrol;
    public EnemyState LastState;


    [Header("Stats")]
    public bool canBeStunned;
    public float patrolSpeed;
    public float chaseSpeed;
    public float normalVisionAngle;
    public float visionRange;
    private float visionAngle;

    [Header("Stun")]
    public int stunTime;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    Transform patrolTarget;

    [Header("Chase")]
    public int chaseTime;

    [Header("Search")]
    private Vector3 lastKnownPosition;
    public int searchTime;
    public float searchVisionAngle;

    [Header("Attack")]
    public float attackRange;
    private EnemyAttack enemyAttack;

    [Header("Misc")]
    bool haveVision;
    bool stateChanged;

    Transform playerTarget;
    Transform playerTargetVision;
    public Transform detectionSource;
    NavMeshAgent navMeshAgent;

    [Header("Debug")]
    public bool patrolDebug;
    public bool chaseDebug;
    public bool attackDebug;
    public bool stateDebug;
    public bool DetectionDebug;

    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        playerTargetVision = GameObject.Find("PlayerCamera").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();

        currentPatrolPoint = 0;
        patrolTarget = patrolPoints[currentPatrolPoint];

        visionAngle = normalVisionAngle;
        stateChanged = true;
    }


    void Update()
    {
        if (stateDebug)
        {
            Debug.Log("My current state is " + currentState);
        }

        switch (currentState)
        {
            case EnemyState.Stunned:
                Stunned();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Search:
                Search();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        var relativePos = playerTargetVision.position - detectionSource.position;
        var fwd = transform.forward;
        var angle = Vector3.Angle(relativePos, fwd);

        if (angle < visionAngle)
        {
            if (DetectionDebug)
            {
                Debug.Log("Player is within sight");
            }

            RaycastHit hit;
            if (Physics.Raycast(detectionSource.position, relativePos, out hit, visionRange))
            {
                if (hit.collider.transform.gameObject.layer == 8)
                {
                    if (DetectionDebug)
                    {
                        Debug.Log("I saw the player");
                        Debug.DrawRay(detectionSource.position, relativePos, Color.red, 1f);
                    }

                    haveVision = true; ;
                }
                else
                {
                    if (DetectionDebug)
                    {
                        Debug.Log("I didn't see the player");
                        Debug.DrawRay(detectionSource.position, relativePos, Color.white, 1f);
                    }

                    haveVision = false;
                }
            }
        }
        else
        {
            haveVision = false;
        }
    }

    public void HearPlayer()
    {
        if (DetectionDebug)
        {
            Debug.Log("I heard the player");
        }

        if (currentState != EnemyState.Stunned)
        {
            StartCoroutine(ChangeState(0, EnemyState.Chase));
        }
    }

    private void Stunned()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am stunned");
            }

            if (enemyAttack.isAttacking)
            {
                enemyAttack.StopAttacking();
            }

            if (LastState != EnemyState.Stunned)
            {
                StartCoroutine(ChangeState(stunTime, LastState));
            }
            else
            {
                StartCoroutine(ChangeState(stunTime, EnemyState.Patrol));
            }
            stateChanged = false;
        }
    }

    private void Patrol()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am patrolling");
            }

            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.stoppingDistance = 0;

            stateChanged = false;
        }

        navMeshAgent.destination = patrolTarget.position;

        if (Vector3.Distance(transform.position, patrolTarget.position) < 1f)
        {
            if (patrolDebug)
            {
                Debug.Log("I am at my target. Index " + currentPatrolPoint);
            }

            currentPatrolPoint++;
            if (currentPatrolPoint == patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }

            if (patrolDebug)
            {
                Debug.Log("My new target index is " + currentPatrolPoint);
            }

            patrolTarget = patrolPoints[currentPatrolPoint];
        }

        if (haveVision)
        {
            StartCoroutine(ChangeState(0, EnemyState.Chase));
        }
    }

    private void Chase()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am chasing");
            }

            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.stoppingDistance = 1.75f;

            stateChanged = false;
        }

        navMeshAgent.destination = playerTarget.position;

        if (!haveVision)
        {
            StartCoroutine(ChangeState(chaseTime, EnemyState.Search));
        }
        else if (haveVision && Vector3.Distance(transform.position, playerTarget.position) <= attackRange)
        {
            StartCoroutine(ChangeState(0, EnemyState.Attack));
        }
        else if (haveVision)
        {
            StopAllCoroutines();
        }
    }

    private void Search()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am searching");
            }

            visionAngle = searchVisionAngle;

            lastKnownPosition = playerTarget.position;

            navMeshAgent.destination = lastKnownPosition;
            navMeshAgent.stoppingDistance = 0;

            StartCoroutine(ChangeState(searchTime, EnemyState.Patrol));

            stateChanged = false;
        }

        if (haveVision)
        {
            StartCoroutine(ChangeState(0, EnemyState.Chase));
        }
    }

    private void Attack()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am attacking");
            }

            navMeshAgent.destination = transform.position;

            stateChanged = false;
        }

        enemyAttack.Attack();

        if (!haveVision || Vector3.Distance(transform.position, playerTarget.position) > attackRange)
        {
            if (!enemyAttack.isAttacking)
            {
                StartCoroutine(ChangeState(0, LastState));
            }
        }
    }

    // The flag bool is used to help indentify when a specific coroutine is running. Only one active flag works at a time
    public IEnumerator ChangeState(int waitTime, EnemyState newState)
    {
        if (newState == currentState)
        {
            StopAllCoroutines();
        }

        if (stateDebug)
        {
            Debug.Log("Trying to change state");
        }

        yield return new WaitForSeconds(waitTime);
        LastState = currentState;
        if (LastState == EnemyState.Search)
        {
            visionAngle = normalVisionAngle;
        }
        currentState = newState;
        stateChanged = true;


        StopAllCoroutines();
    }
}

