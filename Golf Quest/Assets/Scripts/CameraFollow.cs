using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform ball;

    void Start() {
        
        ball = GameObject.Find("Player Ball").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
