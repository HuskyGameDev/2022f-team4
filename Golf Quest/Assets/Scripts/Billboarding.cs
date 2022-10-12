using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{

    private Camera bilCam;
    private BallMovement mov;

    // Start is called before the first frame update
    void Start()
    {
        bilCam = Camera.main;
        mov = this.GetComponentInParent<BallMovement>();
    }

    void LateUpdate()
    {
        transform.LookAt(bilCam.transform);
    }
    
    void Update(){
        Vector3 dir = mov.getDirection();
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x,angle,transform.rotation.z);

    }
}
