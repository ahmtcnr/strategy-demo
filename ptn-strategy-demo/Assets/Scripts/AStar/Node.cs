using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector2 worldPos;

    public Vector2Int gridIndex;
    
    
    public int GCost;
    public int HCost;
    public Node parent;
    public int FCost => GCost + HCost;


    public Node(bool isWalkable, Vector2 worldPos, Vector2Int gridIndex)
    {
        this.isWalkable = isWalkable;
        this.worldPos = worldPos;
        this.gridIndex = gridIndex;
    }


}