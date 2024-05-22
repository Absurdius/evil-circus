using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Search,
        Attack
    }

    EnemyState currentState = EnemyState.Patrol;

    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    Transform patrolTarget;

    public float detectionRangeRunning;
    public float detectionRangeWalking;
    public float detectionRangeCrouching;

    Transform playerTarget;
    PlayerMovement playerMovement;
    public float chaseTime;
    float remainingChaseTime;
    bool isChasing;

    NavMeshAgent navMeshAgent;

    Vector3 lastKnowPosition;
    public float searchTime;
    float remainingSearchTime;
    bool isSearching;

    public float normalSpeed;
    public float chaseSpeed;

    public float attackRange;
    public EnemyAttack enemyAttack;
    //public PlayerDeath playerDeath;

    public bool isStunned;
    public float stunTime;
    float remainingStunTime;



    private void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = normalSpeed;

        patrolTarget = patrolPoints[currentPatrolPoint];

        isStunned = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
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

        SwitchState();
    }

    private void SwitchState()
    {
        if (isStunned)
        {
            if (currentState != EnemyState.Idle)
            {
                remainingStunTime = stunTime;

                if (enemyAttack.isAttacking)
                {
                    enemyAttack.StunCancel();
                }
            }
            currentState = EnemyState.Idle;

            Debug.Log("I am stunned");
        }

        else if (Vector3.Distance(transform.position, playerTarget.position) < attackRange && currentState != EnemyState.Attack)
        {
            currentState = EnemyState.Attack;

            Debug.Log("I am attacking");
        }

        else if (DetectionCheck() && currentState != EnemyState.Chase)
        {
            isChasing = true;
            currentState = EnemyState.Chase;

            Debug.Log("I am chasing");
        }

        else if (currentState == EnemyState.Chase && !isChasing && currentState != EnemyState.Search)
        {
            lastKnowPosition = playerTarget.position;
            remainingSearchTime = searchTime;
            currentState = EnemyState.Search;

            Debug.Log("I am searching");
        }

        else if (((currentState == EnemyState.Search && !isSearching) || (currentState == EnemyState.Attack && Vector3.Distance(transform.position, playerTarget.position) > attackRange) || (currentState == EnemyState.Idle && !isStunned)) && currentState != EnemyState.Patrol)
        {
            currentState = EnemyState.Patrol;

            Debug.Log("I am patrolling");
        }
    }

    private bool DetectionCheck()
    {
        if (playerMovement.isMoving)
        {
            if (Vector3.Distance(transform.position, playerTarget.position) < detectionRangeRunning && playerMovement.isRunning)
            {
                return true;
            }
            else if (Vector3.Distance(transform.position, playerTarget.position) < detectionRangeWalking && !playerMovement.isCrouching)
            {
                return true;
            }
            else if (Vector3.Distance(transform.position, playerTarget.position) < detectionRangeCrouching)
            {
                return true;
            }
        }
        return false;
    }

    private void Idle()
    {
        //navMeshAgent.speed = 0;
        navMeshAgent.isStopped = true;

        if (remainingStunTime <= 0)
        {
            isStunned = false;
            navMeshAgent.isStopped = false;
        }
        else
        {
            remainingStunTime -= Time.deltaTime;
        }
    }

    private void Patrol()
    {
        navMeshAgent.speed = normalSpeed;

        navMeshAgent.destination = patrolTarget.position;

        if (Vector3.Distance(transform.position, patrolTarget.position) < 1f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint == patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
            patrolTarget = patrolPoints[currentPatrolPoint];
        }
    }

    private void Chase()
    {
        navMeshAgent.speed = chaseSpeed;

        navMeshAgent.destination = playerTarget.position;

        if (!DetectionCheck())
        {
            remainingChaseTime -= Time.deltaTime;

            if (remainingChaseTime <= 0)
            {
                isChasing = false;
            }
        }
        else
        {
            remainingChaseTime = chaseTime;
        }
    }

    private void Search()
    {
        isSearching = true;

        navMeshAgent.destination = lastKnowPosition;

        if (Vector3.Distance(transform.position, lastKnowPosition) < 1f)
        {
            remainingSearchTime -= Time.deltaTime;

            if (remainingSearchTime <= 0f)
            {
                isSearching = false;
            }
        }
    }

    private void Attack()
    {
        navMeshAgent.speed = 0.1f;

        var relativePos = playerTarget.position - transform.position;
        var fwd = transform.forward;
        var angle = Vector3.Angle(relativePos, fwd);

        if (angle < 45f)
        {
            navMeshAgent.destination = transform.position;
            enemyAttack.Attack();
        }
        else
        {
            navMeshAgent.destination = playerTarget.position;
        }
    }

    public void Stun()
    {
        isStunned = true;
    }

    public EnemyState GetEnemyState()
    {
        return currentState;
    }
}
