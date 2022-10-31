using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Destructible : MonoBehaviour {

    [SerializeField]
    private int maxHealth = 1, dmgOnWallHit = 0, dmgOnHit = 0, dmgOnDestroy = 0;
    /*
        Max Health          Stores the number of hits it takes from the player to destroy this object.
        Dmg On Wall Hit     Stores the damage this object takes when it collides with a wall.
        Dmg On Hit          Stores the damage this object deals the player when it hits this object.
        Dmg On Destroy      Stores the damage this object deals the player when it destroys this object.
    */

    private int currHealth;

    void Start() {

        currHealth = maxHealth;
    }

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {

            currHealth = Mathf.Max(0, currHealth - 1);

            BallStats ballStats = other.GetComponent<BallStats>();

            if (currHealth == 0)
                ballStats.takeDamage(dmgOnDestroy);
            else
                ballStats.takeDamage(dmgOnHit);

        } else if (other.CompareTag("Wall")) {

            currHealth = Mathf.Max(0, currHealth - dmgOnWallHit);
        }

        if(currHealth == 0) {

            // Play destruction animation
            // Play destruction sound
            Destroy(gameObject);
        }
    }

    public int getMaxHealth() { return maxHealth; }
    public int getCurrHealth() { return currHealth; }
}
