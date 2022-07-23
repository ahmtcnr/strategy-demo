using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector2 WorldPosition;
    public Vector2 PivotWorldPosition;


    public Vector2Int gridIndex;


    public int GCost;
    public int HCost;
    public Node parent;
    public int FCost => GCost + HCost;


    public Node(bool isWalkable, Vector2 worldPosition, Vector2 pivotWorldPosition, Vector2Int gridIndex)
    {
        this.isWalkable = isWalkable;
        this.WorldPosition = worldPosition;
        this.PivotWorldPosition = pivotWorldPosition;
        this.gridIndex = gridIndex;
    }
}