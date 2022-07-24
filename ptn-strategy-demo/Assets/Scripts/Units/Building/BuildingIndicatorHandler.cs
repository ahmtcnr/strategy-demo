using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BuildingIndicatorHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _spriteParent;

    private bool isBuilding;
    private Coroutine indicatorRoutine;

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
        _spriteParent.localScale = (Vector2)baseUnitData.unitSize;
        _spriteRenderer.sprite = baseUnitData.unitSprite;

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