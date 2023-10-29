using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtBest;
    private float timeLeft = 120.0f;
    private bool isTimeModeActive = true;
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;
    private int playerScore = 0;
    void Update()
    {
        if (GameManager.singleton)
        {
            txtBest.text = "Best: " + GameManager.singleton.best;
            txtScore.text = "Score: " + GameManager.singleton.score;
        }
        else
        {
            Debug.LogWarning("GameManager singleton not found!");
        }
        if (isTimeModeActive)
        { // Boolean to check if Time Mode is active
            timeLeft -= Time.deltaTime;
            UpdateUITimeDisplay(timeLeft);

            if (timeLeft <= 0)
            {
                EndGame();
            }
        }
    }
    void UpdateUITimeDisplay(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("timerText reference is missing!");
        }
    }

    void EndGame()
    {
        // Stop gameplay
        isTimeModeActive = false;

        // Show a game over screen
        gameOverScreen.SetActive(true); 

    }
    void SaveScore()
    {
        // Save the score if it's higher than the saved high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            PlayerPrefs.Save();
        }
    }

}
