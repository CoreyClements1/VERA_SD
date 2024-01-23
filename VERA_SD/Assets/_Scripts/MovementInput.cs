using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementInput : MonoBehaviour
{
    // Start is called before the first frame update
    InputActions _input = null;
    public float turnLPress = 0;
    public float turnRPress = 0;
    public float forwardPress = 0;

    float rotationValue = 0;
    private void OnEnable(){
        _input = new InputActions();
        _input.Movement.Enable();

        _input.Movement.TurnLeft.performed += SetLookLeft;
        // _input.Movement.TurnLeft.canceled += SetLookLeft;

        _input.Movement.Forward.performed += SetForward;
        // _input.Movement.Forward.canceled += SetForward;

        _input.Movement.TurnRight.performed += SetLookRight;
        // _input.Movement.TurnRight.canceled += SetLookRight;
    }

    private void OnDisable(){
        _input.Movement.TurnLeft.performed -= SetLookLeft;
        // _input.Movement.TurnLeft.canceled -= SetLookLeft;

        _input.Movement.Forward.performed -= SetForward;
        // _input.Movement.Forward.canceled -= SetForward;

        _input.Movement.TurnRight.performed -= SetLookRight;
        // _input.Movement.TurnRight.canceled -= SetLookRight;

        _input.Movement.Disable();
    }

    // These functions set the value to 1 to signify that it was pressed
    private void SetLookLeft(InputAction.CallbackContext ctx){
        turnLPress = ctx.ReadValue<float>();
        // Debug.Log(ctx.ReadValue<float>());
    }

    private void SetLookRight(InputAction.CallbackContext ctx){
        turnRPress = ctx.ReadValue<float>();
    }

    private void SetForward(InputAction.CallbackContext ctx){
        forwardPress = ctx.ReadValue<float>();
    }
}
