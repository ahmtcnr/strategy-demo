using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private InputActions _inputActions;
    private Camera _mainCamera;

    
    
    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputActions();

        //TODO: Expensive Cast
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Mouse.Click.started += StartedClick;
        _inputActions.Mouse.Click.performed += EndedClick;
    }


    private void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Mouse.Click.started -= StartedClick;
        _inputActions.Mouse.Click.performed -= EndedClick;
    }

    private void StartedClick(InputAction.CallbackContext obj)
    {
       Actions.OnLeftClick?.Invoke();
    }

    private void EndedClick(InputAction.CallbackContext obj)
    {
        DetectObject();
    }

    //TODO: Selection manager
    private void DetectObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(GetMousePosition());
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null && hits2D.collider.transform.parent.TryGetComponent(out ISelectable _ISelectable))//TODO: Find another way for .parent
        {
            _ISelectable.OnClickAction();
        }
    }


    private Vector2 GetMousePosition() => _inputActions.Mouse.Position.ReadValue<Vector2>();

    public Vector3 GetMouseToWorldPosition()
    {
        Vector3 mousePosition = GetMousePosition();
        mousePosition.z = _mainCamera.nearClipPlane;
        return _mainCamera.ScreenToWorldPoint(mousePosition);
    }
}