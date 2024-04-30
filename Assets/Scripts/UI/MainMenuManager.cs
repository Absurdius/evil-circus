using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("DevScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
