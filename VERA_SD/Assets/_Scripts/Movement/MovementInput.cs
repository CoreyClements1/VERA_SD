using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementInput : MonoBehaviour
{


    #region VARIABLES


    InputActions _input = null;
    public float turnLPress = 0;
    public float turnRPress = 0;
    public float forwardPress = 0;
    public Vector2 stickInput = Vector2.zero;
    public float upPress = 0;
    public float downPress = 0;
    public float interactPress = 0;
    public float menuPress = 0;
    public float state = 0;
    // If true, movement will not be done via input actions, but called via the tree
    [SerializeField] private bool movementControlledByTree = false;
    // Variable to change the accessibility level to change controls for movement.
    [SerializeField] public int accessLevel = 1;

    #endregion


    #region ENABLE / DISABLE INPUT ACTIONS

    // Enabling actionmaps and subscribing to their events
    private void OnEnable()
    {
        // Level 1 of accessibility 
        if (accessLevel == 1)
        {
            _input = new InputActions();
            _input.Movement.Enable();

            _input.Movement.TurnLeft.performed += SetLookLeft;
            // _input.Movement.TurnLeft.canceled += SetLookLeft;

            _input.Movement.Forward.performed += SetForward;
            // _input.Movement.Forward.canceled += SetForward;

            _input.Movement.TurnRight.performed += SetLookRight;
            // _input.Movement.TurnRight.canceled += SetLookRight;
            // _input.Movement.Switch.performed -= SetStage2;
        }
        // Level 2 of accessibility 
        else if (accessLevel == 2)
        {
            _input = new InputActions();
            _input.MovementT2.Enable();

            _input.MovementT2.Movement.performed += setStickMovement;

            _input.MovementT2.LookUp.performed += setLookUp;

            _input.MovementT2.LookDown.performed += setLookDown;

            _input.MovementT2.Interact.performed += setInteract;

            _input.MovementT2.Menu.performed += setMenu;

            // _input.MovementT2.Switch.performed += setState;
        }

    }

    private void OnDisable(){
        // Level 1 of accessibility 
        if (accessLevel == 1 && !movementControlledByTree)
        {
            _input.Movement.TurnLeft.performed -= SetLookLeft;
            // _input.Movement.TurnLeft.canceled -= SetLookLeft;

            _input.Movement.Forward.performed -= SetForward;
            // _input.Movement.Forward.canceled -= SetForward;

            _input.Movement.TurnRight.performed -= SetLookRight;
            // _input.Movement.TurnRight.canceled -= SetLookRight;

            // _input.Movement.Switch.performed -= setState;

            _input.Movement.Disable();
        }
        // Level 2 of accessibility 
        else if (accessLevel == 2 && !movementControlledByTree)
        {
            _input.MovementT2.Movement.performed -= setStickMovement;

            _input.MovementT2.LookUp.performed -= setLookUp;

            _input.MovementT2.LookDown.performed -= setLookDown;

            _input.MovementT2.Interact.performed -= setInteract;

            _input.MovementT2.Menu.performed -= setMenu;

            // _input.MovementT2.Switch.performed -= setState;

            _input.MovementT2.Disable();
        }
        
    }


    #endregion

    #region ACCESS LEVEL 1
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

    // private void SetStage2(InputAction.CallbackContext ctx){
    //     state = ctx.ReadValue<float>();
    // }
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
    #endregion

    #region ACCESS LEVEL 2
    private void setStickMovement(InputAction.CallbackContext ctx)
    {
        stickInput = ctx.ReadValue<Vector2>();
    }
    private void setLookUp(InputAction.CallbackContext ctx)
    {
        upPress = ctx.ReadValue<float>();
    }

    private void setLookDown(InputAction.CallbackContext ctx)
    {
        downPress = ctx.ReadValue<float>();
    }

    private void setInteract(InputAction.CallbackContext ctx)
    {
        interactPress = ctx.ReadValue<float>();
    }

    private void setMenu(InputAction.CallbackContext ctx)
    {
        menuPress = ctx.ReadValue<float>();
    }

    private void setState(InputActionMap actionMap)
    {
        // state = ctx.ReadValue<float>();
        // if (accessLevel == 1){
        //     accessLevel = 2;
        // } else {
        //     accessLevel = 1;
        // }
        // if (actionMap.enabled)
        //     return;
    }
    #endregion

}
