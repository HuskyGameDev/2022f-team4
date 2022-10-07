using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Times at which enemy can attack. Not mutually exclusive. Start/end times are subjust to needsLOS, and will start cooldown if attack occurs
    public bool turnStartAttack;    // Attempt to attack as soon as ball it hit
    public bool turnEndAttack;      // Attempt to attack as soon as ball comes to a stop
    public bool anyTimeAttack;      // Will attack as much as possible according to line of sight, cooldown, and attacks per turn

    // Conditions that determine when attack occurs
    public float attackCooldown;    // Time between attacks in seconds
    public bool needsLOS;           // Player must be in line of sight to attack
    public int maxTurnAttacks;      // Maximum attacks which can occur within a turn, including start/end attacks. -1 for infinite

    // Reference to player ball
    public BallMovement playerBall;

    private bool _attackReady;      // True if cooldown has passed since last attack
    private int _attacksElapsed;    // Number of attacks which have occured this turn

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (anyTimeAttack && CanAttack())
        {
            Debug.Log("Any Time Attack");
            Attack();
        }
    }

    // Triggered when GameObject is set to active at start of turn
    private void Awake()
    {
        _attackReady = true;
        _attacksElapsed = 0;

        if (turnStartAttack && CanAttack())
        {
            Debug.Log("Turn Start Attack");
            Attack();
        }
    }

    // Determine if enemy can attack
    private bool CanAttack()
    {
        if (_attackReady && ((_attacksElapsed < maxTurnAttacks) || maxTurnAttacks < 0) )
        {
            if (needsLOS)
            {
                Ray playerBallRay = new Ray(transform.position, playerBall.transform.position -transform.position);
                RaycastHit hitInfo;

                if (!Physics.Raycast(playerBallRay, out hitInfo) || hitInfo.transform != playerBall.transform)
                {
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    // Override this with actual attack behavior, then call super.attack() to handle attack readiness and cooldown
    protected void Attack()
    {
        Debug.Log("I'm Attacking!");
        _attackReady = false;
        _attacksElapsed++;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _attackReady = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
