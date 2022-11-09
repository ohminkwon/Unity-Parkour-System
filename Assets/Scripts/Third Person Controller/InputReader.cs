using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;

    public Vector2 MovementValue { get; private set; }
    public Vector2 MousePos { get; private set; }
    public float MouseScrollY { get; private set; }

    public bool IsJumping { get; private set; }

    //public event Action OnJumpEvent;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //if (!context.performed)
        //    return;
        //OnJumpEvent?.Invoke();

        if (context.performed)
            IsJumping = true;
        else if (context.canceled)
            IsJumping = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MousePos = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        MouseScrollY = context.ReadValue<float>();
    }
}
