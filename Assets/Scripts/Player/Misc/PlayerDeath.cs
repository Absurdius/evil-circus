using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathMessage;
    public UIStateManager stateManager;

    public bool isDead;

    public void Death()
    {
        isDead = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        stateManager.currentState = UIStateManager.UIState.PAUSED;
        deathMessage.SetActive(true);

    }

    public void Exit()
    {
        isDead = false;
        SceneManager.LoadScene("MainMenu");
    }
}
