using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStateManager : MonoBehaviour
{
    public GameObject playingUI;
    public GameObject pausedUI;

    private enum UIState
    {
        PLAYING,
        PAUSED
    }

    void Start()
    {
        Play();    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == UIState.PLAYING){
                Pause();
            } else {
                Play();
            }
        }
    }

    private UIState currentState;

    public void Play()
    {
        Time.timeScale = 1.0f;
        currentState = UIState.PLAYING;
        HideUI(pausedUI);
        ShowUI(playingUI);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        currentState = UIState.PAUSED;
        ShowUI(pausedUI);
        HideUI(playingUI);
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowUI(GameObject uiElement)
    {
        if (uiElement != null)
            uiElement.SetActive(true);
    }

    private void HideUI(GameObject uiElement)
    {
        if (uiElement != null)
            uiElement.SetActive(false);
    }
}
