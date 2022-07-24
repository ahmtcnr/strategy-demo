
using UnityEngine;

public abstract class BaseBuilding : BaseUnit
{
    private void Awake()
    {
        SetBuildingData();
    }
    private void SetBuildingData()
    {
        spriteParent.localScale = (Vector2)baseUnitData.unitSize;

    }
    
}