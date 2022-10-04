using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private float followFactor = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(this.transform.position.x, Camera.main.transform.position.y, this.transform.position.z);
        Vector3 current = Camera.main.transform.position;
        Camera.main.transform.position = Vector3.Lerp(target, current, followFactor);
    }
}
