using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Stunned,
        Patrol,
        Chase,
        Attack
    }

    public EnemyState currentState = EnemyState.Patrol;
    public EnemyState LastState;
    

    [Header("Stats")]
    public bool canBeStunned;
    public float patrolSpeed;
    public float chaseSpeed;
    public float visionAngle;
    public float visionRange;

    [Header("Stun")]
    public int stunTime;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    Transform patrolTarget;

    [Header("Chase")]
    public int chaseTime;

    [Header("Attack")]
    public float attackRange;
    private EnemyAttack enemyAttack;

    [Header("Misc")]
    bool stateChanged;
    bool coRoutineRunning;

    Transform playerTarget;
    NavMeshAgent navMeshAgent;

    [Header("Debug")]
    public bool patrolDebug;
    public bool chaseDebug;
    public bool attackDebug;
    public bool stateDebug;
    public bool visionDebug;

    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();

        currentPatrolPoint = 0;
        patrolTarget = patrolPoints[currentPatrolPoint];

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
            case EnemyState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private bool VisionCheck()
    {
        var relativePos = playerTarget.position - transform.position;
        var fwd = transform.forward;
        var angle = Vector3.Angle(relativePos, fwd);

        if (angle < visionAngle)
        {
            if (visionDebug)
            {
                Debug.Log("Player is within sight");
            }

            if (Physics.Raycast(transform.position, relativePos, visionRange, 1 << 8))
            {
                if (visionDebug)
                {
                    Debug.Log("I saw the player");
                }

                return true;
            }
            else
            {
                if (visionDebug)
                {
                    Debug.Log("I didn't see the player");
                }
            }
        }
        return false;
    }

    public void HearPlayer()
    {
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

            StartCoroutine(ChangeState(stunTime, LastState));
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

            stateChanged = false;
        }

        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.stoppingDistance = 0;
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

        if (VisionCheck())
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

            stateChanged = false;
        }

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.stoppingDistance = 2f;
        navMeshAgent.destination = playerTarget.position;

        if (!VisionCheck())
        {
            StartCoroutine(ChangeState(chaseTime, EnemyState.Patrol));
        }
        else if (VisionCheck() && Vector3.Distance(transform.position, playerTarget.position) < attackRange)
        {
            StartCoroutine(ChangeState(0, EnemyState.Attack));
        }
        else if (VisionCheck())
        {
            StopAllCoroutines();
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

            stateChanged = false;
        }

        enemyAttack.Attack();

        if (!VisionCheck() || Vector3.Distance(transform.position, playerTarget.position) > attackRange)
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
        currentState = newState;
        stateChanged = true;


        StopAllCoroutines();
    }
}
