using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, ISelectable
{
    [SerializeField] public BuildingData buildingData;

    [SerializeField] private Transform spriteParent;
    


    private void Awake()
    {
        SetBuildingData();
    }


    public void OnClickAction()
    {
    }

    private void SetBuildingData()
    {
        spriteParent.localScale = (Vector2)buildingData.buildingSize;

    }
}