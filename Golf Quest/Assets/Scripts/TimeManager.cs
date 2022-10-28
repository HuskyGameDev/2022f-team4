using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private float velocityThreshold = 0.1f, timeScaleMin = 0.1f;

    private Rigidbody playerRb;
    private BallMovement ball;

    void Start() {
        
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Player").GetComponent<BallMovement>();
        Time.timeScale = 1.0f;
    }

    void Update() {
        
        float mag = playerRb.velocity.sqrMagnitude;

        if(mag <= velocityThreshold && !ball.isLaunching())
            Time.timeScale = Mathf.Max(((1 - timeScaleMin) / (velocityThreshold - ball.getMovingThreshold()) * (mag - velocityThreshold) + 1), timeScaleMin);
        else
            Time.timeScale = 1.0f;
    }
}
