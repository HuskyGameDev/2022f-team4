using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallStats : MonoBehaviour {

    [Header("InGame Stats UI Components")]
    [SerializeField]
    private TextMeshProUGUI timeLabel;
    [SerializeField]
    private TextMeshProUGUI strokeLabel;
    [SerializeField]
    private TextMeshProUGUI healthLabel;

    private Image deathMenuBg;
    private GameObject deathMenuPanel;

    [Header("Player Stats")]
    [SerializeField]
    private int maxHealth = 5;

    private int currHealth, strokeCount;
    private float startTime;

    void Start() {

        currHealth = maxHealth;
        startTime = Time.time;

        deathMenuBg = GameObject.Find("DeathMenu").GetComponent<Image>();
        deathMenuPanel = deathMenuBg.transform.GetChild(0).gameObject;
    }

    void Update() {

        if (timeLabel)
            timeLabel.SetText(TimeManager.formatTime(getElapsedTime()));
        
        if (strokeLabel)
            strokeLabel.SetText(getStrokeCount().ToString());
        
        if (healthLabel)
            healthLabel.SetText(getCurrHealth() + "/" + getMaxHealth());
    }

    //////////////////////////////
    // Public Setters & Getters //
    //////////////////////////////

    public void takeDamage(int damage) {

        currHealth = Mathf.Max(0, currHealth - damage);

        if(currHealth == 0) {

            deathMenuBg.enabled = true;
            deathMenuPanel.SetActive(true);
            TimeManager.Pause();
        }
    }

    public void addStroke() { strokeCount++; }

    public int getMaxHealth() { return maxHealth; }
    public int getCurrHealth() { return currHealth; }    
    public int getStrokeCount() { return strokeCount; }
    public float getElapsedTime() { return Time.time - startTime; }
}
