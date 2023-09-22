using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    
    [SerializeField] private bool clockwise;  // Flag that indicates whether the door should move clockwise to open or counter clockwise
    [SerializeField] public int enemiesRemaining; // Value that indicates how many enemies should be left to open the door
    [SerializeField] public int angle = 90; // Value that indicates how far a door should rotate to be considered open
    private bool open;  // Flag that shows whether the door needs to "open"
    private int doorAngle; // Keeps track of the number of degrees the door has been rotated since its starting position
    AudioSource doorCreak;
    private GameObject[] allEnemies;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
        doorAngle = 0;
        doorCreak = this.gameObject.GetComponent<AudioSource>();

        //levelCompletedManager = GameObject.Find("LevelCompletedMenu").GetComponent<LevelCompletedManager>();
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
/*
        if(allEnemies.Length <= enemiesRemaining) {
            open = true;
            //openSprite.enabled = true;
            //closedSprite.enabled = false;
            
        } else {
            open = false;
            //openSprite.enabled = false;
            //closedSprite.enabled = true;
        }

*/
    }

    // Update is called once per frame
    void Update()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If the door is not all the way open, see if enough enemies have been killed to open it
        if(!open && allEnemies.Length <= enemiesRemaining ) 
            StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor() {
        // Play the door creak at the start of the door opening
        if(doorAngle == 0)
            doorCreak.Play();

        // Open the door
        //for(int i = 0; i <= angle; i++)
        if(doorAngle <= angle){
            if(clockwise) {
                transform.Rotate(Vector3.down, -1);
                doorAngle++;
            } else {
                transform.Rotate(Vector3.down, 1);
                doorAngle++; 
            }
            if(doorAngle==angle)
                open = true; // Update the flag
            yield return null; // Pause here
        }
    }
}
