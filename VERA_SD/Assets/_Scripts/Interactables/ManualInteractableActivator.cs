using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualInteractableActivator : MonoBehaviour
{

    // ManualInteractableActivator allows manual activation of various interactions on an interactable


    #region VARIABLES


    public VERA_Interactable interactableToTrigger;
    XRIDefaultInputActions inputActions;


    #endregion


    #region ACTIVATE INPUT


    // OnEnable, activate input
    //--------------------------------------//
    private void OnEnable()
    //--------------------------------------//
    {
        inputActions = new XRIDefaultInputActions();
        inputActions.Keyboard.Enable();
        inputActions.Keyboard.NumberKeys.performed += OnNumberKey;

    } // END OnEnable


    // OnDisable, deactivate input
    //--------------------------------------//
    private void OnDisable()
    //--------------------------------------//
    {
        inputActions.Keyboard.NumberKeys.performed -= OnNumberKey;

    } // END OnDisable


    #endregion


    #region FUNCTIONS


    // OnNumberKey, activate interaction with given index in interactable
    //--------------------------------------//
    public void OnNumberKey(InputAction.CallbackContext value)
    //--------------------------------------//
    {
        // Parse number from keyboard key
        int numKeyValue;
        int.TryParse(value.control.name, out numKeyValue);

        // If 0, display all interactions
        if (numKeyValue == 0)
        {
            List<string> interactions = interactableToTrigger.GetInteractions();
            if (interactions.Count > 0)
            {
                string int_print = "Interactions: 1 = " + interactions[0];

                for (int i = 1; i < interactions.Count; i++)
                {
                    int_print += ", " + (i + 1) + " = ";
                    int_print += interactions[i];
                }

                Debug.Log(int_print);
            }
            else
            {
                Debug.Log("No interactions to display");
            }
        }
        // If 1-9, trigger interaction by index
        else
        {
            interactableToTrigger.TriggerInteractionByIndex(numKeyValue - 1);
        }

    } // END OnNumberKey


    #endregion


} // END ManualInteractableActivator.cs
