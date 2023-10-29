using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public BallController target;
    private float offset; // Keep initial distance between cam and ball

    private void Awake()
    {
        if (target)
        {
            offset = transform.position.y - target.transform.position.y;
        }
        else
        {
            Debug.LogWarning("Target ball not set for CameraController.");
        }
    }

    void Update () 
    {
        // Move camera smoothly to target height (yTargetPos)
        if (target)
        {
            Vector3 curPos = transform.position;
            curPos.y = target.transform.position.y + offset;
            transform.position = curPos;
        }
    }
}
