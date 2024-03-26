using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInputDistributor : MonoBehaviour
{

    // UiInputDistributor funnels the button inputs from the UI to their respective functions


    #region VARIABLES


    private Looking looking;
    private MovementInput movementInput;


    #endregion


    #region INIT


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        Init();

    } // END Start


    // Initializes the components to distribute input to
    //--------------------------------------//
    private void Init()
    //--------------------------------------//
    {
        looking = FindObjectOfType<Looking>();
        movementInput = FindObjectOfType<MovementInput>();

    } // END Init


    #endregion


    #region DISTRIBUTIONS: LOOKING


    // LookUp
    //--------------------------------------//
    public void LookUp()
    //--------------------------------------//
    {
        looking.LookUp();

    } // END LookUp


    // LookDown
    //--------------------------------------//
    public void LookDown()
    //--------------------------------------//
    {
        looking.LookDown();

    } // END LookDown


    // LookReset
    //--------------------------------------//
    public void LookReset()
    //--------------------------------------//
    {
        looking.ResetAngle();

    } // END LookReset


    #endregion


    #region DISTRIBUTIONS: MOVEMENT


    // MoveForward
    //--------------------------------------//
    public void MoveForward()
    //--------------------------------------//
    {
        movementInput.TreeMoveForward();

    } // END MoveForward


    // TurnLeft
    //--------------------------------------//
    public void TurnLeft()
    //--------------------------------------//
    {
        movementInput.TreeTurnLeft();

    } // END TurnLeft


    // TurnRight
    //--------------------------------------//
    public void TurnRight()
    //--------------------------------------//
    {
        movementInput.TreeTurnRight();

    } // END TurnRight


    #endregion


} // END UiInputDistributor