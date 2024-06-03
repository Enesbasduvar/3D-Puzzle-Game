using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene when the "Start Game" button is clicked
        SceneManager.LoadScene("SampleScene"); // Replace "GameScene" with the name of your game scene
    }

    public void TrainingCenter()
    {
        // Load the game scene when the "Start Game" button is clicked
        SceneManager.LoadScene("TrainingCenter"); // Replace "GameScene" with the name of your game scene
    }

    public void QuitGame()
    {
        // Quit the game when the "Quit" button is clicked
        Application.Quit();
    }
}
