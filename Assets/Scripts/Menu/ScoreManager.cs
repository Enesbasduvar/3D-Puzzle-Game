using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance

    public int score = 0;
    public int level = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endscoreText;
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void AddLevel(int amount)
    {
        level += amount;
        UpdateLevelUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        endscoreText.text = "Score: " + score.ToString();
    }

    void UpdateLevelUI()
    {
        levelText.text = "Level: " + level.ToString();
    }
}
