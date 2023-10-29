using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPart : MonoBehaviour {

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void HittedDeathPart( )
    {
        Debug.Log("Player hit a death part. Restarting level.");
        if (GameManager.singleton)
        {
            GameManager.singleton.RestartLevel();
        }
        else
        {
            Debug.LogWarning("GameManager singleton not found!");
        }
           
    }
}
