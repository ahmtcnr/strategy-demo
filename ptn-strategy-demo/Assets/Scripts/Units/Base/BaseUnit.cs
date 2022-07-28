using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, ISelectable
{
    [SerializeField] public BaseUnitData baseUnitData;
    [SerializeField] private SpriteRenderer unitSprite;
    [SerializeField] protected Transform spriteParent;
    
    
    private BoxCollider2D _boxCollider2D;
   
    protected virtual void Awake()
    {
        unitSprite.sprite = baseUnitData.unitSprite;
        SetColliderSize();
    }

    public void OnClickAction()
    {
        Actions.OnUnitSelected?.Invoke(baseUnitData);
    }
    
    private void SetColliderSize()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.size = baseUnitData.unitSize;
        _boxCollider2D.offset = (Vector2)baseUnitData.unitSize / 2;
    }

}