using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    
    private BallMovement ball;

    private float angle;

    // Start is called before the first frame update
    void Start() {

        ball = this.GetComponentInParent<BallMovement>();
    }

    void Update() {

        if(ball.isMoving() || ball.isAiming()) {
        
            angle = Mathf.Atan2(ball.getDirection().x, ball.getDirection().z) * Mathf.Rad2Deg;
        }

        transform.forward = Camera.main.transform.forward;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
    }
}
