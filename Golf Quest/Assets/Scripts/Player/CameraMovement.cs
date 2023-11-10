using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    private PlayerInput input;
    private InputAction input_Zoom;
    private Transform ball;
    private Transform hole;
    private Vector3 velocity = Vector3.zero;
    private bool hasStarted = false;
    
    private float dampTime = 0.04f;
    [SerializeField]
    private float defaultDampTime = 0.2f;

    [SerializeField]
    private Vector2 zoomCap = new Vector2(5.0f, 15.0f);

    void Start() {
        input = GameObject.Find("Player Ball").GetComponent<PlayerInput>();
        input_Zoom = input.actions.FindAction("Zoom");
        ball = GameObject.Find("Player Ball").transform;

        //Start the camera over the hole
        hole = GameObject.Find("ExitHole").transform;
        this.transform.position =  new Vector3(hole.position.x, this.transform.position.y, hole.position.z);
        //StartCoroutine(delayStart());
    }

    void Update() {
        
        if (Time.timeScale > 0)
            CameraZoom();
        if(hasStarted)
            CameraFollow();
        else if (!TimeManager.isPaused())   
            StartCoroutine(delayStart());

        
    }

    //void FixedUpdate() {
        
        
    //}

    void CameraZoom() {

        Vector3 pos = this.transform.position;
        float scrollDelta = input_Zoom.ReadValue<float>();
        pos.y += scrollDelta;
        pos.y = Mathf.Max(zoomCap.x, Mathf.Min(zoomCap.y, pos.y));
        this.transform.position = pos;
    }

    void CameraFollow() {
        if(this.transform.position == ball.position)
            dampTime = defaultDampTime;


        Vector3 point = Camera.main.WorldToViewportPoint(ball.position);
        Vector3 delta = ball.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = this.transform.position + delta;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTime);
    }

    IEnumerator delayStart() {
        yield return new WaitForSeconds(0.025f);
        hasStarted = true;
    }
}
