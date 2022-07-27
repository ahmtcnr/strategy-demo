
using UnityEngine;

public abstract class BaseBuilding : BaseUnit
{
    private BoxCollider2D _boxCollider2D;
    
    protected new virtual void Awake()
    {
        base.Awake();
        
        SetColliderSize();
    }


    private void SetColliderSize()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.size = baseUnitData.unitSize;
        _boxCollider2D.offset = baseUnitData.unitSize / 2;
    }

}