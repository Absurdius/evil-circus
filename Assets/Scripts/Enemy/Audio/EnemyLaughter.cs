using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaughter : MonoBehaviour
{
    public EnemyController enemyController;
    public AudioClip[] laughs;
    
    private Dictionary<EnemyController.EnemyState, AudioClip> soundDictionary;
    private AudioSource audioSource;
    private EnemyController.EnemyState currentState;



    void Start()
    {
        soundDictionary = new Dictionary<EnemyController.EnemyState, AudioClip>();

        audioSource = gameObject.AddComponent<AudioSource>();

        // Add audio clips to the dictionary
        soundDictionary.Add(EnemyController.EnemyState.Idle, laughs[0]);
        soundDictionary.Add(EnemyController.EnemyState.Patrol, laughs[1]);
        soundDictionary.Add(EnemyController.EnemyState.Chase, laughs[2]);
        soundDictionary.Add(EnemyController.EnemyState.Search, laughs[3]);
        soundDictionary.Add(EnemyController.EnemyState.Attack, laughs[4]);

        // Set the initial previous state
        currentState = enemyController.GetEnemyState();
    }

    void Update()
    {
        if(UIStateManager.currentState == UIStateManager.UIState.PLAYING)
        {
            // Check if the enemy state has changed
            if (enemyController.GetEnemyState() != currentState)
            {
                currentState = enemyController.GetEnemyState();
                if (soundDictionary.ContainsKey(currentState))
                {
                    audioSource.PlayOneShot(soundDictionary[currentState]);
                }
            }
        } else
        {
            audioSource.Stop();
        }
    }
}
