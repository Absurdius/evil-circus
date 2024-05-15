using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;
    public NavMeshAgent navMeshAgent;
    //UIStateManager stateManager;

    public float runningThreshhold;

    void Start()
    {
        //stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Stops the sound when the game is paused
        if (UIStateManager.currentState == UIStateManager.UIState.PLAYING)
        {
            if (navMeshAgent.velocity.magnitude > runningThreshhold)
            {
                if (!audioSource.isPlaying || audioSource.clip != clips[1])
                {
                    audioSource.clip = clips[1];
                    audioSource.Play();
                }
            }
            else if (navMeshAgent.velocity.magnitude > 0f)
            {
                if (!audioSource.isPlaying || audioSource.clip != clips[0])
                {
                    audioSource.clip = clips[0];
                    audioSource.Play();
                }  
            }
            else
            {
                audioSource.Stop();
            }  
        }
        else
        {
            audioSource.Stop();
        }
    }
}
