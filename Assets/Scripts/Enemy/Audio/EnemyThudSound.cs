using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThudSound : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private AudioClip[] thudSounds;

    private AudioSource audioSource;

    private bool currentStunState;

    void Start()
    {
        currentStunState = false;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (currentStunState != enemyController.IsStunned())
        {
            if (enemyController.IsStunned())
            {
                audioSource.PlayOneShot(thudSounds[UnityEngine.Random.Range(0, thudSounds.Length)]);
                //Debug.Log("Play thud");
            }
            currentStunState = enemyController.IsStunned();
        }
    }
}
