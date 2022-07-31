using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BuildingIndicatorHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _spriteParent;

    [SerializeField] private LayerMask unitLayer;
    private bool isBuilding;
    private Coroutine indicatorRoutine;
    private BaseUnitData _currentUnitData;

    private Vector3 _cursorpos;

    private Collider2D[] results = new Collider2D[1];

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
        if (isBuilding)
        {
            MyCollisions();
        }
    }


    void MyCollisions()
    {
        _cursorpos = GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition;

        Physics2D.OverlapBoxNonAlloc((Vector2)_cursorpos + _currentUnitData.UnitSize / 2, _currentUnitData.UnitSize / 2, 0, results, unitLayer);

        if (!Equals(results[0], null))
        {
            results[0] = null;
            _spriteRenderer.color = forbiddenToBuildColor;
        }
        else
        {
            _spriteRenderer.color = safeToBuildColor;
        }
    }

    private IEnumerator MoveIndicator()
    {
        while (true)
        {
            _spriteParent.transform.position = GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition;
            yield return null;
        }
    }

    private void SetIndicator(BaseUnitData baseUnitData)
    {
        isBuilding = true;
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = baseUnitData.UnitSprite;
        _currentUnitData = baseUnitData;
        if (indicatorRoutine == null)
        {
            indicatorRoutine = StartCoroutine(MoveIndicator());
        }
    }

    private void DeactivateIndicator()
    {
        isBuilding = false;
        _spriteRenderer.gameObject.SetActive(false);

        if (indicatorRoutine != null)
        {
            StopCoroutine(indicatorRoutine);
            indicatorRoutine = null;
        }
    }
}