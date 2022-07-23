using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private void OnEnable()
    {
        Actions.OnBuildSuccess += Build;
    }

    private void OnDisable()
    {
        Actions.OnBuildSuccess -= Build;
    }
    private void Build(BaseBuilding currentBuilding)
    {
       Instantiate(currentBuilding, GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition, Quaternion.identity);

        GridSystem.Instance.SetNodesWalkableStatus(false, currentBuilding.buildingData.buildingSize);
    }
}