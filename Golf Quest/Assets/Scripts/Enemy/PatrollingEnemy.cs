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
    
    private EnemyPathNode _targetNode;
    private NavMeshAgent _navMeshAgent;
    private TurretEnemy _turretEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize path following
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _turretEnemy = GetComponent<TurretEnemy>();
        setTargetNode(enemyPath.getNode(startNode));

        // If node wait time = -1, set it to as much time as is needed to face next node
        // WIP DO NOT USE YET
        if (nodeWaitTime < 0)
        {
            if (_turretEnemy != null)
            {
                nodeWaitTime = _turretEnemy.getAngleToTarget() / _turretEnemy.degreesPerSecond;
            }
            else
            {
                nodeWaitTime = 0;
            }
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
    }
    public IEnumerator WaitAtNode()
    {
        yield return new WaitForSeconds(nodeWaitTime);
        _navMeshAgent.SetDestination(_targetNode.transform.position);
    }

    private void setTargetNode(EnemyPathNode targetNode)
    {
        _targetNode = targetNode;

        if (_turretEnemy != null)
        {
            _turretEnemy.setAltRotationTarget(_targetNode.gameObject);
        }

        StartCoroutine(WaitAtNode());
    }
}
