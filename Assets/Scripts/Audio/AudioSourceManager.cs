using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    
 
 */
public class AudioSourceManager : MonoBehaviour
{


    public string[] audioNames; 
    public AudioClip[] audioClips;
    public bool loopSound; 

    private Dictionary<string, AudioClip> wordToAudioClipMap;
    private AudioSource audioSource;

    void Start()
    {
        if(audioClips.Length != audioNames.Length)
        {
            Debug.LogError("Audionames has a dirrefernt length than Clips");
        } else
        {
            for(int i = 0; i < audioClips.Length; i++)
            {
                wordToAudioClipMap.Add(audioNames[i], audioClips[i]);
            }
        }

        this.audioSource = GetComponent<AudioSource>();
        if (this.audioSource == null) {
            Debug.LogError("No audio source found!");
        }

        if(loopSound)
        {
            this.audioSource.loop = loopSound;
        }
    }

    void PlayAudio(string audioName)
    {
        if(this.audioSource.isPlaying)
        {
            this.audioSource.Stop();
        }
        this.audioSource.clip = wordToAudioClipMap[audioName];
        this.audioSource.Play();
    }
}
