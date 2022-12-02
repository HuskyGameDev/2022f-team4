using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    private LineRenderer lazer;
    [SerializeField]
    private LayerMask wallMask;
    [SerializeField]
    private int reflectionCount = 2;

    private MeshCollider _meshCollider;

    public float flashSpeed, chargeWidth, fireWidth;
    public int flashCount;

    public Color chargeColor, fireColor;

    public void charge()
    {
        lazer.startWidth = chargeWidth;
        lazer.endWidth = chargeWidth;
        lazer.startColor = chargeColor;
        lazer.endColor = chargeColor;

        for (int i = flashCount; i >= 0; i--)
        {

            StartCoroutine(wait(flashSpeed));

        }

        fire();
    }

    public void fire()
    {
        lazer.startWidth = fireWidth;
        lazer.endWidth = fireWidth;
        lazer.startColor = fireColor;
        lazer.endColor = fireColor;

        Mesh mesh = new Mesh();
        lazer.BakeMesh(mesh, true);
        _meshCollider.sharedMesh = mesh;
    }

    public IEnumerator wait(float timeInSeconds)
    {
        lazer.enabled = false;
        yield return new WaitForSeconds(timeInSeconds);
        lazer.enabled = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        lazer = GetComponent<LineRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        raycastPath(this.transform.position, this.transform.right);

        for (int i = 0; i < path.Count; i++)
            lazer.SetPosition(i, path[i]);

        charge();
    }

    List<Vector3> path = new List<Vector3>();

    // Update is called once per frame
    void Update() { }

    private void raycastPath(Vector3 origin, Vector3 direction)
    {

        if (path.Count >= reflectionCount)
            return;

        path.Add(origin);
        direction.y = 0.0f;

        RaycastHit hit;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallMask))
        {

            Vector3 reflectedDirection = Vector3.Reflect(direction, hit.normal);
            raycastPath(hit.point, reflectedDirection);

        }
        else
        {

            path.Add(direction.normalized + origin);
        }
    }


}
