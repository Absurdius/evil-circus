using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVictory : MonoBehaviour
{
    public GameObject victoryMessage;
    public UIStateManager stateManager;

    private void Victory()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        stateManager.currentState = UIStateManager.UIState.PAUSED;
        victoryMessage.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Victory();
        }

        Debug.Log("detected collision. layermask is " + other.gameObject.layer);
    }
}
