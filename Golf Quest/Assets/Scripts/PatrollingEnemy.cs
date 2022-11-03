using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public EnemyPath enemyPath;
    public float reachNodeDistance;
    public float nodeWaitTime;
    
    private EnemyPathNode _targetNode;
    private NavMeshAgent _navMeshAgent; 

    // Start is called before the first frame update
    void Start()
    {
        // Initialize path following
        _navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetNode(enemyPath.getFirstNode());
        
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
        StartCoroutine(WaitAtNode());
    }
}
