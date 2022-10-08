using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public float degreesPerSecond;

    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy.PlayerVisible())
        {
            //Debug.Log(Vector3.Angle(Vector3.right, _enemy.GetPlayerVector()) - transform.eulerAngles.y);

            //Quaternion lookRotation = Quaternion.LookRotation(_enemy.GetPlayerVector());
            //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, degreesPerSecond);
            //Debug.Log(Vector3.Angle(Vector3.right, _enemy.GetPlayerVector()) - transform.eulerAngles.y);
            transform.Rotate(Vector3.up, Mathf.Clamp(Vector3.Angle(Vector3.right, _enemy.GetPlayerVector()) - transform.eulerAngles.y, -1 * degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime));
            //Vector3 newRotate = Vector3.RotateTowards(Rotation Vector3.right, playerBall.transform.position, degreesPerSecond * Time.fixedDeltaTime, 0.0f);
            //transform.eulerAngles = Vector3.MoveTowards(transform.rotation.eulerAngles, Vector3.down * Vector3.Angle(Vector3.right, playerBall.transform.position - transform.position), degreesPerSecond * Time.deltaTime);
            //transform.Rotate(Vector3.down * Mathf.Clamp(Vector3.Angle(Vector3.right, _enemy.GetPlayerVector()) - transform.eulerAngles.y, -1 * degreesPerSecond, degreesPerSecond));
            //Debug.Log(Vector3.RotateTowards(transform.rotation.eulerAngles, playerBall.transform.position - transform.position, RaidansPerSecond * Time.deltaTime, 100.0f));
        }
    }
}
