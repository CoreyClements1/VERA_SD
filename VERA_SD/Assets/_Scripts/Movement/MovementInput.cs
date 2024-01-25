using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementInput : MonoBehaviour
{


    #region VARIABLES


    InputActions _input = null;
    public float turnLPress = 0;
    public float turnRPress = 0;
    public float forwardPress = 0;

    float rotationValue = 0;

    // If true, movement will not be done via input actions, but called via the tree
    [SerializeField] private bool movementControlledByTree = false;


    #endregion


    #region ENABLE / DISABLE INPUT ACTIONS


    private void OnEnable()
    {
        if (!movementControlledByTree)
        {
            _input = new InputActions();
            _input.Movement.Enable();

            _input.Movement.TurnLeft.performed += SetLookLeft;
            // _input.Movement.TurnLeft.canceled += SetLookLeft;

            _input.Movement.Forward.performed += SetForward;
            // _input.Movement.Forward.canceled += SetForward;

            _input.Movement.TurnRight.performed += SetLookRight;
            // _input.Movement.TurnRight.canceled += SetLookRight;
        }

    }

    private void OnDisable(){
        if (!movementControlledByTree)
        {
            _input.Movement.TurnLeft.performed -= SetLookLeft;
            // _input.Movement.TurnLeft.canceled -= SetLookLeft;

            _input.Movement.Forward.performed -= SetForward;
            // _input.Movement.Forward.canceled -= SetForward;

            _input.Movement.TurnRight.performed -= SetLookRight;
            // _input.Movement.TurnRight.canceled -= SetLookRight;

            _input.Movement.Disable();
        }
        
    }


    #endregion


    #region HANDLE PERFORMANCE OF INPUT


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


    #endregion


    #region MOVEMENT ACTIVATION FROM TREE


    // These functions manually activate input functions as if called from input,
    //     but is instead done so via a call from the main tree UI.

    public void TreeTurnLeft()
    {
        turnLPress = 1f;
    }

    public void TreeTurnRight()
    {
        turnRPress = 1f;
    }

    public void TreeMoveForward()
    {
        forwardPress = 1f;
    }


    #endregion


}
