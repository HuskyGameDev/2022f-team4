using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class SelectionHighlight : MonoBehaviour {

    private EventSystem eventSystem;
    private GameObject currSelection;

    void Start() {

        eventSystem = GetComponent<EventSystem>();
        currSelection = eventSystem.currentSelectedGameObject;
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

        if(outline == null)
            return;

        outline.enabled = true;
    }
}
