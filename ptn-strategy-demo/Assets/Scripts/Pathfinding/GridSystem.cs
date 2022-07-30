using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : Singleton<GridSystem>
{
    public LayerMask UnwalkableLayer;
    public Vector2 gridBoundSize;
    public float nodeSize;
    public Node[,] nodes;

    private Vector2Int gridSize;

    private float nodeDiameter;
    public List<Node> path = new List<Node>();

    [SerializeField] private bool isDrawGizmos;


    protected override void Awake()
    {
        base.Awake();
        CalculatedGridSize();
        CreateGrid();
    }


    private void CreateGrid()
    {
        nodes = new Node[gridSize.x, gridSize.y];

        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridBoundSize.x * .5f - Vector2.up * gridBoundSize.y * .5f;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                var nodePos = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeSize) + Vector2.up * (j * nodeDiameter + nodeSize);

                nodes[i, j] = new Node(true, nodePos, nodePos + ((Vector2.down + Vector2.left) * nodeSize), new Vector2Int(i, j));
            }
        }

        CacheAllNodesNeighbours();
    }

    private void CalculatedGridSize()
    {
        nodeDiameter = nodeSize * 2;
        gridSize.x = Mathf.RoundToInt(gridBoundSize.x / nodeDiameter);
        gridSize.y = Mathf.RoundToInt(gridBoundSize.y / nodeDiameter);
    }


    public Node GetNodeFromWorldPos(Vector2 worldPos)
    {
        Vector2Int gridIndex = new Vector2Int();
        float percentX = Mathf.Clamp01((worldPos.x + gridBoundSize.x * .5f) / gridBoundSize.x);
        float percentY = Mathf.Clamp01((worldPos.y + gridBoundSize.y * .5f) / gridBoundSize.y);
        gridIndex.x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        gridIndex.y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
        return nodes[gridIndex.x, gridIndex.y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridIndex.x + x;
                int checkY = node.gridIndex.y + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public bool IsNodesEmpty(Vector2Int checkSize)
    {
        var startNode = GetNodeOnCursor();
        checkSize += startNode.gridIndex;
        if (!startNode.isWalkable || checkSize.x > gridSize.x || checkSize.y > gridSize.y)
            return false;


        for (int x = startNode.gridIndex.x; x < checkSize.x; x++)
        {
            for (int y = startNode.gridIndex.y; y < checkSize.y; y++)
            {
                if (!nodes[x, y].isWalkable)
                {
                    //Debug.Log("There is a building");
                    return false;
                }
            }
        }

        return true;
    }

    public void SetNodesWalkableStatus(bool status, Vector2Int nodeDimension)
    {
        var startNode = GetNodeOnCursor();
        nodeDimension += startNode.gridIndex;

        for (int x = startNode.gridIndex.x; x < nodeDimension.x; x++)
        {
            for (int y = startNode.gridIndex.y; y < nodeDimension.y; y++)
            {
                nodes[x, y].isWalkable = status;
            }
        }
    }


    public bool TryGetNearestWalkableNode(Vector2 worldPos, out Node node)
    {
        Node startNode = GetNodeFromWorldPos(worldPos);

        Vector2Int checkIndex = Vector2Int.zero;
        int max = 1;
        int min = -1;

        while (true)
        {
            for (int x = min; x < max; x++)
            {
                for (int y = min; y < max; y++)
                {
                    checkIndex.x = x + startNode.gridIndex.x;
                    checkIndex.y = y + startNode.gridIndex.y;
                    if (checkIndex.x < 0 || checkIndex.x >= gridSize.x || checkIndex.y < 0 || checkIndex.y >= gridSize.y)
                    {
                        break;
                    }

                    if (nodes[checkIndex.x, checkIndex.y].isWalkable)
                    {
                        node = nodes[checkIndex.x, checkIndex.y];
                        return true;
                    }
                }
            }

            max++;
            min--;
        }

        node = GetNodeFromWorldPos(worldPos);
        return false;
    }

    private void CacheAllNodesNeighbours()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                foreach (var neighbour in GetNeighbours(nodes[x, y]))
                {
                    nodes[x, y].AddNeighbour(neighbour);
                }
            }
        }
    }

    public Node GetNodeOnCursor() => GetNodeFromWorldPos(InputInteraction.Instance.GetMouseToWorldPosition());

    public int MaxGridSize => gridSize.x * gridSize.y;


    private void OnDrawGizmos()
    {
        if (!isDrawGizmos) return;

        //Grid Edges
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(gridBoundSize.x, gridBoundSize.y));

        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                if (!node.isWalkable)
                {
                    Gizmos.color = Color.magenta;
                }
                else
                {
                    Gizmos.color = Color.gray;
                }

                Gizmos.DrawCube(node.WorldPosition, Vector2.one * (nodeDiameter - 0.01f));
            }

            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(path[i].WorldPosition, path[i + 1].WorldPosition);
                }
            }
        }
    }
}