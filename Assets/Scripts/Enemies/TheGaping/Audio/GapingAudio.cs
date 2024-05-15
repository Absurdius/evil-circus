using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;

public class GapingAudio : MonoBehaviour
{
    public enum AudioState
    {
        Idle,
        Detected,
        Chasing,
        Attacking
    }
    public AudioState currentState = AudioState.Idle;

    GapingController controller;
    NavMeshAgent agent;
    AudioSource[] audioSources;
    public AudioClip[] clipsFootsteps;
    public AudioClip[] clipsLaughter;
    private int newClip;
    public AudioClip clipDetected;

    private bool stateChanged;
    public int minLaughterCooldown;
    public int maxLaughterCooldown;
    private float laughterCooldown;
    private bool preparingToLaugh;
    public bool isPaused = false;

    void Start()
    {
        controller =GetComponentInParent<GapingController>();
        agent = GetComponentInParent<NavMeshAgent>();
        audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (!isPaused)
        {
            switch (currentState)
            {
                case AudioState.Idle:
                    Idle();
                    break;
                case AudioState.Detected:
                    Detected();
                    break;
                case AudioState.Chasing:
                    Chasing();
                    break;
                case AudioState.Attacking:
                    Attacking();
                    break;
                default:
                    break;
            }
            HandleFootseps();
        }
        HandlePause();
    }

    private void HandleFootseps()
    {
        if (agent.velocity.magnitude > 0)
        {
            if (agent.speed == controller.chaseSpeed && (audioSources[0].clip != clipsFootsteps[0] || !audioSources[0].isPlaying))
            {
                audioSources[0].clip = clipsFootsteps[0];
                audioSources[0].Play();
            }
            else if (agent.speed == controller.patrolSpeed && (audioSources[0].clip != clipsFootsteps[1] || !audioSources[0].isPlaying))
            {
                audioSources[0].clip = clipsFootsteps[1];
                audioSources[0].Play();
            }
            return;
        }
        audioSources[0].Stop();
    }

    private void HandlePause()
    {
        if (UIStateManager.currentState == UIStateManager.UIState.PAUSED && !isPaused)
        {
            isPaused = true;
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Pause();
            }

            Debug.Log("I am paused");
        }
        else if (UIStateManager.currentState == UIStateManager.UIState.PLAYING && isPaused)
        {
            isPaused = false;
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Play();
            }

            Debug.Log("I am playing");
        }
    }


    public void ChangeAudioSate(AudioState newState)
    {
        currentState = newState;
        stateChanged = true;
    }

    private void Idle()
    {
        if (stateChanged)
        {
            stateChanged = false;
        }
        if (!audioSources[2].isPlaying)
        {
            audioSources[2].Play();
        }
        if (audioSources[1].isPlaying)
        {
            audioSources[1].Stop();
        }
        
    }

    private void Detected()
    {
        if (stateChanged)
        {
            audioSources[2].Stop();
            audioSources[1].clip = clipDetected;
            audioSources[1].Play();
            stateChanged = false;
        }
    }

    private void Chasing()
    {
        if (stateChanged)
        {
            if (!audioSources[2].isPlaying)
            {
                audioSources[2].Play();
            }
            audioSources[1].clip = clipsLaughter[1];
            audioSources[1].Play();
            stateChanged = false;
        }

        if (!audioSources[1].isPlaying)
        {
            if (!preparingToLaugh)
            {
                if (!audioSources[2].isPlaying)
                {
                    audioSources[2].Play();
                }
                newClip = Random.Range(0, clipsLaughter.Length - 1);
                audioSources[1].clip = clipsLaughter[newClip];
                laughterCooldown = Random.Range(minLaughterCooldown, maxLaughterCooldown);
                preparingToLaugh = true;
            }
            
            laughterCooldown -= Time.deltaTime;
            if (laughterCooldown <= 0)
            {
                audioSources[2].Stop();
                audioSources[1].Play();
                preparingToLaugh = false;
            }
        }
    }

    private void Attacking()
    {
        if (stateChanged)
        {
            audioSources[2].Stop();
            audioSources[1].clip = clipDetected;
            audioSources[1].Play();
            stateChanged = false;
        }
    }
}
