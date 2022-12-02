using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackType : MonoBehaviour, EnemyAttackType
{
    public float projectileSpeed;
    public GameObject projectilePrefab;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.setEnemyType(this);
    }
    public IEnumerator Attack()
    {
        //Debug.Log("Turret Attack!");

        Projectile projectile = Instantiate(projectilePrefab, transform, false).GetComponent<Projectile>();
        projectile.transform.Translate(Vector3.right * 0.6f);
        projectile.transform.SetParent(null);

        if (projectileSpeed > 0)
        {
            projectile.Speed = projectileSpeed;
        }

        yield return new WaitForSeconds(0.1f);
    }
}
