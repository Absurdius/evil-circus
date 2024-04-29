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
            wordToAudioClipMap = new Dictionary<string, AudioClip>();
            for(int i = 0; i < audioClips.Length; i++)
            {
                wordToAudioClipMap.Add(audioNames[i], audioClips[i]);
            }
            Debug.Log(wordToAudioClipMap.ToString());
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
    public void PlayAudio(string audioName)
    {
        Debug.Log("PlaySound " + audioName);
        if(this.audioSource.isPlaying)
        {
            this.audioSource.Stop();
        }
        Debug.Break();
        this.audioSource.clip = audioClips[1];
        this.audioSource.Play();
    }

    public void Stop()
    {
        this.audioSource.Stop();
    }
}
