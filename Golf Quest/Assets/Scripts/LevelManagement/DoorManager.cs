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

        if(allEnemies.Length <= enemiesRemaining) {
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

        // If the door is closed, see if enough enemies have been killed to open it
        if(allEnemies.Length <= enemiesRemaining) {
            open = true;
            //openSprite.enabled = true;
            //closedSprite.enabled = false;
        } else { //TODO This piece may not be necessary. 
            open = false;
            //openSprite.enabled = false;
            //closedSprite.enabled = true;
        }

        // Play the door creak at the start of the door opening
        if(open && doorAngle == 0) {
            doorCreak.Play();
        }

        // If the door needs to be opened counter clockwise
        if(!clockwise && open && doorAngle <= angle) {
            //Debug.Log(GameObject.eulerAngles.x); TODO Figure out how to have a door be open or closed...
            OpenCounterClockwise();
            
            
        } 
        // If the door needs to be opened clockwise
        else if(clockwise && open && doorAngle <= angle) {
            Debug.Log("There"); //TODO remove
            OpenClockwise();
        }

    }

    void OpenCounterClockwise() {
        transform.Rotate(Vector3.down, 1);
        doorAngle++; 
    }

    void OpenClockwise() {
        transform.Rotate(Vector3.down, -1);
        doorAngle++;
    }
}
