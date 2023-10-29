using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BallController : MonoBehaviour {

    public Rigidbody rb;
    public float impulseForce = 5f;

    private Vector3 startPos;
    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;

    public GameObject ballPrefab;
    public int scoreValue = 10;
    private void Awake()
    {
        startPos = transform.position;
    }



    private void OnCollisionEnter(Collision other)
    {
        if (ignoreNextCollision)
            return;
        if (isSuperSpeedActive)
        {
            if (!other.transform.GetComponent<Goal>())
            {
                foreach (Transform t in other.transform.parent)
                {
                    gameObject.AddComponent<TriangleExplosion>();

                    StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
                    Destroy(other.gameObject);
                    Debug.Log("exploding - exploding - exploding - exploding");
                }
                Destroy(other.transform.parent.gameObject);

            }
            else
            {
                Goal goalPart = other.transform.GetComponent<Goal>();
                if (goalPart)
                {
                    Debug.Log("Ball scored!");
                    GameManager.singleton.AddScore(scoreValue);
                }
            }

        }
        // If super speed is not active and a death part git hit -> restart game
        else
        {
            DeathPart deathPart = other.transform.GetComponent<DeathPart>();
            if (deathPart)
            {
                Debug.Log("Hit a Death Part!");
                deathPart.HittedDeathPart();
            }
        }

        rb.velocity = Vector3.zero; // Remove velocity to not make the ball jump higher after falling done a greater distance
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);



        // Safety check
        ignoreNextCollision = true;
        StartCoroutine(AllowCollision());

        // Handlig super speed
        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        // activate super speed
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            Debug.Log("Activating Super Speed Mode");
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        Debug.Log("Resetting Ball Position");
        transform.position = startPos;
    }

    private IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(0.2f);
        ignoreNextCollision = false;
    }
    public void SpawnBalls(int numberOfBalls)
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            Instantiate(ballPrefab, startPos, Quaternion.identity);
        }
    }
    private void Start() 
    {
        RetrieveGameScores();
    }

    private void RetrieveGameScores()
    {
        if (GameManager.singleton != null)
        {
            int currentScore = GameManager.singleton.score;
            int currentBest = GameManager.singleton.best;
           
        }
    }

    public void AddScoreFromBall(int scoreToAdd, GameObject ball)
    {
        GameManager.singleton.AddScore(scoreToAdd); // Use GameManager's AddScore method
        Debug.Log($"Ball {ball.name} added {scoreToAdd} to score.");
    }


}
