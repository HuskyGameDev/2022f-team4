using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHoleManager : MonoBehaviour {

    private bool open = false;

    [SerializeField]
    private SpriteRenderer openSprite, closedSprite;

    private LevelCompletedManager levelCompletedManager;
    private GameObject[] allEnemies;
    
    [SerializeField]private AudioSource success;

    void Start() {

        levelCompletedManager = GameObject.Find("LevelCompletedMenu").GetComponent<LevelCompletedManager>();
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(allEnemies.Length == 0) {
            open = true;
            openSprite.enabled = true;
            closedSprite.enabled = false;
        } else {
            open = false;
            openSprite.enabled = false;
            closedSprite.enabled = true;
        }
    }

    void Update() {

        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(allEnemies.Length == 0) {
            open = true;
            openSprite.enabled = true;
            closedSprite.enabled = false;
        } else {
            open = false;
            openSprite.enabled = false;
            closedSprite.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {

        if(!other.CompareTag("Player"))
            return;

        if(!isOpen())
            return;

        success.Play();    

        levelCompletedManager.LevelCompleted();
    }

    public bool isOpen() { return open; }
}
