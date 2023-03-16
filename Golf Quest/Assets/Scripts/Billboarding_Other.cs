using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding_Other : MonoBehaviour
{

    private Projectile projectile;

    private float angle;

    // Start is called before the first frame update
    void Start() {
            projectile = this.GetComponentInParent<Projectile>();
    }

    void Update() {
        transform.forward = Camera.main.transform.forward;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.y);
    }
}
