using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool IsReserved; // This system allows moving units to pass through each other.
    public bool IsWalkable;
    public BaseUnit ReservedUnit;
    public Vector2 WorldPosition;
    public Vector2 PivotWorldPosition;
    public Vector2Int GridIndex;
    public int GCost;
    public int HCost;
    public Node Parent;
    private int FCost => GCost + HCost;
    private int _heapIndex;

    public HashSet<Node> neighbours = new HashSet<Node>();
    public Node(bool isWalkable, Vector2 worldPosition, Vector2 pivotWorldPosition, Vector2Int gridIndex)
    {
        this.IsWalkable = isWalkable;
        this.WorldPosition = worldPosition;
        this.PivotWorldPosition = pivotWorldPosition;
        this.GridIndex = gridIndex;
    }

    public void AddNeighbour(Node node)
    {
        neighbours.Add(node);
    }




    public int HeapIndex
    {
        get { return _heapIndex; }
        set { _heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }

        return -compare;
    }
}