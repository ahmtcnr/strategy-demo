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
    [SerializeField] private SpriteRenderer selectionBorderSprite;

    private BoxCollider2D _boxCollider2D;

    protected event Action OnSelected;

    protected virtual void Awake()
    {
        unitSprite.sprite = baseUnitData.UnitSprite;
        SetInitialSize();
        DisableSelectionBorder();
    }

    protected virtual void OnEnable()
    {
        OnSelected += ActivateSelectionBorder;
        Actions.OnUnitDeselected += DisableSelectionBorder;
    }

    protected virtual void OnDisable()
    {
        OnSelected -= ActivateSelectionBorder;
        Actions.OnUnitDeselected -= DisableSelectionBorder;
    }


    public void OnClickAction()
    {
        Actions.OnUnitDeselected?.Invoke();
        Actions.OnUnitSelected?.Invoke(this);
        OnSelected?.Invoke();
    }


    private void SetInitialSize()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.size = baseUnitData.UnitSize;
        _boxCollider2D.offset = (Vector2)baseUnitData.UnitSize / 2;
        selectionBorderSprite.transform.localScale = (Vector2)baseUnitData.UnitSize * 1.1f;
        selectionBorderSprite.transform.localPosition = (Vector2)baseUnitData.UnitSize / 2;
    }

    private void DisableSelectionBorder() => selectionBorderSprite.gameObject.SetActive(false);

    private void ActivateSelectionBorder() => selectionBorderSprite.gameObject.SetActive(true);
}