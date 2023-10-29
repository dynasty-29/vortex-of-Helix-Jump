using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Goal hit by the ball! Moving to the next level.");
        if (GameManager.singleton)
        {
            GameManager.singleton.NextLevel();
        }
        else
        {
            Debug.LogWarning("GameManager singleton not found!");
        }
           
    }
}
