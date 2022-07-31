using System;
using UnityEngine;

namespace Building
{
    public class BuildPlacementHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask unitLayer;
        private bool _isBuildingState;
        private BaseBuilding _currentBuilding;

        private void OnEnable()
        {
            Actions.OnBuildingClick += SetBuildStatus;
            Actions.OnLeftClickGround += CheckConditions;
            Actions.OnDeselectBuilding += Deselect;
        }

        private void OnDisable()
        {
            Actions.OnBuildingClick -= SetBuildStatus;
            Actions.OnLeftClickGround -= CheckConditions;
            Actions.OnDeselectBuilding -= Deselect;
        }


        private void SetBuildStatus(BaseBuilding baseBuilding)
        {
            _currentBuilding = baseBuilding;
            _isBuildingState = true;
        }

        private void Deselect()
        {
            _currentBuilding = null;
            _isBuildingState = false;
        }

        private void CheckConditions()
        {
            if (!_isBuildingState)
                return;


            Collider2D hitColliders = Physics2D.OverlapBox(
                (Vector2)GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition + _currentBuilding.baseUnitData.UnitSize / 2,
                _currentBuilding.baseUnitData.UnitSize / 2, 0,
                unitLayer);
            if (hitColliders == null && GridSystem.Instance.IsNodesEmpty(_currentBuilding.baseUnitData.UnitSize))
            {
                Actions.OnBuildSuccess?.Invoke(_currentBuilding);
                Actions.OnDeselectBuilding?.Invoke();
            }
        }
    }
}