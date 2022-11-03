using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathNode : MonoBehaviour
{
    public int order;

    public void Start()
    {
        // Destroy path visualization at runtime
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
    }
}
