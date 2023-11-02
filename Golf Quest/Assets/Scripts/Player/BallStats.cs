using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class BallStats : MonoBehaviour {

    [Header("InGame Stats UI Components")]
    [SerializeField]
    private TextMeshProUGUI timeLabel;
    [SerializeField]
    private TextMeshProUGUI strokeLabel;
    [SerializeField]
    private TextMeshProUGUI healthLabel;

    private Image deathMenuBg;
    private GameObject deathMenuPanel, restartButton;

    [Header("Player Stats")]
    [SerializeField]
    private int maxHealth = 5;

    private int currHealth, strokeCount;
    private float startTime;

    [SerializeField] AudioClip deathSound;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] wallHitSounds;
    [SerializeField] private AudioClip[] wallHitSoundSoft;
    [SerializeField] private AudioClip[] woodHitSound;
    private Rigidbody rb;


    void Start() {

        currHealth = maxHealth;
        startTime = Time.time;

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody> ();

        deathMenuBg = GameObject.Find("DeathMenu").GetComponent<Image>();
        deathMenuPanel = deathMenuBg.transform.GetChild(0).gameObject;
        restartButton = deathMenuPanel.transform.GetChild(1).gameObject;


        
    }

    void Update() {

        if (timeLabel)
            timeLabel.SetText(TimeManager.formatTime(getElapsedTime()));
        
        if (strokeLabel)
            strokeLabel.SetText(getStrokeCount().ToString());
        
        if (healthLabel)
            healthLabel.SetText(getCurrHealth() + "/" + getMaxHealth());
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Wood")) {
            //Debug.Log("Trigger Wood Hit SFX");
            audioSource.clip = woodHitSound[Random.Range(0, woodHitSound.Length)];
            audioSource.Play();
        }
    }
    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Wall(for SFX)")) {
            //Debug.Log("Start Wall Hit SFX");
            //Debug.Log(rb.velocity.magnitude);
            if(rb.velocity.magnitude < 10) {
                audioSource.clip = wallHitSoundSoft[Random.Range(0, wallHitSoundSoft.Length)];
            }
            else {
                audioSource.clip = wallHitSounds[Random.Range(0, wallHitSounds.Length)];
            }
            audioSource.Play();
        }
    }

    //////////////////////////////
    // Public Setters & Getters //
    //////////////////////////////

    public void takeDamage(int damage) {

        currHealth = Mathf.Max(0, currHealth - damage);

        if(currHealth == 0) {
            audioSource.clip = deathSound;
            audioSource.Play();
            deathMenuBg.enabled = true;
            deathMenuPanel.SetActive(true);
            TimeManager.Pause();
            EventSystem.current.SetSelectedGameObject(restartButton);
        }
    }

    public void addStroke() { strokeCount++; }

    public int getMaxHealth() { return maxHealth; }
    public int getCurrHealth() { return currHealth; }
    public int getStrokeCount() { return strokeCount; }
    public float getElapsedTime() { return Time.time - startTime; }

}
