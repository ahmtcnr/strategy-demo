using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    private InputActions _inputActions;


    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Mouse.LeftClick.started += StartedLeftClick;
        _inputActions.Mouse.RightClick.started += StartRightClick;

        _inputActions.KeyBoard.TestKey.started += SpaceButtonDown;
    }


    private void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Mouse.LeftClick.started -= StartedLeftClick;
        _inputActions.Mouse.RightClick.started -= StartRightClick;
        
        _inputActions.KeyBoard.TestKey.started -= SpaceButtonDown;
    }

    private void SpaceButtonDown(InputAction.CallbackContext obj)
    {
        Actions.OnSpaceBarDown?.Invoke();
    }

    private void StartRightClick(InputAction.CallbackContext obj)
    {
        Actions.OnRightClick?.Invoke();
    }

    private void StartedLeftClick(InputAction.CallbackContext obj)
    {
        Actions.OnLeftClick?.Invoke();
    }

    public Vector2 GetMousePosition() => _inputActions.Mouse.Position.ReadValue<Vector2>();


    public Vector2 DeltaPosition()
    {
        return _inputActions.Mouse.DeltaPosition.ReadValue<Vector2>();
    }
    
    
}