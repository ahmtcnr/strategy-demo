using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BaseBuilding currentBuilding;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Build);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Build);
    }

    private void Build()
    {
        Actions.OnBuildingUIClick?.Invoke(currentBuilding.baseUnitData);
        Actions.OnBuildingClick?.Invoke(currentBuilding);
    }
}