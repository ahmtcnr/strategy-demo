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


    protected virtual void Awake()
    {
        unitSprite.sprite = baseUnitData.unitSprite;
    }

    public void OnClickAction()
    {
        Actions.OnUnitSelected?.Invoke(baseUnitData);
    }
}