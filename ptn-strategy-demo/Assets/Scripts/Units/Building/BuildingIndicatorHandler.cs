using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BuildingIndicatorHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform spriteParent;

    [SerializeField] private LayerMask unitLayer;
    private bool _isBuilding;
    private Coroutine _indicatorRoutine;
    private BaseUnitData _currentUnitData;

    private Vector3 _cursorPos;
    private Collider2D[] _results = new Collider2D[1];

    private Color forbiddenToBuildColor = Color.red;
    private Color safeToBuildColor = Color.green;

    private void Awake()
    {
        DeactivateIndicator();
    }


    private void OnEnable()
    {
        Actions.OnBuildingUIClick += SetIndicator;
        Actions.OnDeselectBuilding += DeactivateIndicator;
    }

    private void OnDisable()
    {
        Actions.OnBuildingUIClick -= SetIndicator;
        Actions.OnDeselectBuilding -= DeactivateIndicator;
    }

    private void Update()
    {
        if (_isBuilding)
        {
            CheckUnitCollision();
        }
    }


    private void CheckUnitCollision()
    {
        _cursorPos = GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition;

        Physics2D.OverlapBoxNonAlloc((Vector2)_cursorPos + _currentUnitData.UnitSize / 2, _currentUnitData.UnitSize / 2, 0, _results, unitLayer);

        if (!Equals(_results[0], null))
        {
            _results[0] = null;
            spriteRenderer.color = forbiddenToBuildColor;
        }
        else
        {
            spriteRenderer.color = safeToBuildColor;
        }
    }

    private IEnumerator MoveIndicator()
    {
        while (true)
        {
            spriteParent.transform.position = GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition;
            yield return null;
        }
    }

    private void SetIndicator(BaseUnitData baseUnitData)
    {
        _isBuilding = true;
        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = baseUnitData.UnitSprite;
        _currentUnitData = baseUnitData;
        if (_indicatorRoutine == null)
        {
            _indicatorRoutine = StartCoroutine(MoveIndicator());
        }
    }

    private void DeactivateIndicator()
    {
        _isBuilding = false;
        spriteRenderer.gameObject.SetActive(false);

        if (_indicatorRoutine != null)
        {
            StopCoroutine(_indicatorRoutine);
            _indicatorRoutine = null;
        }
    }
}