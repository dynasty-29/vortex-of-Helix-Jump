using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public int best;
    public int score;
    public int currentStage = 0;

    // Challenge Mode Spawning properties
    public GameObject challengeObstaclePrefab; // Prefab for the challenging obstacle
    public Transform spawnPoint; // Point where the obstacle will be spawned
    public bool isChallengeModeActive = false;
    //private float nextSpawnTime = 0f;


    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);  // Keep the singleton across scenes
        }            
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
            
        // Load the saved highscore
        best = PlayerPrefs.GetInt("Highscore");
    }

    public void NextLevel()
    {
        Debug.Log("Loading Next Level...");
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level");
        // Show Adds Advertisement.Show();
        score = 0;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log($"Added {scoreToAdd} to score. Total Score: {score}");

        if (score > best)
        {
            PlayerPrefs.SetInt("Highscore", score);
            best = score;
            Debug.Log($"New Highscore: {best}");
        }
    }
    // The new method for spawning challenge obstacles
    void SpawnChallengeObstacle()
    {
        float randomYOffset = Random.Range(-2f, 2f); // Vary the Y position for added challenge
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, randomYOffset, 0);
        Instantiate(challengeObstaclePrefab, spawnPosition, Quaternion.identity);
    }


}
