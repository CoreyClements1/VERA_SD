using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SettingsManager : MonoBehaviour
{

    // SettingsManager manages the settings


    #region VARIABLES


    [Header("UI Elements")]
    [SerializeField] private Toggle joystickToggle;
    [SerializeField] private Toggle tapActivationToggle;
    [SerializeField] private Toggle disableVirtualHandsToggle;
    [SerializeField] private Slider moveSpeedSlider;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private Slider turnAmtSlider;
    [SerializeField] private TMP_Text turnAmtText;
    [SerializeField] private Slider menuTiltSlider;
    [SerializeField] private TMP_Text menuTiltText;
    [SerializeField] private Slider menuHeightSlider;
    [SerializeField] private TMP_Text menuHeightText;
    [SerializeField] private Slider menuOpacitySlider;
    [SerializeField] private TMP_Text menuOpacityText;

    private GameObject virtualHand1, virtualHand2;
    private MovementController movementController;
    private float defaultMoveSpeed;
    private NewMenuNavigation menuNav;
    private float defaultMenuRotX;
    private float defaultMenuPosY;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        XRController[] objs = GameObject.FindObjectsOfType<XRController>();
        //virtualHand1 = objs[0].gameObject;
        //virtualHand2 = objs[1].gameObject;

        movementController = FindObjectOfType<MovementController>();
        defaultMoveSpeed = movementController.speed;

        menuNav = FindObjectOfType<NewMenuNavigation>();
        defaultMenuRotX = menuNav.transform.localRotation.x;
        defaultMenuPosY = menuNav.transform.localPosition.y;

    } // END Start


    #endregion


    #region ON TOGGLE CHANGE


    // Called on change
    //--------------------------------------//
    public void OnJoystickToggleChange()
    //--------------------------------------//
    {
        // TODO

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnTapActivationToggleChange()
    //--------------------------------------//
    {
        // TODO

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnDisableVirtualHandsToggleChange()
    //--------------------------------------//
    {
        // TODO

        /*
        if(disableVirtualHandsToggle.isOn)
        {
            virtualHand1.SetActive(true);
            virtualHand2.SetActive(true);
        }
        else
        {
            virtualHand1.SetActive(false);
            virtualHand2.SetActive(false);
        }
        */

    } // END OnChange


    #endregion


    #region SLIDER ON CHANGE


    // Called on change
    //--------------------------------------//
    public void OnMoveSpeedSliderToggleChange()
    //--------------------------------------//
    {
        float multiplier = 1f;

        switch (moveSpeedSlider.value)
        {
            case 0: multiplier = .1f;
                break;
            case 1: multiplier = .35f;
                break;
            case 2: multiplier = .7f;
                break;
            case 3: multiplier = 1f;
                break;
            case 4: multiplier = 1.5f;
                break;
            case 5: multiplier = 2f;
                break;
            case 6: multiplier = 3f;
                break;
        }

        if (menuNav == null)
        {
            movementController = FindObjectOfType<MovementController>();
        }

        if (movementController != null)
        {
            movementController.speed = defaultMoveSpeed * multiplier;
            moveSpeedText.text = "" + Mathf.FloorToInt(multiplier * 100f) + "%";
        }

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnTurnAmtToggleChange()
    //--------------------------------------//
    {
        float turnAmt = 30f;

        switch (turnAmtSlider.value)
        {
            case 0:
                turnAmt = 5;
                break;
            case 1:
                turnAmt = 10f;
                break;
            case 2:
                turnAmt = 15f;
                break;
            case 3:
                turnAmt = 30f;
                break;
            case 4:
                turnAmt = 45f;
                break;
            case 5:
                turnAmt = 90f;
                break;
            case 6:
                turnAmt = 180f;
                break;
        }

        if (menuNav == null)
        {
            movementController = FindObjectOfType<MovementController>();
        }

        if (movementController != null)
        {
            movementController.rotationValue = turnAmt;
            turnAmtText.text = "" + Mathf.FloorToInt(turnAmt) + "°";
        }

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnMenuTiltToggleChange()
    //--------------------------------------//
    {
        float turnAmt = 30f;

        switch (menuTiltSlider.value)
        {
            case 0:
                turnAmt = 0f;
                break;
            case 1:
                turnAmt = 10f;
                break;
            case 2:
                turnAmt = 20f;
                break;
            case 3:
                turnAmt = 30f;
                break;
            case 4:
                turnAmt = 50f;
                break;
            case 5:
                turnAmt = 70f;
                break;
            case 6:
                turnAmt = 90f;
                break;
        }

        if (menuNav == null)
        {
            menuNav = FindObjectOfType<NewMenuNavigation>();
        }

        if (menuNav != null)
        {
            //menuNav.transform.localRotation = Quaternion.Euler(defaultMenuRotX + turnAmt, menuNav.transform.localRotation.eulerAngles.y, menuNav.transform.localRotation.eulerAngles.z);
            turnAmtText.text = "" + Mathf.FloorToInt(turnAmt) + "°";
        }
        

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnMenuHeightToggleChange()
    //--------------------------------------//
    {
        float turnAmt = 30f;

        switch (turnAmtSlider.value)
        {
            case 0:
                turnAmt = -.15f;
                break;
            case 1:
                turnAmt = -.1f;
                break;
            case 2:
                turnAmt = -.5f;
                break;
            case 3:
                turnAmt = 0f;
                break;
            case 4:
                turnAmt = .5f;
                break;
            case 5:
                turnAmt = 1f;
                break;
            case 6:
                turnAmt = 1.5f;
                break;
        }

        //menuNav.transform.localPosition = Vector3.zero;
        turnAmtText.text = "" + Mathf.FloorToInt(turnAmt) + "°";

    } // END OnChange


    // Called on change
    //--------------------------------------//
    public void OnMenuOpacityToggleChange()
    //--------------------------------------//
    {
        // TODO

    } // END OnChange


    #endregion


} // END SettingsManager.cs
