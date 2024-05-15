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
    public int detectionTime;

    public GameObject eyes;
    public SpotLight eyeLight;

    [Header("Chase")]
    public int chaseTime;

    //[Header("Attack")]

    [Header("Misc")]
    bool stateChanged;
    bool coRoutineRunning;

    Transform playerTarget;
    NavMeshAgent navMeshAgent;
    GapingAudio gapingAudio;

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
        gapingAudio = GetComponentInChildren<GapingAudio>();

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

            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Idle);

            stateChanged = false;
            var patrolTime = Random.Range(scanCooldownMin, scanCooldownMax);
            StartCoroutine(ChangeState(patrolTime, TheGapingState.Scan, false));
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
            StartCoroutine(ChangeState(scanTime, TheGapingState.Patrol, false));
        }

        navMeshAgent.destination = transform.position;


        if (VisionCheck() && !coRoutineRunning)
        {
            StartCoroutine(ChangeState(detectionTime, TheGapingState.Chase, true));
            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Detected);
        }
        else if (!VisionCheck() && coRoutineRunning)
        {
            StopCoroutine(ChangeState(detectionTime, TheGapingState.Chase, true));
            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Idle);
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

            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Chasing);
            stateChanged = false;
        }

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.stoppingDistance = 5;
        navMeshAgent.destination = playerTarget.position;

        if (!VisionCheck())
        {
            StartCoroutine(ChangeState(chaseTime, TheGapingState.Scan, false));
            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Idle);
        }
        else if (VisionCheck())
        {
            StopAllCoroutines();
            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Chasing);
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

            gapingAudio.ChangeAudioSate(GapingAudio.AudioState.Attacking);
            stateChanged = false;
        }

        return;
    }

    // The flag bool is used to help indentify when a specific coroutine is running. Only one active flag works at a time
    private IEnumerator ChangeState(int waitTime, TheGapingState newState, bool flag)
    {
        if (flag)
        {
            coRoutineRunning = true;
        }

        if (stateDebug)
        {
            Debug.Log("Trying to change state");
        }

        yield return new WaitForSeconds(waitTime);
        currentState = newState;
        stateChanged = true;

        coRoutineRunning = false;
        StopAllCoroutines();
    }
}
