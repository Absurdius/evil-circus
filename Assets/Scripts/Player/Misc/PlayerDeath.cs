using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathMessage;
    public GameObject playerCam;
    public AudioSource playerAudioSource;
    public AudioClip deathAudioClip;
    public float waitingtime = 1.0f;
    //UIStateManager stateManager;

    public bool isDead;
    private bool sequenceCompleted = false;

    private void Start()
    {
        //stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();
        playerCam.GetComponent<Animator>().enabled = false;
    }

    public void Death()
    {
        Animator deathAnimator = playerCam.GetComponent<Animator>();
        if(deathAnimator.enabled == false) { 
            Debug.Log("Trigger death sequence");
            deathAnimator.enabled = true;
            deathAnimator.SetTrigger("DeathTrigger");
            playerAudioSource.PlayOneShot(deathAudioClip);
        }

        
        StartCoroutine(ShowDeathScreen());
        StartCoroutine(PlayDeathSequence());
    }

    private IEnumerator ShowDeathScreen()
    {
        yield return new WaitUntil(() => sequenceCompleted);
        isDead = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        UIStateManager.currentState = UIStateManager.UIState.PAUSED;
        deathMessage.SetActive(true);
    }

    private IEnumerator PlayDeathSequence()
    {
        yield return new WaitForSeconds(waitingtime);
        sequenceCompleted = true;
    }

    public void Exit()
    {
        isDead = false;
        SceneManager.LoadScene("MainMenu");
    }
}
