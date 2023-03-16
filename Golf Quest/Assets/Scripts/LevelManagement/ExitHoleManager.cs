using UnityEngine;

public class ExitHoleManager : MonoBehaviour {

    private bool open = false;
    private bool hasPlayed = false;


    [SerializeField]
    private SpriteRenderer openSprite, closedSprite;

    private LevelCompletedManager levelCompletedManager;
    private GameObject[] allEnemies;
    public AudioSource audioPlayer;

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
            if(!hasPlayed){
               audioPlayer.Play();
            hasPlayed = true;
     }
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

        levelCompletedManager.LevelCompleted();
    }

    public bool isOpen() { return open; }
}
