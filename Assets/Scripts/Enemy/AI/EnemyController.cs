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
    
    EnemyState currentState = EnemyState.Idle;

    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    Transform patrolTarget;

    public float detectionRange;

    public Transform playerTarget;
    public PlayerMovement playerMovement;
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
    public PlayerDeath playerDeath;



    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = normalSpeed;

        patrolTarget = patrolPoints[currentPatrolPoint];
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                currentState = EnemyState.Patrol;
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
        if (Vector3.Distance(transform.position, playerTarget.position) < detectionRange && !playerMovement.isCrouching && playerMovement.isMoving && currentState != EnemyState.Chase)
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

        else if (((currentState == EnemyState.Search && !isSearching) || (currentState == EnemyState.Attack && Vector3.Distance(transform.position, playerTarget.position) > attackRange)) && currentState != EnemyState.Patrol)
        {
            currentState = EnemyState.Patrol;

            Debug.Log("I am patrolling");
        }

        else if (Vector3.Distance(transform.position, playerTarget.position) < attackRange && currentState != EnemyState.Attack)
        {
            currentState = EnemyState.Attack;

            Debug.Log("I am attacking");
        }
    }

    private void Patrol()
    {
        navMeshAgent.speed = normalSpeed;

        navMeshAgent.destination = patrolTarget.position;

        if (Vector3.Distance(transform.position, patrolTarget.position) < 1f)
        {
            if (currentPatrolPoint < patrolPoints.Length)
            {
                currentPatrolPoint++;
            }
            else
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

        if (Vector3.Distance(transform.position, playerTarget.position) > detectionRange || playerMovement.isCrouching || !playerMovement.isMoving)
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
        playerDeath.Death();
    }
}