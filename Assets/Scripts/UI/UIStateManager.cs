using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIStateManager : MonoBehaviour
{
    public GameObject playingUI;
    public GameObject pausedUI;

    public enum UIState
    {
        PLAYING,
        PAUSED
    }

    public UIState currentState;

    void Start()
    {
        Play();    
    }

    void Update()
    {
        // Changed Escape to Tab for testing
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentState == UIState.PLAYING){
                Pause();
            } else {
                Play();
            }
        }
    }

    //private UIState currentState;

    public void Play()
    {
        Time.timeScale = 1.0f;
        currentState = UIState.PLAYING;
        HideUI(pausedUI);
        ShowUI(playingUI);
        HideCursor();

        // Prevents play button getting stuck in selected state
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        currentState = UIState.PAUSED;
        ShowUI(pausedUI);
        HideUI(playingUI);
        ShowCursor();
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

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
