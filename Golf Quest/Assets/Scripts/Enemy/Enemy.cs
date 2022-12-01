using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Times at which enemy can attack. Not mutually exclusive. Start/end times are subjust to needsLOS, and will start cooldown if attack occurs
    public bool turnStartAttack;    // Attempt to attack as soon as ball it hit
    public bool turnEndAttack;      // Attempt to attack as soon as ball comes to a stop
    public bool anyTimeAttack;      // Will attack as much as possible according to line of sight, cooldown, and attacks per turn

    // Conditions that determine when attack occurs
    public float attackCooldown;    // Time between attacks in seconds
    public bool attackNeedsLOS;     // Player must be in line of sight to attack
    public bool attackNeedsFOV;     // Player must be in field of view
    public float fov;               // Field of view in angles
    public int maxTurnAttacks;      // Maximum attacks which can occur within a turn, including start/end attacks. -1 for infinite

    // Reference to player ball
    private GameObject _playerBall;
    private bool _attackReady;      // True if cooldown has passed since last attack
    private int _attacksElapsed;    // Number of attacks which have occured this turn

    // Reference to ExitHoleManager
    private ExitHoleManager exitHole;

    // Enemy type for attack coroutine
    private EnemyType _enemyType;

    /*
    void Awake()
    {
        _playerBall = GameObject.FindGameObjectWithTag("Player");
        exitHole = GameObject.Find("ExitHole").GetComponent<ExitHoleManager>();

        
        Debug.Log("monobehaviors: " + GetComponents<MonoBehaviour>());
        foreach (MonoBehaviour m in GetComponents<MonoBehaviour>())
        {
            if (m.GetType() == typeof(EnemyType))
            {
                _enemyType = (EnemyType)m;
            }
        }
        Debug.Log(_enemyType);
    }*/

    private void Update()
    {
        if (anyTimeAttack && CanAttackPlayer())
        {
            //Debug.Log("Any Time Attack");
            Attack();
        }
    }

    // Triggered when GameObject is set to active at start of turn
    private void Awake()
    {
        _playerBall = GameObject.FindGameObjectWithTag("Player");
        exitHole = GameObject.Find("ExitHole").GetComponent<ExitHoleManager>();

        _attackReady = true;
        _attacksElapsed = 0;

        // Set up layermask to ignore projectiles and other non-blocking objects
        //int layerMask = DefaultRaycastLayers;
        // originalLayerMask &= ~(1 << layerToRemove);

        if (turnStartAttack && CanAttackPlayer())
        {
            //Debug.Log("Turn Start Attack");
            Attack();
        }
    }

    // Determine if enemy can attack
    private bool CanAttackPlayer()
    {
        return _attackReady                                                 // Ready to attack
            && ((_attacksElapsed < maxTurnAttacks) || maxTurnAttacks < 0)   // Has turn attacks remaining
            && CanSeePlayer();
    }

    public bool CanSeePlayer()
    {
        return (!attackNeedsLOS || PlayerInLOS()) && (!attackNeedsFOV || PlayerInFOV());
    }

    // Override this with actual attack behavior, then call super.attack() to handle attack readiness and cooldown
    protected void Attack()
    {
        if(_enemyType == null)
        return;

        _attackReady = false;
        StartCoroutine(_enemyType.Attack()); 
        _attacksElapsed++;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _attackReady = true;
    }

    // Get normalized vector from enemy's position to player's position
    public Vector3 GetPlayerVector()
    {
        return Vector3.Normalize(_playerBall.transform.position - transform.position);
    }

    public bool PlayerInLOS()
    {
        Ray playerBallRay = new Ray(transform.position, GetPlayerVector());
        RaycastHit hitInfo;

        if (Physics.Raycast(playerBallRay, out hitInfo) && hitInfo.transform == _playerBall.transform)
        {
            return true;
        }

        return false;
    }

    public bool PlayerInFOV()
    {
        Vector3 playerVector = GetPlayerVector();
        Vector3 lookVector = Vector3.Normalize(transform.TransformDirection(Vector3.right));

        //Debug.Log("Player Vector: " + playerVector + " Look Vector: " + lookVector + " Angle: " + angle);
        return Vector3.Angle(lookVector, playerVector) <= fov / 2;
    }

    public void setEnemyType(EnemyType enemyType)
    {
        _enemyType = enemyType;
        //Debug.Log(_enemyType);
    }

    public bool getAttackReady()
    {
        return _attackReady;
    }
}
