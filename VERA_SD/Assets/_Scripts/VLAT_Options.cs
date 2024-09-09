using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLAT_Options : MonoBehaviour
{

    // VLAT_Options stores and distributes the various options for VLAT customization.


    #region VARIABLES


    [Header("XR Player")]
    [Tooltip("The XR player's parent game object (e.g., for the XR Interaction Toolkit, the XR Origin game object)")]
    [SerializeField] private GameObject xrPlayerParent;
    [Tooltip("The radius of the player's movement collider")]
    [SerializeField] private float xrPlayerRadius = 0.2f;
    [Tooltip("The height of the player's movement collider")]
    [SerializeField] private float xrPlayerHeight = 2.0f;

    [Header("Interaction")]
    [Tooltip("The maximum distance from which interactable objects may be interacted with")]
    [SerializeField] private float interactionRadius = 5f;


    #endregion


    #region SETUP


    // Start, distribute settings
    //--------------------------------------//
    void Start()
    //--------------------------------------//
    {
        SetupMovement();
        SetupInteraction();
        
    } // END Start


    // Sets up the movement / character controller based on options
    //--------------------------------------//
    private void SetupMovement()
    //--------------------------------------//
    {
        if (xrPlayerParent == null)
        {
            Debug.LogError("No XR player parent given for VLAT options. Please provide one for movement to work.");
            return;
        }
            
        MovementController moveControl = FindObjectOfType<MovementController>();

        if (moveControl != null)
            moveControl.SetupCharController(xrPlayerParent, xrPlayerRadius, xrPlayerHeight);
        else
            Debug.LogError("No VLAT MovementController could be found in scene; VLAT movement will not work.");

    } // END SetupMovement


    // Sets up interaction based on options
    //--------------------------------------//
    private void SetupInteraction()
    //--------------------------------------//
    {
        SelectionController selectControl = FindObjectOfType<SelectionController>();
    
    } // END SetupInteraction


    #endregion


} // END VLAT_Options.cs
