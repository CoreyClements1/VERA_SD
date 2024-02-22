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
    public float buttonPress1 = 0;
    public float buttonPress2 = 0;
    public float buttonPress3 = 0;
    public float buttonPress4 = 0;
    public Vector2 stickInput = Vector2.zero;
    public float state = 0;
    // If true, movement will not be done via input actions, but called via the tree
    [SerializeField] private bool movementControlledByTree = false;
    // Variable to change the accessibility level to change controls for movement.
    // [SerializeField] public int accessLevel = 1;

    #endregion


    #region ENABLE / DISABLE INPUT ACTIONS

    // Enabling actionmaps and subscribing to their events
    private void OnEnable()
    {
        // Level 1 of accessibility 
        // if (accessLevel == 1)
        // {
        _input = new InputActions();
        _input.Movement.Enable();

        _input.Movement.StickInput.performed += SetStickMovement;

        _input.Movement.Switch1.performed += SetPressed1;

        _input.Movement.Switch2.performed += SetPressed2;

        _input.Movement.Switch3.performed += SetPressed3;
        
        _input.Movement.Switch4.performed += SetPressed4;

        _input.Movement.SwitchLevel.performed += SetState;
        // }
        // // Level 2 of accessibility 
        // else if (accessLevel == 2)
        // {
        //     _input = new InputActions();
        //     _input.MovementT2.Enable();

        //     _input.MovementT2.Movement.performed += SetStickMovement;

        //     _input.MovementT2.LookUp.performed += SetLookUp;

        //     _input.MovementT2.LookDown.performed += SetLookDown;

        //     _input.MovementT2.Interact.performed += SetInteract;

        //     _input.MovementT2.Menu.performed += SetMenu;

        //     // _input.MovementT2.Switch.performed += SetState;
        // }

    }

    private void OnDisable(){
        // Level 1 of accessibility 
        // if (accessLevel == 1 && !movementControlledByTree)
        _input.Movement.StickInput.performed -= SetStickMovement;

        _input.Movement.Switch1.performed -= SetPressed1;

        _input.Movement.Switch2.performed -= SetPressed2;

        _input.Movement.Switch3.performed -= SetPressed3;

        _input.Movement.Switch4.performed -= SetPressed4;

        _input.Movement.SwitchLevel.performed -= SetState;

        _input.Movement.Disable();
        // // Level 2 of accessibility 
        // else if (accessLevel == 2 && !movementControlledByTree)
        // {
        //     _input.MovementT2.Movement.performed -= SetStickMovement;

        //     _input.MovementT2.LookUp.performed -= SetLookUp;

        //     _input.MovementT2.LookDown.performed -= SetLookDown;

        //     _input.MovementT2.Interact.performed -= SetInteract;

        //     _input.MovementT2.Menu.performed -= SetMenu;

        //     // _input.MovementT2.Switch.performed -= SetState;

        //     _input.MovementT2.Disable();
        // }
        
    }


    #endregion

    #region ACCESS LEVEL 1
    #region HANDLE PERFORMANCE OF INPUT


    // These functions set the value to 1 to signify that it was pressed
    private void SetPressed1(InputAction.CallbackContext ctx){
        if (!movementControlledByTree)
            buttonPress1 = ctx.ReadValue<float>();
    }

    private void SetPressed2(InputAction.CallbackContext ctx){
        if (!movementControlledByTree)
            buttonPress2 = ctx.ReadValue<float>();
    }

    private void SetPressed3(InputAction.CallbackContext ctx){
        if (!movementControlledByTree)
            buttonPress3 = ctx.ReadValue<float>();
    }

    private void SetPressed4(InputAction.CallbackContext ctx){
        if (!movementControlledByTree)
            buttonPress4 = ctx.ReadValue<float>();
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
        buttonPress1 = 1f;
    }

    public void TreeTurnRight()
    {
        buttonPress2 = 1f;
    }

    public void TreeMoveForward()
    {
        buttonPress3 = 1f;
    }
    #endregion
    #endregion

    // #region ACCESS LEVEL 2
    private void SetStickMovement(InputAction.CallbackContext ctx)
    {
        stickInput = ctx.ReadValue<Vector2>();
    }
    // private void SetLookUp(InputAction.CallbackContext ctx)
    // {
    //     upPress = ctx.ReadValue<float>();
    // }

    // private void SetLookDown(InputAction.CallbackContext ctx)
    // {
    //     downPress = ctx.ReadValue<float>();
    // }

    // private void SetInteract(InputAction.CallbackContext ctx)
    // {
    //     interactPress = ctx.ReadValue<float>();
    // }

    // private void SetMenu(InputAction.CallbackContext ctx)
    // {
    //     menuPress = ctx.ReadValue<float>();
    // }

    private void SetState(InputAction.CallbackContext ctx)
    {
        state = ctx.ReadValue<float>();
    }
    // #endregion

}
