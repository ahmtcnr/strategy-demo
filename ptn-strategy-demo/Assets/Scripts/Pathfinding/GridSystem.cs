using System.Collections.Generic;
using UnityEngine;

public class GridSystem : Singleton<GridSystem>
{   
    public Vector2 gridBoundSize;
    public float nodeSize;
    
    
    [SerializeField] private bool isDrawGizmos;
    private Node[,] _nodes;
    private Vector2Int gridSize;
    private float nodeDiameter;



    protected override void Awake()
    {
        base.Awake();
        CalculatedGridSize();
        CreateGrid();
    }
    private void CreateGrid()
    {
        _nodes = new Node[gridSize.x, gridSize.y];

        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridBoundSize.x * .5f - Vector2.up * gridBoundSize.y * .5f;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                var nodePos = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeSize) + Vector2.up * (j * nodeDiameter + nodeSize);

                _nodes[i, j] = new Node(true, nodePos, nodePos + ((Vector2.down + Vector2.left) * nodeSize), new Vector2Int(i, j));
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


    private List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.GridIndex.x + x;
                int checkY = node.GridIndex.y + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbours.Add(_nodes[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    private void CacheAllNodesNeighbours()// This method is to prevent pathfinding from constantly calculating neighbors
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                foreach (var neighbour in GetNeighbours(_nodes[x, y]))
                {
                    _nodes[x, y].AddNeighbour(neighbour);
                }
            }
        }
    }

  

    public Node GetNodeFromWorldPos(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridBoundSize.x / 2) / gridBoundSize.x;
        float percentY = (worldPosition.y + gridBoundSize.y / 2) / gridBoundSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(Mathf.Min(gridSize.x * percentX, gridSize.x - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSize.y * percentY, gridSize.y - 1));
        return _nodes[x, y];
    }

    public bool IsNodesEmpty(Vector2Int checkSize)
    {
        var startNode = GetNodeOnCursor();
        checkSize += startNode.GridIndex;
        if (!startNode.IsWalkable || checkSize.x > gridSize.x || checkSize.y > gridSize.y)
            return false;


        for (int x = startNode.GridIndex.x; x < checkSize.x; x++)
        {
            for (int y = startNode.GridIndex.y; y < checkSize.y; y++)
            {
                if (!_nodes[x, y].IsWalkable)
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
        nodeDimension += startNode.GridIndex;

        for (int x = startNode.GridIndex.x; x < nodeDimension.x; x++)
        {
            for (int y = startNode.GridIndex.y; y < nodeDimension.y; y++)
            {
                _nodes[x, y].IsWalkable = status;
            }
        }
    }

    private int iterationCount = 0;

    public bool TryGetNearestWalkableNode(Vector2 worldPos, out Node node)
    {
        Node startNode = GetNodeFromWorldPos(worldPos);

        if (!startNode.IsReserved && startNode.IsWalkable )
        {
            node = startNode;
            return true;
        }

        Vector2Int checkIndex = Vector2Int.zero;
        int max = 0;
        int min = 0;
        int maxValue = Mathf.Max(gridSize.x, gridSize.y);
        while (true)
        {
            for (int x = min; x < max; x++)
            {
                for (int y = min; y < max; y++)
                {
                    checkIndex.x = x + startNode.GridIndex.x;
                    checkIndex.y = y + startNode.GridIndex.y;
                    if (checkIndex.x < 0 || checkIndex.x >= gridSize.x || checkIndex.y < 0 || checkIndex.y >= gridSize.y)
                        continue;


                    if (_nodes[checkIndex.x, checkIndex.y].IsWalkable && !_nodes[checkIndex.x, checkIndex.y].IsReserved)
                    {
                        node = _nodes[checkIndex.x, checkIndex.y];
                        return true;
                    }
                }
            }


            max++;
            min--;


            if (min <= -maxValue - 1)
            {
                node = null;
                return false;
            }
        }
    }


    public Node GetNodeOnCursor() => GetNodeFromWorldPos(InputInteraction.Instance.GetMouseToWorldPosition());

    public int MaxGridSize => gridSize.x * gridSize.y;

   

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (!isDrawGizmos) return;

        //Grid Edges
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(gridBoundSize.x, gridBoundSize.y));

        if (_nodes != null)
        {
            foreach (var node in _nodes)
            {
                if (!node.IsWalkable)
                {
                    Gizmos.color = Color.magenta;
                }
                else
                {
                    Gizmos.color = Color.gray;
                }

                if (node.IsReserved)
                {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawCube(node.WorldPosition, Vector2.one * (nodeDiameter - 0.01f));
            }

        }
    }

    #endregion
}