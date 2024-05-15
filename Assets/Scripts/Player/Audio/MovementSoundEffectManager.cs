using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MovementSoundEffectManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] clips;
    //private Rigidbody rb;
    //private readonly float threshold = 0.7f;
    public PlayerMovement playerMovement;
    //public UIStateManager stateManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (UIStateManager.currentState == UIStateManager.UIState.PLAYING)
        {
            // rb.magnitude takes yVel into account and doesn't work for walking sound
            if (playerMovement.isMoving && playerMovement.grounded)
            {
                if (playerMovement.isRunning)
                {
                    PlayRunningSound();
                }
                else
                {
                    PlayWalkingSound();
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

    private void PlayRunningSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != clips[0])
        {
            audioSource.clip = clips[0];
            audioSource.Play();
        }
    }

    private void PlayWalkingSound()
    {   if (!audioSource.isPlaying || audioSource.clip != clips[1])
        {
            audioSource.clip = clips[1];
            audioSource.Play();
        }
        
    }
}
