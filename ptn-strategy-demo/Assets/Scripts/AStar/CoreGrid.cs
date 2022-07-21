using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGrid : MonoBehaviour
{
    public LayerMask UnwalkableLayer;
    public Vector2 gridBoundSize;
    public float nodeSize;
    public Node[,] grid;

    private Vector2Int gridSize;

    private float nodeDiameter;
    public List<Node> path = new List<Node>();


    private void Awake()
    {
        CalculatedGridSize();
        CreateGrid();
    }


    private void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];

        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridBoundSize.x * .5f - Vector2.up * gridBoundSize.y * .5f;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                var nodePos = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeSize) + Vector2.up * (j * nodeDiameter + nodeSize);
                grid[i, j] = new Node(true, nodePos,new Vector2Int(i,j));
            }
        }
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
        return grid[gridIndex.x, gridIndex.y];
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
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    private void OnDrawGizmos()
    {
        //Grid Edges
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(gridBoundSize.x, gridBoundSize.y));

        if (grid != null)
        {   

            foreach (var node in grid)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(node.worldPos, Vector2.one * (nodeDiameter - .1f));
            }
            
            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count-1; i++)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(path[i].worldPos, path[i + 1].worldPos);
                }
            }
        }
    }
}