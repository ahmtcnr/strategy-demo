using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public GridSystem _gridSystem;//TODO: Restructure this

    public BuildingData buildingData;

    public GameObject test;
    
    public event Action<BuildingData> OnBuilding;


    public bool isClicked;

    private void OnEnable()
    {
        OnBuilding += Building;
    }

    private void OnDisable()
    {
        OnBuilding -= Building;
    }


    private void Update()
    {
        if (isClicked)
        {
            test.transform.position = _gridSystem.GetNodeFromWorldPos(InputManager.Instance.GetMouseToWorldPosition()).worldPos;
        }
       
    }

    //TODO: Call when only mousepos changes
    private void Building(BuildingData bd)
    {
        
    }

}
