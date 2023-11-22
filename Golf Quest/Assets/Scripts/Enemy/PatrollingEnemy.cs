using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public EnemyPath enemyPath1;
    public EnemyPath bossOnlyPath2;
    private EnemyPath enemyPath;
    public int startNode;
    public float reachNodeDistance;
    public float nodeWaitTime;
    public bool stopWhenTargettingPlayer;
    [SerializeField] public int bossMovesWhenEnemiesRemaining; // enemies left for boss to move to his new path
    
    private EnemyPathNode _targetNode;
    private NavMeshAgent _navMeshAgent;
    private Enemy _enemy;
    private RotatingEnemy _rotatingEnemy;
    private float _navMeshAgentSpeed;   // Speed of navMeshAgent set in inspector. Used to restore after stopping to attack
    private bool isBoss;
    private int bossPath;
    private GameObject[] allEnemies;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize path following
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _rotatingEnemy = GetComponent<RotatingEnemy>();

        _navMeshAgentSpeed = _navMeshAgent.speed;
        _navMeshAgent.angularSpeed = _rotatingEnemy.degreesPerSecond;
        enemyPath = enemyPath1;
        setTargetNode(enemyPath.getNode(startNode));
        
        if(bossOnlyPath2 != null) {
            isBoss = true;
            allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            bossPath = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distToTarget = Vector3.Distance(transform.position, _targetNode.transform.position);
        if (distToTarget <= reachNodeDistance)
        {
            setTargetNode(enemyPath.getNextNode(_targetNode));
        }

        if (stopWhenTargettingPlayer && _rotatingEnemy.canTargetPlayer())
        {
            _navMeshAgent.speed = 0;
        }
        else if (_navMeshAgent.speed == 0)
        {
            _navMeshAgent.speed = _navMeshAgentSpeed;
        }

        if(isBoss) {
            allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            // If the boss has not switched paths, see if enough enemies have been killed to switch
            if(bossPath != 2 && allEnemies.Length <= bossMovesWhenEnemiesRemaining ) {
               enemyPath = bossOnlyPath2;
               bossPath = 2;
            }
        }
        
    }
    public IEnumerator WaitAtNode()
    {
        yield return new WaitForSeconds(nodeWaitTime);
        _navMeshAgent.SetDestination(_targetNode.transform.position);
    }

    private void setTargetNode(EnemyPathNode targetNode)
    {
        _targetNode = targetNode;

        if (_rotatingEnemy != null)
        {
            _rotatingEnemy.setAltRotationTarget(_targetNode.gameObject);
        }

        StartCoroutine(WaitAtNode());
    }
}
