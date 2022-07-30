using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using Units.Base;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, ISelectable
{
    [SerializeField] public BaseUnitData baseUnitData;
    [SerializeField] private SpriteRenderer unitSprite;
    [SerializeField] protected Transform spriteParent;
    
    
    private BoxCollider2D _boxCollider2D;
   
    protected virtual void Awake()
    {
        unitSprite.sprite = baseUnitData.UnitSprite;
        SetColliderSize();
    }

    public void OnClickAction()
    {
        Actions.OnUnitSelected?.Invoke(this);
    }
    
    private void SetColliderSize()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.size = baseUnitData.UnitSize;
        _boxCollider2D.offset = (Vector2)baseUnitData.UnitSize / 2;
    }

}