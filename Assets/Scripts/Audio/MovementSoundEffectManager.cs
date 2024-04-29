using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MovementSoundEffectManager : MonoBehaviour
{
   private AudioSource audioSource;
   public AudioClip[] clips;
    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Run")){
            PlayRunningSound();
        } else if (Input.GetButtonUp("Run"))
        {
            audioSource.Stop();
        }

    }

    private void PlayRunningSound()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }
}
