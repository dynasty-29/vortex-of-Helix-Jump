using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered in PassCheck");
        if (GameManager.singleton)
        {
            GameManager.singleton.AddScore(2);
        }
        else
        {
            Debug.LogWarning("GameManager singleton not found!");
        }

        BallController ball = FindObjectOfType<BallController>();
        if (ball)
            ball.perfectPass++;
    }
}
