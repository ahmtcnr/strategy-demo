using System;
using UnityEngine;

namespace Building
{
    public class BuildPlacementHandler : MonoBehaviour
    {
        private bool isBuildingState;
        private BaseBuilding _currentBuilding;
        [SerializeField] private LayerMask unitLayer;

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
            isBuildingState = true;
        }

        private void Deselect()
        {
            _currentBuilding = null;
            isBuildingState = false;
        }

        private void CheckConditions()
        {
            if (!isBuildingState)
                return;


            Collider2D hitColliders =
                Physics2D.OverlapBox((Vector2)GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition + _currentBuilding.baseUnitData.UnitSize / 2,
                    _currentBuilding.baseUnitData.UnitSize / 2, 0,
                    unitLayer);
            if (hitColliders == null)
            {
                Actions.OnBuildSuccess?.Invoke(_currentBuilding);
                Actions.OnDeselectBuilding?.Invoke();
            }
        }
    }
}