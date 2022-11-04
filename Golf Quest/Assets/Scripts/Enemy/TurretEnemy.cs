using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour, EnemyType
{
    public float degreesPerSecond;
    public bool rotateWhenIdle;
    public GameObject projectile;

    private Enemy _enemy;
    private GameObject _altRotationTarget;  // What to rotate towards when player is not visible

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.setEnemyType(this);

        _altRotationTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.y < 0)
        {
            transform.eulerAngles.Set(0, transform.eulerAngles.y + 360, 0);
        }

        transform.Rotate(Vector3.up * Mathf.Clamp(getAngleToTarget(), -1 * degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime));
    }

    public float getAngleToTarget()
    {
        float angleToTarget = 0;
        Vector3 lookVector = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.right;

        if (_enemy.PlayerInLOS())
        {
            angleToTarget = Vector3.SignedAngle(lookVector, _enemy.GetPlayerVector(), Vector3.up);
        }
        else if (_altRotationTarget != null)
        {
            Vector3 targetVector = Vector3.Normalize(_altRotationTarget.transform.position - transform.position);
            angleToTarget = Vector3.SignedAngle(lookVector, targetVector, Vector3.up);
        }
        else if (rotateWhenIdle)
        {
            angleToTarget = 90.0f;
        }

        return angleToTarget;
    }

    public void setAltRotationTarget(GameObject altRotationTarget)
    {
        _altRotationTarget = altRotationTarget;
    }

    public IEnumerator Attack()
    {
        Debug.Log("Turret Attack!");

        GameObject Projectile = Instantiate(projectile, transform, false);
        Projectile.transform.Translate(Vector3.right * 0.6f);
        Projectile.transform.SetParent(null);

        yield return new WaitForSeconds(0.1f);
    }
}
