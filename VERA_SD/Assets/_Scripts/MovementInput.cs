using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementInput : MonoBehaviour
{
    // Start is called before the first frame update
    InputActions _input = null;
    [SerializeField] float speed = 10f;
    [SerializeField] float rotateDegrees = 15f;
    public float turnPress = 0;
    public float ForwardPress = 0;

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

    private void SetLookLeft(InputAction.CallbackContext ctx){
        turnPress = rotateDegrees;
        Debug.Log(turnPress);
    }

    private void SetLookRight(InputAction.CallbackContext ctx){
        turnPress = -rotateDegrees;
        Debug.Log(turnPress);
    }

    private void SetForward(InputAction.CallbackContext ctx){
        ForwardPress = speed;
        Debug.Log(ForwardPress);
    }
}
