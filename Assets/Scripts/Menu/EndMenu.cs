using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] GameObject EndMenuUI;
    void Start()
    {
        EndMenuUI.SetActive(false);
    }
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (EndMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        */
    }
    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EndMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Cursor.visible = false;
        EndMenuUI.SetActive(false);
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
