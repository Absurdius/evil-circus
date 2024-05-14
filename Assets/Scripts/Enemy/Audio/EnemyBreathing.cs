using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreathing : MonoBehaviour
{
    UIStateManager stateManager;
    private AudioSource audioSource;

    void Start()
    {
        stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (stateManager.currentState == UIStateManager.UIState.PAUSED)
        {
            audioSource.Stop();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
