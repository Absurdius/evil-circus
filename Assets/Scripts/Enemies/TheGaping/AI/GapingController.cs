using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class GapingController : MonoBehaviour
{
    private enum TheGapingState
    {
        Patrol,
        Scan,
        Chase,
        Attack
    }

    TheGapingState currentState = TheGapingState.Patrol;

    [Header("Stats")]
    public bool canBeStunned;
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    Transform patrolTarget;

    [Header("Scan")]
    public float ScanDistance;
    public float scanAngle;
    public int scanTimeMin;
    public int scanTimeMax;
    public int scanCooldownMin;
    public int scanCooldownMax;

    public GameObject eyes;
    public SpotLight eyeLight;

    [Header("Chase")]
    public int chaseTime;

    //[Header("Attack")]

    [Header("Misc")]
    bool stateChanged;

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
        //eyeLight = GetComponentInChildren<SpotLight>();
        playerTarget = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

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
            case TheGapingState.Patrol:
                Patrol();
                break;
            case TheGapingState.Scan:
                Scan();
                break;
            case TheGapingState.Chase:
                Chase();
                break;
            case TheGapingState.Attack:
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

        if (angle < scanAngle)
        {
            if (visionDebug)
            {
                Debug.Log("Player is within sight");
            }

            if (Physics.Raycast(transform.position, relativePos, ScanDistance, 1 << 8))
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

    private void Patrol()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am patrolling");
            }

            eyes.SetActive(false);

            stateChanged = false;
            var patrolTime = Random.Range(scanCooldownMin, scanCooldownMax);
            StartCoroutine(ChangeState(patrolTime, TheGapingState.Scan));
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
    }

    private void Scan()
    {
        if (stateChanged)
        {
            if (stateDebug)
            {
                Debug.Log("I am scanning;");
            }

            eyes.SetActive(true);

            stateChanged = false;
            var scanTime = Random.Range(scanTimeMin, scanTimeMax);
            StartCoroutine(ChangeState(scanTime, TheGapingState.Patrol));
        }

        navMeshAgent.destination = transform.position;


        if (VisionCheck())
        {
            StartCoroutine(ChangeState(0, TheGapingState.Chase));
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
        navMeshAgent.stoppingDistance = 5;
        navMeshAgent.destination = playerTarget.position;

        if (!VisionCheck())
        {
            StartCoroutine(ChangeState(chaseTime, TheGapingState.Scan));
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

        return;
    }

    private IEnumerator ChangeState(int waitTime, TheGapingState newState)
    {
        if (stateDebug)
        {
            Debug.Log("Trying to change state");
        }

        yield return new WaitForSeconds(waitTime);
        currentState = newState;
        stateChanged = true;

        StopAllCoroutines();
    }
}
