using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private Rigidbody rigidBody;
    private LineRenderer pullLine;

    private Vector3 dragStartPos;   // Stores the position of where the mouse drag was started.
    private Vector3 dragCurrPos;    // Stores the position of the mouse during the drag.
    private bool dragging;          // Stores whether or not the player is currently dragging.
    private bool moving;

    [Header("Launch Properties")]
    [SerializeField]
    private float magnitudeMax = 20.0f;
    [SerializeField]
    private float magnitudeScalar = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        pullLine = GetComponentInChildren<LineRenderer>();
        pullLine.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {

        if (dragging) {
            
            dragCurrPos = screenToWorld(Input.mousePosition);    // Update the current mouse position while dragging

            // TODO: Draw trajectory of ball

            // Draw pull back band
            pullLine.SetPositions(new Vector3[] {dragStartPos, dragCurrPos});
        }

        if(rigidBody.velocity.sqrMagnitude > 0.001f)
            moving = true;
        else
            moving = false;
    }

    private void OnMouseDown()
    {
        //Debug.Log("Mouse Pressed");

        if (moving)                                                          // Only allow dragging if ball has stopped moving
            return;

        dragStartPos = screenToWorld(Camera.main.WorldToScreenPoint(this.transform.position));                 // Store the start drag position
        dragging = true;                                                                                       // Started dragging
        pullLine.enabled = true;
    }

    private void OnMouseUp()
    {
        //Debug.Log("Mouse Released");
        Vector3 launchVector = dragCurrPos - dragStartPos;                                      // Calculate the desired launch vector
        float magnitude = Mathf.Min(launchVector.magnitude * magnitudeScalar, magnitudeMax);    // Scale the magnitude and ensure it is capped at a maximum.
        Vector3 cappedVector = launchVector.normalized * magnitude * -1.0f;                     // Calculate the capped vector with the adjusted magnitude.

        //Debug.Log(dragStartPos + " : " + dragCurrPos);
        //Debug.Log(magnitude);

        rigidBody.AddForce(cappedVector, ForceMode.Impulse);                                    // Apply an impulse force in the capped vector direction.
        dragging = false;                                                                       // Stopped dragging
        dragStartPos = Vector3.zero;                                                            // Reset positions
        dragCurrPos = Vector3.zero;
        pullLine.enabled = false;
    }

    private Vector3 screenToWorld(Vector3 vec) 
    {   
        vec.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(vec);
    }

    public bool isMoving() { return moving; }

    public bool isDragging() { return dragging; }

    public Vector3 getDirection() {

        if(moving)
            return rigidBody.velocity.normalized;
        else 
            return (dragStartPos - dragCurrPos).normalized;
    }

    public float getMagnitude() {

        Vector3 dir = dragCurrPos - dragStartPos;
        return Mathf.Min(dir.magnitude * magnitudeScalar, magnitudeMax);
    }
}
