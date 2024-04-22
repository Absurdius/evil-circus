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

    NavMeshAgent navMeshAgent;

    Vector3 lastKnowPosition;
    public float searchTime;
    float remainingSearchTime;

    public float normalSpeed;
    public float chaseSpeed;



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

        
        if (Vector3.Distance(transform.position, playerTarget.position) < detectionRange && !playerMovement.isCrouching && playerMovement.isMoving)
        {
            currentState = EnemyState.Chase;
        }


    }

    private void Chase()
    {
        navMeshAgent.speed = chaseSpeed;

        navMeshAgent.destination = playerTarget.position;

        if (Vector3.Distance(transform.position, playerTarget.position) > detectionRange || playerMovement.isCrouching || !playerMovement.isMoving)
        {
            lastKnowPosition = playerTarget.position;
            currentState = EnemyState.Search;
        }
    }

    private void Search()
    {
        navMeshAgent.speed = normalSpeed;

        navMeshAgent.destination = lastKnowPosition;

        if (Vector3.Distance(transform.position, lastKnowPosition) < 1f)
        {
            remainingSearchTime -= Time.deltaTime;

            if (remainingSearchTime <= 0f)
            {
                currentState = EnemyState.Patrol;
            }
        }
        else
        {
            remainingSearchTime = searchTime;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) < detectionRange && !playerMovement.isCrouching && playerMovement.isMoving)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void Attack()
    {
        return;
    }
}
