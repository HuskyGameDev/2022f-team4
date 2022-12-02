using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED- DO NOT USE

public class LazerEnemy : MonoBehaviour
{
    public float degreesPerSecond;
    public bool rotateWhenIdle;
    public bool targetingNeedsLOS;
    public bool targetingNeedsFOV;
    public float projectileSpeed;

    private Enemy _enemy;
    private GameObject _altRotationTarget;  // What to rotate towards when player is not visible

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();

        _altRotationTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.y < 0)
        {
            transform.eulerAngles.Set(0, transform.eulerAngles.y + 360, 0);
        }

        if (!_enemy.getAttackReady())
        {
            transform.Rotate(Vector3.up * Mathf.Clamp(getAngleToTarget(), -1 * degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime));
        }
    }

    public float getAngleToTarget()
    {
        float angleToTarget = 0;
        Vector3 lookVector = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.right;

        if (canTargetPlayer())
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

    public bool canTargetPlayer()
    {
        return (!targetingNeedsLOS || _enemy.PlayerInLOS()) && (!targetingNeedsFOV || _enemy.PlayerInFOV());
    }

    public void setAltRotationTarget(GameObject altRotationTarget)
    {
        _altRotationTarget = altRotationTarget;
    }
}
