using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
	public float fixedRotation = 5;
 	void Update ()
	{
		Vector3 eulerAngles = transform.eulerAngles;
		transform.eulerAngles = new Vector3( eulerAngles.x , fixedRotation , eulerAngles.z );
	}




}
