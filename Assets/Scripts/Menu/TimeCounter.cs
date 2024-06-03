using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float totalTime = 300f; // Total time in seconds (5 minutes)
    private float currentTime;
    EndMenu endMenu;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = totalTime;
        
    }
    private void Awake()
    {
        endMenu = GameObject.FindGameObjectWithTag("endscore").GetComponent<EndMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current time
        currentTime -= Time.deltaTime;

        // Update the countdown text
        UpdateCountdownText();

        // Check if the countdown has reached zero
        if (currentTime <= 0)
        {
            // Countdown reached zero, perform necessary actions (e.g., end game)
            // You can add your own logic here
            Debug.Log("Countdown reached zero!");
            currentTime = 0; // Ensure the timer doesn't go negative
            endMenu.Pause();

        }
    }

    void UpdateCountdownText()
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Format the time as MM:SS
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Update the TextMeshPro Text component
        countdownText.text = timeString;
    }
}
