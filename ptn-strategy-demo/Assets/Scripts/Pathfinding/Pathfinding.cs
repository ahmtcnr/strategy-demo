using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Pathfinding : Singleton<Pathfinding>
{
    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;

    private GridSystem _gridSystem;


    private void Start()
    {
        _gridSystem = GridSystem.Instance;
    }


    private void OnEnable()
    {
        Actions.OnSpaceBarDown += Test;
    }

    private void OnDisable()
    {
        Actions.OnSpaceBarDown -= Test;
    }

    private void Test()
    {
        //FindPath(_transformA.position, _transformB.position);
    }

    public void StartFindPath(Vector3 startPos, Node targetNode)
    {
        StartCoroutine(FindPath(startPos, targetNode));
    }

    private IEnumerator FindPath(Vector2 startPos, Node targetNode)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _gridSystem.GetNodeFromWorldPos(startPos);
        if (startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<Node> openNodes = new Heap<Node>(_gridSystem.MaxGridSize);
            HashSet<Node> closedNodes = new HashSet<Node>();
            openNodes.Add(startNode);
            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.RemoveFirst();

                closedNodes.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    
                    break;
                }

                foreach (var neighbour in currentNode.neighbours)
                {
                    if (!neighbour.isWalkable || closedNodes.Contains(neighbour))
                    {
                        continue;
                    }

                    int movementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (movementCostToNeighbour < neighbour.GCost || !openNodes.Contains(neighbour))
                    {
                        neighbour.GCost = movementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        PathRequestManager.Instance.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridIndex.x - path[i].gridIndex.x, path[i - 1].gridIndex.y - path[i].gridIndex.y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].PivotWorldPosition);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }


    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridIndex.x - nodeB.gridIndex.x);
        int dstY = Mathf.Abs(nodeA.gridIndex.y - nodeB.gridIndex.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}