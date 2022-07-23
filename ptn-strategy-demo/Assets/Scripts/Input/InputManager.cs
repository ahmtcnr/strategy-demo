using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputActions _inputActions;
    private Camera _mainCamera;

    protected override void Awake()
    {
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
    }

    private void StartedClick(InputAction.CallbackContext obj)
    {
        Debug.Log("StartedClick");
    }

    private void EndedClick(InputAction.CallbackContext obj)
    {
        Debug.Log("EndedClick");
        DetectObject();
    }


    private void DetectObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_inputActions.Mouse.Position.ReadValue<Vector2>());
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        if (hits2D.collider != null && hits2D.collider.TryGetComponent(out ISelectable _ISelectable))
        {
            _ISelectable.OnClickAction();
        }
    }
}