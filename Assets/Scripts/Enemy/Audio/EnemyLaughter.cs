using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaughter : MonoBehaviour
{
    private NewEnemyController enemyController;
    public AudioClip[] laughs;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyController = GetComponentInParent<NewEnemyController>();
    }

    void Update()
    {
        if(UIStateManager.currentState == UIStateManager.UIState.PLAYING)
        {
            // Check if the enemy state has changed
            if (enemyController.stateChanged)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(laughs[UnityEngine.Random.Range(0, laughs.Length)], 1.0f);
                }
            }
        } else
        {
            audioSource.Stop();
        }
    }
}
