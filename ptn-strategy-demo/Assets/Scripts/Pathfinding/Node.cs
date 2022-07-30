using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isWalkable;
    public bool isReserved;
    public BaseUnit reservedUnit;
    
    public Vector2 WorldPosition;
    public Vector2 PivotWorldPosition;



    public Vector2Int gridIndex;


    public int GCost;
    public int HCost;
    public Node parent;
    public int FCost => GCost + HCost;
    private int _heapIndex;

    public HashSet<Node> neighbours = new HashSet<Node>();
    public Node(bool isWalkable, Vector2 worldPosition, Vector2 pivotWorldPosition, Vector2Int gridIndex)
    {
        this.isWalkable = isWalkable;
        this.WorldPosition = worldPosition;
        this.PivotWorldPosition = pivotWorldPosition;
        this.gridIndex = gridIndex;
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