using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{

    
    [SerializeField]public float deployedTime, retractedTime;
    [SerializeField]public bool startDeployed = false;
    private bool deployed = false, retracting = false, deploying = false;

    private void Start() {
        // Set start position
        if(!startDeployed) {
            transform.Translate(0, 0.8f, 0);
            deployed = true;
        } 

        
    }

   private void Update() {
        // Shoot and retract periodically
        if(deployed && !retracting) {
            StartCoroutine(retract());
        } 
        if(!deployed && !deploying) {
            StartCoroutine(deploy());
        }
   }

    IEnumerator retract() {
        Debug.Log("Retract at " + Time.time);
        retracting = true;
        transform.Translate(0, -0.8f, 0);
        //Time.timeScale = 1;
        yield return new WaitForSeconds(retractedTime);
        deployed = false;
        retracting = false;
        
    }

    IEnumerator deploy() {
        Debug.Log("Deploy at " + Time.time);
        deploying = true;
        transform.Translate(0, 0.8f, 0);
        //Time.timeScale = 1;
        yield return new WaitForSeconds(deployedTime);
        deployed = true;
        deploying = false;
    }

/*    public List<CharacterControl> ListCharacters = new List<CharacterControl>();

    private void start() {
        // Start with empty lists
        ListCharacters.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        CharacterControl control other.GameObject.transform.root.GameObject.GetComponent<CharacterControl>();

        if (control != null) {
            if(!ListCharacters.Contains(control)) {
                ListCharacters.Remove(control); 
            }
        }
    } */
}
