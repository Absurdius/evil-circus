using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject tutorialUI; 

    public void StartGame()
    {
        //SceneManager.LoadScene("SliceScene");
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("DevScene");
    }

    public void ToggleTutorial()
    {
        if (tutorialUI == null) { Debug.LogError("[MainMenuManger] GameObject tutorialUI not found"); }

        if (tutorialUI.activeSelf) { 
            tutorialUI.SetActive(false); 
        } 
        else { 
            tutorialUI.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
