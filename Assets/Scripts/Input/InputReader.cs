using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(menuName = "Input Reader", fileName = "New Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    private Action _onPointerClicked;
    private Action _onPointerClickedRelease;
    private Action<Vector2> _onPointerDrag;
    private GameInput _gameInput;
    private Vector2 _pointerPos;

    public Action OnPointerClicked { get => _onPointerClicked; set => _onPointerClicked = value; }
    public Action OnPointerClickedRelease { get => _onPointerClickedRelease; set => _onPointerClickedRelease = value; }
    public Action<Vector2> OnPointerDrag { get => _onPointerDrag; set => _onPointerDrag = value; }
    public Vector2 PointerPos { get => _pointerPos; set => _pointerPos = value; }

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
        }
        _gameInput.Gameplay.Enable();
        _gameInput.Gameplay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        if (_gameInput != null)
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Gameplay.RemoveCallbacks(this);
        }
    }


    public void OnPointerClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnPointerClicked?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnPointerClickedRelease?.Invoke();
        }
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        OnPointerDrag?.Invoke(context.ReadValue<Vector2>());
        PointerPos = context.ReadValue<Vector2>();
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnPointerClicked?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnPointerClickedRelease?.Invoke();
        }
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        OnPointerDrag?.Invoke(context.ReadValue<Vector2>());
        PointerPos = context.ReadValue<Vector2>();
    }
}
