using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused
    }
    public static GameState currentState = GameState.Playing;

    public bool canPause;

    private GameObject HUD;
    private GameObject pauseMenu;
    private GameObject deathScreen;
    private GameObject victoryScreen;

    private void Start()
    {
        HUD = GameObject.Find("HUD");
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        deathScreen = GameObject.Find("DeathScreen");
        deathScreen.SetActive(false);
        victoryScreen = GameObject.Find("VictoryScreen");
        victoryScreen.SetActive(false);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        if (currentState == GameState.Playing)
        {
            Pause();
            HUD.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else if (currentState == GameState.Paused)
        {
            Pause();
            HUD.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }

    public void DisplayDeathScreen()
    {
        canPause = false;
        Pause();
        HUD.SetActive(false);
        pauseMenu.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void DisplayVictoryScreen()
    {
        canPause = false;
        Pause();
        HUD.SetActive(false);
        pauseMenu.SetActive(false);
        victoryScreen.SetActive(true);
    }

    public void Pause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
