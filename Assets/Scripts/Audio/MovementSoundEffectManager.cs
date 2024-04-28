using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MovementSoundEffectManager : MonoBehaviour
{
    public static readonly int WALKING_SOUND = 0;
    public static readonly int RUNNING_SOUND = 1;

    private AudioSource[] audioSources; 

    void Start()
    {
        // audioSources[0] walking
        // ... [1] running
        this.audioSources = GetComponents<AudioSource>();
        if (audioSources.Length == 0)
        {
            Debug.LogError("AudioSources Not Found");
        }
    }

    public void PlaySound(int id)
    {
        audioSources[id].Play();
    }

    public void StopSound(int id)
    {
        audioSources[id].Pause();
    }
}
