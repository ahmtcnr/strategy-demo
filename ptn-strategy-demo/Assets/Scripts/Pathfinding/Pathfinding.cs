using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Pathfinding : Singleton<Pathfinding>
{

    private GridSystem _gridSystem;


    private void Start()
    {
        _gridSystem = GridSystem.Instance;
    }

    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    private IEnumerator FindPath(Vector2 startPos, Vector2 endPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _gridSystem.GetNodeFromWorldPos(startPos);
        Node targetNode = _gridSystem.GetNodeFromWorldPos(endPos);


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
                if (!neighbour.IsWalkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }

                int movementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (movementCostToNeighbour < neighbour.GCost || !openNodes.Contains(neighbour))
                {
                    neighbour.GCost = movementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;
                    if (!openNodes.Contains(neighbour))
                    {
                        openNodes.Add(neighbour);
                    }
                    else
                    {
                        openNodes.UpdateItem(neighbour);
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
            currentNode = currentNode.Parent;
        }

        Vector3[] waypoints = NodeListToArray(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] NodeListToArray(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        foreach (var node in path)
        {
            waypoints.Add(node.PivotWorldPosition);
        }

        return waypoints.ToArray();
    }


    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridIndex.x - nodeB.GridIndex.x);
        int dstY = Mathf.Abs(nodeA.GridIndex.y - nodeB.GridIndex.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}