using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject tutorialUI; 
    [SerializeField] private GameObject gameSelectUI; 

    public void StartGame()
    {
        ToggleGameSelect();
    }

    public void playSurvival()
    {
        SceneManager.LoadScene("MainGame");
    }    
    
    public void playArcade()
    {
        SceneManager.LoadScene("CircusTentScene");
    }

    public void ToggleTutorial()
    {
        if (tutorialUI == null) { Debug.LogError("[MainMenuManger] GameObject tutorialUI not found"); }
        if (gameSelectUI.activeSelf)
        {
            gameSelectUI.SetActive(false);
        }

        if (tutorialUI.activeSelf) { 
            tutorialUI.SetActive(false); 
        } 
        else { 
            tutorialUI.SetActive(true);
        }
    }

    public void ToggleGameSelect()
    {
        if (gameSelectUI == null) { Debug.LogError("[MainMenuManger] GameObject gameSelectUI not found"); }

        if (tutorialUI.activeSelf)
        {
            tutorialUI.SetActive(false);
        }

        if (gameSelectUI.activeSelf)
        {
            gameSelectUI.SetActive(false);
        }
        else
        {
            gameSelectUI.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
