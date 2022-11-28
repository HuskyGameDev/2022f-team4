using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public EnemyPath enemyPath;
    public int startNode;
    public float reachNodeDistance;
    public float nodeWaitTime;
    public bool stopWhenTargettingPlayer;
    
    private EnemyPathNode _targetNode;
    private NavMeshAgent _navMeshAgent;
    private Enemy _enemy;
    private RotatingEnemy _rotatingEnemy;
    private float _navMeshAgentSpeed;   // Speed of navMeshAgent set in inspector. Used to restore after stopping to attack

    // Start is called before the first frame update
    void Start()
    {
        // Initialize path following
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _rotatingEnemy = GetComponent<RotatingEnemy>();

        _navMeshAgentSpeed = _navMeshAgent.speed;
        _navMeshAgent.angularSpeed = _rotatingEnemy.degreesPerSecond;
        setTargetNode(enemyPath.getNode(startNode));
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
