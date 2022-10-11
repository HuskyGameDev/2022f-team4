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
    private LayerMask pathMask;

    void Start() {

        ballObj = GameObject.Find("Player Ball");
        ballRb = ballObj.GetComponent<Rigidbody>();
        ballMovement = ballObj.GetComponent<BallMovement>();
        ballRadius = ballObj.GetComponent<SphereCollider>().radius;
        
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    List<Vector3> path = new List<Vector3>();

    void Update() {

        if(ballMovement.isMoving() || ballMovement.isDragging()) {

            line.enabled = true;
            path.Clear();

            raycastPath(ballObj.transform.position, ballMovement.getDirection(), lineMultiplier * ballMovement.getMagnitude());

            line.positionCount = path.Count;

            for (int i = 0; i < path.Count; i++)
                line.SetPosition(i, path[i]);

        } else {

            line.enabled = false;
        }
    }

    private void raycastPath(Vector3 origin, Vector3 direction, float distance) {

            path.Add(origin);

            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit, distance, pathMask) && hit.distance < distance) {
                
                Vector3 reflectedDirection = Vector3.Reflect(direction, hit.normal);    
                raycastPath(hit.point, reflectedDirection, distance - hit.distance);

            } else {

                path.Add(direction.normalized * distance + origin);
            }
    }
}
