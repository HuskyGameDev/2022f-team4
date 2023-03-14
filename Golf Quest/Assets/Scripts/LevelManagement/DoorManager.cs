using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    
    private bool open = false;  // Flag that shows whether the door is open or closed
    [SerializeField] public int openWithEnemiesLeft; // Flag that indicates how many enemies should be left to open the door
    
    private GameObject[] allEnemies;
    private int startRotation;

    // Start is called before the first frame update
    void Start()
    {
        //levelCompletedManager = GameObject.Find("LevelCompletedMenu").GetComponent<LevelCompletedManager>();
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        startRotation = 0;

        if(allEnemies.Length <= openWithEnemiesLeft) {
            open = true;
            //openSprite.enabled = true;
            //closedSprite.enabled = false;
            
        } else {
            open = false;
            //openSprite.enabled = false;
            //closedSprite.enabled = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(allEnemies.Length <= openWithEnemiesLeft) {
            open = true;
            //openSprite.enabled = true;
            //closedSprite.enabled = false;
        } else {
            open = false;
            //openSprite.enabled = false;
            //closedSprite.enabled = true;
        }

        if(open) {
            OpenDoor();
        }
    }

    void OpenDoor() {
        startRotation = 1;
        transform.Rotate(Vector3.down, startRotation);
        startRotation = 0;
    }
}
