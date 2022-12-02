using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAttackType : MonoBehaviour, EnemyAttackType
{
    public GameObject lazerPrefab;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.setEnemyType(this);
    }

    public IEnumerator Attack()
    {
        Lazer lazer = Instantiate(lazerPrefab, transform.position, transform.rotation).GetComponent<Lazer>();
        //lazer.transform.Translate(Vector3.right);
        //lazer.transform.SetParent(null);

        yield return new WaitForSeconds(0.1f);
    }
}
