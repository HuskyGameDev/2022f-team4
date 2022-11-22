using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPath : MonoBehaviour {
    
    private GameObject ballObj;
    private Rigidbody ballRb;
    private BallMovement ballMovement;
    private float ballRadius;

    private LineRenderer line;

    [SerializeField]
    private float lineMultiplier;
    [SerializeField]
    private LayerMask wallMask;

    void Start() {

        ballObj = GameObject.Find("Player Ball");
        ballRb = ballObj.GetComponent<Rigidbody>();
        ballMovement = ballObj.GetComponent<BallMovement>();
        ballRadius = ballObj.transform.localScale.x * (ballObj.GetComponent<SphereCollider>().radius * 0.99f);
        
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    List<Vector3> path = new List<Vector3>();

    void Update() {

        if (ballMovement.isAiming() && !ballMovement.isMoving()) {

            path.Clear();

            raycastPath(ballObj.transform.position + new Vector3(0.0f, 0.1f, 0.0f), ballMovement.getDirection(), lineMultiplier * ballMovement.getMagnitude());

            line.positionCount = path.Count;

            for (int i = 0; i < path.Count; i++)
                line.SetPosition(i, path[i]);
 
            line.enabled = true;
        } else {

            path.Clear();
            line.enabled = false;
        }
    }

    private void raycastPath(Vector3 origin, Vector3 direction, float distance) {

            path.Add(origin);
            direction.y = 0.0f;

            RaycastHit hit;
            Ray ray = new Ray(origin, direction * distance);

            if(Physics.SphereCast(ray, ballRadius, out hit, distance, wallMask) && hit.distance < distance) {

                Vector3 reflectedDirection = Vector3.Reflect(direction, hit.normal);
                raycastPath(hit.point, reflectedDirection, distance - hit.distance);

            } else {

                path.Add(direction.normalized * distance + origin);
            }
    }
}
