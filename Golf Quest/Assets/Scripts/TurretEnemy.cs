using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour, EnemyType
{
    public float degreesPerSecond;

    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.setEnemyType(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy.PlayerVisible())
        {
            if (transform.eulerAngles.y < 0)
            {
                transform.eulerAngles.Set(0, transform.eulerAngles.y + 360, 0);
            }
            Vector3 lookVector = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.right;
            float angleToPlayer = Vector3.SignedAngle(lookVector, _enemy.GetPlayerVector(), Vector3.up);
            //Debug.Log(angleToPlayer);
            transform.Rotate(Vector3.up * Mathf.Clamp(angleToPlayer, -1 * degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime));
        }
    }

    public IEnumerator Attack()
    {
        Debug.Log("Turret Attack!");
        yield return new WaitForSeconds(0.1f);
    }
}
