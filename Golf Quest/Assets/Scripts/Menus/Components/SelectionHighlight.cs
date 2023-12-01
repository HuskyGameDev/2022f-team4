using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class SelectionHighlight : MonoBehaviour {

    private EventSystem eventSystem;
    private GameObject currSelection;
    private AudioSource audioSource;
    private bool firstPlay; // Used to skip the first attempt to play the selction sound
    
    //private TitleScreenManager titleScreenManager;
    private AudioSource neighborAudioSource;

    void Start() {

        firstPlay = true;
        eventSystem = GetComponent<EventSystem>();
        currSelection = eventSystem.currentSelectedGameObject;
        audioSource = GetComponent<AudioSource>();
        GameObject neighborObject = GameObject.Find("Canvas");
        neighborAudioSource = neighborObject.GetComponent<AudioSource>();
        if(neighborAudioSource == null){
            neighborObject = GameObject.Find("PauseMenu");
            neighborAudioSource = neighborObject.GetComponent<AudioSource>(); 
        }
        //titleScreenManager = GetComponent<TitleScreenManager>();
        enable(currSelection);
    }

    void Update() {

        if (eventSystem.currentSelectedGameObject != currSelection) {

            disable(currSelection);
            currSelection = eventSystem.currentSelectedGameObject;
            enable(currSelection);
        }
    }

    void disable(GameObject selection) {

        if(selection == null)
            return;

        Outline outline = selection.GetComponent<Outline>();

        if(outline == null)
            return;

        outline.enabled = false;
    }

    void enable(GameObject selection) {

        if(selection == null)
            return;

        Outline outline = selection.GetComponent<Outline>();
        //if(!firstPlay && !titleScreenManager.IsButtonSoundPlaying())
        if(!firstPlay && !neighborAudioSource.isPlaying)
            audioSource.Play();
        else 
            firstPlay = false;

        if(outline == null)
            return;

        outline.enabled = true;
    }
}
