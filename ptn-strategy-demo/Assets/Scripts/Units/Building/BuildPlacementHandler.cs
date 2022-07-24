using System;
using UnityEngine;

namespace Building
{
    public class BuildPlacementHandler : MonoBehaviour
    {
        private bool canBuild;
        private BaseBuilding _currentBuilding;

        private void OnEnable()
        {
            Actions.OnBuildingClick += SetBuildStatus;
            Actions.OnLeftClick += CheckConditions;
            Actions.OnDeselectBuilding += Deselect;
        }

        private void OnDisable()
        {
            Actions.OnBuildingClick -= SetBuildStatus;
            Actions.OnLeftClick -= CheckConditions;
            Actions.OnDeselectBuilding -= Deselect;
        }

        private void SetBuildStatus(BaseBuilding baseBuilding)
        {
            _currentBuilding = baseBuilding;
            canBuild = true;
        }

        private void Deselect()
        {
            _currentBuilding = null;
            canBuild = false;
        }

        private void CheckConditions()
        {
            if (!canBuild)
                return;

            if (GridSystem.Instance.IsNodesEmpty(_currentBuilding.baseUnitData.unitSize))
            {
                Actions.OnBuildSuccess?.Invoke(_currentBuilding);
                Actions.OnDeselectBuilding?.Invoke();
            }
        }
    }
}