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
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(laughs[UnityEngine.Random.Range(0, laughs.Length)]);
                }
            }
        } else
        {
            audioSource.Stop();
        }
    }
}
