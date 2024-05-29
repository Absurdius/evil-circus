using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject deathScreen;

    public bool gameOver;

    public void DisplayDeathScreen()
    {
        gameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        UIStateManager.currentState = UIStateManager.UIState.PAUSED;
        deathScreen.SetActive(true);
    }

    public void DisplayVictoryScreen()
    {
        gameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        UIStateManager.currentState = UIStateManager.UIState.PAUSED;
        victoryScreen.SetActive(true);
    }
}
