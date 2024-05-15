using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryAndDeath : MonoBehaviour
{
    //UIStateManager stateManager;
    public GameObject victoryMessage;
    public GameObject deathMessage;

    public bool gameOver = false;

    private void Start()
    {
        //stateManager = GetComponent<UIStateManager>();
    }

    public void Victory()
    {
        gameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        UIStateManager.currentState = UIStateManager.UIState.PAUSED;
        victoryMessage.SetActive(true);
    }

    public void Death()
    {
        gameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        UIStateManager.currentState = UIStateManager.UIState.PAUSED;
        deathMessage.SetActive(true);
    }

    public void Exit()
    {
        gameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
