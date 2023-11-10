using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {


    [SerializeField]
    private float velocityThreshold = 0.1f, timeScaleMin = 0.1f;

    private Rigidbody playerRb;
    private BallMovement ball;

    private static int pauseFactor = 1;

    void Start() {
        
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Player").GetComponent<BallMovement>();
        Time.timeScale = 1.0f;
    }

    void Update() {
        
        float mag = playerRb.velocity.sqrMagnitude;

        if(mag <= velocityThreshold && !ball.isLaunching())
            Time.timeScale = Mathf.Max(((1 - timeScaleMin) / (velocityThreshold - ball.getMovingThreshold()) * (mag - velocityThreshold) + 1), timeScaleMin) * pauseFactor;
        else
            Time.timeScale = 1.0f * pauseFactor;
    }

    public static void Pause() { pauseFactor = 0; }
    public static void Resume() { pauseFactor = 1; }

    public static string formatTime(float elapsedTime) {

        int minutes = (int) elapsedTime / 60;
        int seconds = (int) elapsedTime % 60;
        int milliseconds = Mathf.RoundToInt((elapsedTime - (int) elapsedTime) * 100);

        if (minutes == 0)
            return string.Format("{0:00}.{1:00}", seconds, milliseconds);
        else
            return string.Format("{0:##}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
