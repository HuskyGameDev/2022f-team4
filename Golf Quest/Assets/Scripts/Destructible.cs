using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private Animator anim;

    private int currHealth;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip destroySound;

    private bool attackingOn; // Debug toggle that turns all damage by destrucible objects off;

    void Start() {
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        attackingOn = true;
    }

    void OnTriggerEnter(Collider other) {

        hit(other.gameObject);
    }

    void OnCollisionEnter(Collision collision) {

        hit(collision.gameObject);
    }

    private void hit(GameObject other) {

        if (other.CompareTag("Player")) {

            currHealth = Mathf.Max(0, currHealth - 1);

            BallStats ballStats = other.GetComponent<BallStats>();

            if (currHealth == 0)
                if(attackingOn)
                    ballStats.takeDamage(dmgOnDestroy);
            else
                if(attackingOn)
                    ballStats.takeDamage(dmgOnHit);

        } else if (other.CompareTag("Wall") || other.CompareTag("Wood")) {

            currHealth = Mathf.Max(0, currHealth - dmgOnWallHit);
        }

        if(currHealth == 0) {
            
            // Play destruction animation
            if(audioSource != null){
                transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                if(transform.GetChild(0).childCount == 2)
                    transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
                audioSource.clip = destroySound;
                audioSource.Play();
                // anim.SetTrigger("Break");                                       //Needs to be fixed; will be used to trigger the breaking / death animation
                Destroy(gameObject, destroySound.length);
            } else {
                // anim.SetTrigger("Break");                                       //Needs to be fixed; will be used to trigger the breaking / death animation
                Destroy(gameObject); 
            }
        }
    }

    public int getMaxHealth() { return maxHealth; }
    public int getCurrHealth() { return currHealth; }
}
