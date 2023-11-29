using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    private EnemyPathNode[] _pathNodes;

    private class PathNodeComparer : IComparer
    {
        int IComparer.Compare(System.Object x, System.Object y)
        {
            return ((EnemyPathNode)x).order - ((EnemyPathNode)y).order;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Sort children
        _pathNodes = GetComponentsInChildren<EnemyPathNode>();
        Array.Sort(_pathNodes, new PathNodeComparer());
        //Debug.Log(_pathNodes[0].order);
        //Debug.Log(_pathNodes[1].order);
    }

    public EnemyPathNode getNodeZero()
    {
        return _pathNodes[0];
    }
    public EnemyPathNode getNextNode(EnemyPathNode reachedNode)
    {
        return _pathNodes[(reachedNode.order + 1) % _pathNodes.Length];
    }

    public EnemyPathNode getNode(int nodeNum)
    {
        return _pathNodes[nodeNum];
    }
}
