using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private bool isPaused = false;

    private NewPlayerController playerController;
    private CharacterController characterController;
    private AudioSource[] audioSources;
    public AudioClip[] footsteps;
    public AudioClip Panting;

    void Start()
    {
        playerController = GetComponentInParent<NewPlayerController>();
        characterController = GetComponentInParent<CharacterController>();
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    void Update()
    {
        // Handle pause
        if (UIStateManager.currentState == UIStateManager.UIState.PAUSED && !isPaused)
        {
            isPaused = true;
            foreach (var source in audioSources)
            {
                source.Pause();
            }
        }
        else if (UIStateManager.currentState == UIStateManager.UIState.PAUSED && isPaused)
        {
            isPaused = false;
            foreach (var source in audioSources)
            {
                source.Play();
            }
        }

        // Skip code if we're paused
        if (isPaused)
        {
            return;
        }

        // Handle footsteps
        if (playerController.currentState == NewPlayerController.MovementState.Running && characterController.velocity.magnitude > 0 && (!audioSources[0].isPlaying || audioSources[0].clip != footsteps[0]))
        {
            audioSources[0].clip = footsteps[0];
            audioSources[0].Play();
        }
        else if (playerController.currentState != NewPlayerController.MovementState.Running && playerController.currentState != NewPlayerController.MovementState.Airborne && characterController.velocity.magnitude > 0 && (!audioSources[0].isPlaying || audioSources[0].clip != footsteps[1]))
        {
            audioSources[0].clip = footsteps[1];
            audioSources[0].Play();
        }
        else if (playerController.currentState == NewPlayerController.MovementState.Airborne || characterController.velocity.magnitude < 0.1f)
        {
            audioSources[0].Stop();
        }

        // Handle panting
        if (playerController.currentState == NewPlayerController.MovementState.StaminaRecharge && !audioSources[1].isPlaying)
        {
            audioSources[1].Play();
        }
        else if (playerController.currentState != NewPlayerController.MovementState.StaminaRecharge)
        {
            audioSources[1].Stop();
        }
    }
}
