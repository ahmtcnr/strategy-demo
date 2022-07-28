using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInteraction : Singleton<InputInteraction>
{
    
    
    private Camera _mainCamera;

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
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null && hits2D.collider.TryGetComponent(out ISelectable _ISelectable))
        {
            _ISelectable.OnClickAction();
        }
        else
        {
            Actions.OnDeselectUnit?.Invoke();
        }
    }
    
    public Vector3 GetMouseToWorldPosition()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePosition();
        mousePosition.z = _mainCamera.nearClipPlane;
        return _mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
