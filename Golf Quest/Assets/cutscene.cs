using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscene : MonoBehaviour
{
    public GameObject videoPlayer;

    void Start(){
        videoPlayer.SetActive(false);
    }
    void OnTriggerEnter(Collider other){

        if(!other.CompareTag("Player"))
            return;

        videoPlayer.SetActive(true);
        Invoke("EndCutscene",2);
    }

    void EndCutscene(){
        videoPlayer.SetActive(false);
    }
}
