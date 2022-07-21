using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private CoreGrid _coreGrid;

    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;

    void Awake()
    {
        _coreGrid = GetComponent<CoreGrid>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        FindPath(_transformA.position, _transformB.position);
    }

    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = _coreGrid.GetNodeFromWorldPos(startPos);
        Node targetNode = _coreGrid.GetNodeFromWorldPos(targetPos);
        // Node startNode = _coreGrid.grid[0,5];
        // Node targetNode = _coreGrid.grid[0,1];
        
        
        List<Node> openNodes = new List<Node>();
        HashSet<Node> closedNodes = new HashSet<Node>();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if ((openNodes[i].FCost < currentNode.FCost || openNodes[i].FCost == currentNode.FCost) && openNodes[i].HCost < currentNode.HCost)
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            if (currentNode == targetNode)
            {
                
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (var neighbour in _coreGrid.GetNeighbours(currentNode))
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

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        
        _coreGrid.path = path;
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