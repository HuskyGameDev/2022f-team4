using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Transform ball;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float dampTime = 0.2f;

    [SerializeField]
    private float zoomScale = -1f;
    [SerializeField]
    private Vector2 zoomCap = new Vector2(5.0f, 15.0f);

    void Start() {
        
        ball = GameObject.Find("Player Ball").transform;
        this.transform.position =  new Vector3(ball.position.x, this.transform.position.y, ball.position.z);
    }

    void Update() {

        CameraZoom();
        // Need to fix LineRenderers when zooming
    }

    void FixedUpdate() {

        CameraFollow();
    }

    void CameraZoom() {

        Vector3 pos = this.transform.position;
        float scrollDelta = Input.mouseScrollDelta.y * zoomScale;
        pos.y += scrollDelta;
        pos.y = Mathf.Max(zoomCap.x, Mathf.Min(zoomCap.y, pos.y));
        this.transform.position = pos;
    }

    void CameraFollow() {

        Vector3 point = Camera.main.WorldToViewportPoint(ball.position);
        Vector3 delta = ball.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = this.transform.position + delta;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTime);
    }
}
