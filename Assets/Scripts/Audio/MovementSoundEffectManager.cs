using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MovementSoundEffectManager : MonoBehaviour
{
   private AudioSource audioSource;
   public AudioClip[] clips;
    private Rigidbody rb;
    private readonly float threshold = 0.7f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Run")){
            PlayRunningSound();
        } else if (Input.GetButtonUp("Run") && rb.velocity.magnitude > threshold)
        {
            audioSource.Stop();
            PlayWalkingSound();
        } else if (rb.velocity.magnitude > threshold && !audioSource.isPlaying) 
        {
            PlayWalkingSound();
        } else if (rb.velocity.magnitude < threshold) {
            audioSource.Stop();
        }
    }

    private void PlayRunningSound()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }

    private void PlayWalkingSound()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
    }
}
