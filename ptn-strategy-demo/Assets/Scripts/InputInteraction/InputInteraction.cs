using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInteraction : Singleton<InputInteraction>
{
    private Camera _mainCamera;
    private RaycastHit2D[] _raycastResults;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        Actions.OnLeftClick += DetectObject;
    }

    private void OnDisable()
    {
        Actions.OnLeftClick -= DetectObject;
    }


    private void DetectObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        _raycastResults = Physics2D.RaycastAll(ray.origin, ray.origin);


        foreach (var result in _raycastResults)
        {
            if (!Equals(result.collider, null))
            {
                if (result.collider.TryGetComponent(out ISelectable _ISelectable))
                {
                    _ISelectable.OnClickAction();

                    break;
                }
                else
                {
                    Actions.OnLeftClickGround?.Invoke();
                    Actions.OnUnitDeselected?.Invoke();
                }
            }
            else
            {
                Actions.OnUnitSelected?.Invoke(null);
                Actions.OnDeselectBuilding?.Invoke();
            }
        }
    }

    public Vector3 GetMouseToWorldPosition()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePosition();
        mousePosition.z = _mainCamera.nearClipPlane;
        return _mainCamera.ScreenToWorldPoint(mousePosition);
    }
}