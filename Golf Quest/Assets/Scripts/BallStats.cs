using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallStats : MonoBehaviour {

    [Header("InGame Stats UI Components")]
    [SerializeField]
    private TextMeshProUGUI timeLabel;
    [SerializeField]
    private TextMeshProUGUI strokeLabel;
    [SerializeField]
    private TextMeshProUGUI healthLabel;

    [Header("Player Stats")]
    [SerializeField]
    private int maxHealth = 5;

    private int currHealth, strokeCount;
    private float startTime;

    void Start() {

        currHealth = maxHealth;
        startTime = Time.unscaledTime;
    }

    void Update() {

        if (timeLabel)
            timeLabel.SetText(formatTime(getElapsedTime()));
        
        if (strokeLabel)
            strokeLabel.SetText(getStrokeCount().ToString());
        
        if (healthLabel)
            healthLabel.SetText(getCurrHealth() + "/" + getMaxHealth());
    }

    private string formatTime(float elapsedTime) {

        int minutes = (int) elapsedTime / 60;
        int seconds = (int) elapsedTime % 60;
        int milliseconds = (int) ((elapsedTime - (int) elapsedTime) * 100);

        if (minutes == 0)
            return string.Format("{0:00}.{1:00}", seconds, milliseconds);
        else
            return string.Format("{0:##}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    //////////////////////////////
    // Public Setters & Getters //
    //////////////////////////////

    public void takeDamage(int damage) {

        currHealth = Mathf.Max(0, currHealth - damage);

        if(currHealth == 0) {

            Debug.Log("You Died");
            // Provide death screen
            // Reset level
        }
    }

    public void addStroke() { strokeCount++; }

    public int getMaxHealth() { return maxHealth; }
    public int getCurrHealth() { return currHealth; }    
    public int getStrokeCount() { return strokeCount; }
    public float getElapsedTime() { return Time.unscaledTime - startTime; }
}
