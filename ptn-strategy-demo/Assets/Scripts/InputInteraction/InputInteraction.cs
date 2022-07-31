using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInteraction : Singleton<InputInteraction>
{
    private Camera _mainCamera;
    private RaycastHit2D[] _raycastResults = new RaycastHit2D[1];

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
        Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _raycastResults);

        if (!Equals(_raycastResults[0].collider, null))
        {
            if (_raycastResults[0].collider.TryGetComponent(out ISelectable _ISelectable))
            {
                _ISelectable.OnClickAction();
                //break;
            }
            else
            {
                Actions.OnUnitDeselected?.Invoke();
                Actions.OnUnitSelected?.Invoke(null);
            }
        }


        // if (hits2D.collider != null)
        // {   
        //     if (hits2D.collider.TryGetComponent(out ISelectable _ISelectable))
        //     {
        //         _ISelectable.OnClickAction();
        //     }
        //     else
        //     {
        //         Actions.OnUnitDeselected?.Invoke();
        //         Actions.OnUnitSelected?.Invoke(null);
        //     }
        // }
    }

    public Vector3 GetMouseToWorldPosition()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePosition();
        mousePosition.z = _mainCamera.nearClipPlane;
        return _mainCamera.ScreenToWorldPoint(mousePosition);
    }
}