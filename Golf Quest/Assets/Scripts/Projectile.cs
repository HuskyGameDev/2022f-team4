using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 1;
    public float lifetime = 5.0f; // Stored in seconds

    private float startLifetime;

    void Start() {

        startLifetime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update() 
    {
        transform.Translate(Vector3.right * Time.deltaTime * Speed);

        if (Time.fixedTime - startLifetime > lifetime) { }
            //Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (!other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            Destroy(this.gameObject);
        }
        
    }
}
