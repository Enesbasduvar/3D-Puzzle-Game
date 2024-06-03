using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }
}
