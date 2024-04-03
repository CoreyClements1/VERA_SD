using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class NewMenuNavigation : MonoBehaviour
{

    // NewMenuNavigation handles navigation of the new UI menu


    #region VARIABLES


    public enum UiState
    {
        Header,
        Look,
        Move,
        Interact,
        InteractSub,
        Settings
    }

    private UiState pendingStateAfterHeader = UiState.Look;
    public UiState currentUiState { get; private set; } = UiState.Header;
    private int currentButtonHighlight = 0;

    private InputActions inputActions;
    private FolderTabsManager folderTabsManager;
    private MainAreaManager mainAreaManager;

    [SerializeField] private ButtonTabberManager lookTabberManager;
    [SerializeField] private ButtonTabberManager moveTabberManager;
    [SerializeField] private ButtonTabberManager interactTabberManager;
    [SerializeField] private ButtonTabberManager interactSubTabberManager;
    [SerializeField] private ButtonTabberManager settingsTabberManager;

    private UiInputDistributor uiInputDistributor;
    private SelectionController selectionController;
    private InteractionsMenuManager interactionsMenuManager;


    #endregion


    #region SETUP


    // Start
    //--------------------------------------//
    void Start()
    //--------------------------------------//
    {
        folderTabsManager = FindObjectOfType<FolderTabsManager>();
        folderTabsManager.BeginTabbing();
        folderTabsManager.InitialStart();
        mainAreaManager = FindObjectOfType<MainAreaManager>();
        mainAreaManager.SwapMainAreaToMode(UiState.Look);

        inputActions = new InputActions();
        inputActions.Movement.Enable();

        inputActions.Movement.Switch1.performed += Button1;
        inputActions.Movement.Switch2.performed += Button2;
        inputActions.Movement.Switch3.performed += Button3;
        inputActions.Movement.Switch4.performed += Button4;

        uiInputDistributor = FindObjectOfType<UiInputDistributor>();
        selectionController = FindObjectOfType<SelectionController>();
        interactionsMenuManager = FindObjectOfType<InteractionsMenuManager>();

    } // END Start


    #endregion


    #region BUTTONS


    // Button 1 (left)
    //--------------------------------------//
    private void Button1(InputAction.CallbackContext ctx)
    //--------------------------------------//
    {
        switch(currentUiState)
        {
            // If on header, tab one folder to the left
            case UiState.Header:
                switch (folderTabsManager.TabLeft())
                {
                    case 0: 
                        pendingStateAfterHeader = UiState.Look;
                        break;
                    case 1: 
                        pendingStateAfterHeader = UiState.Move; 
                        break;
                    case 2: 
                        pendingStateAfterHeader = UiState.Interact; 
                        break;
                    case 3: 
                        pendingStateAfterHeader = UiState.Settings; 
                        break;
                }
                mainAreaManager.SwapMainAreaToMode(pendingStateAfterHeader);
                break;

            case UiState.Look:
                currentButtonHighlight = lookTabberManager.TabLeft();
                break;

            case UiState.Move:
                currentButtonHighlight = moveTabberManager.TabLeft();
                break;

            case UiState.Interact:
                currentButtonHighlight = interactTabberManager.TabLeft();
                break;

            case UiState.InteractSub:
                currentButtonHighlight = interactSubTabberManager.TabLeft();
                break;

            case UiState.Settings:
                currentButtonHighlight = settingsTabberManager.TabLeft();
                break;
        }

    } // END Button1


    // Button 2 (right)
    //--------------------------------------//
    private void Button2(InputAction.CallbackContext ctx)
    //--------------------------------------//
    {
        switch (currentUiState)
        {
            // If on header, tab one folder to the right
            case UiState.Header:
                switch (folderTabsManager.TabRight())
                {
                    case 0:
                        pendingStateAfterHeader = UiState.Look;
                        break;
                    case 1:
                        pendingStateAfterHeader = UiState.Move;
                        break;
                    case 2:
                        pendingStateAfterHeader = UiState.Interact;
                        break;
                    case 3:
                        pendingStateAfterHeader = UiState.Settings;
                        break;
                }
                mainAreaManager.SwapMainAreaToMode(pendingStateAfterHeader);
                break;

            case UiState.Look:
                currentButtonHighlight = lookTabberManager.TabRight();
                break;

            case UiState.Move:
                currentButtonHighlight = moveTabberManager.TabRight();
                break;

            case UiState.Interact:
                currentButtonHighlight = interactTabberManager.TabRight();
                break;

            case UiState.InteractSub:
                currentButtonHighlight = interactSubTabberManager.TabRight();
                break;

            case UiState.Settings:
                currentButtonHighlight = settingsTabberManager.TabRight();
                break;
        }

    } // END Button2


    // Button 3 (select)
    //--------------------------------------//
    private void Button3(InputAction.CallbackContext ctx)
    //--------------------------------------//
    {
        switch (currentUiState)
        {
            // If on header, begin controlling main area
            case UiState.Header:
                folderTabsManager.EndTabbing();
                switch(pendingStateAfterHeader)
                {
                    case UiState.Look:
                        lookTabberManager.ResetTabbing();
                        break;
                    case UiState.Move:
                        moveTabberManager.ResetTabbing();
                        break;
                    case UiState.Interact:
                        interactTabberManager.ResetTabbing();
                        break;
                    case UiState.Settings:
                        settingsTabberManager.ResetTabbing();
                        break;
                }
                currentUiState = pendingStateAfterHeader;
                currentButtonHighlight = 0;
                break;

            // Activate corresponding look function
            case UiState.Look:
                switch (currentButtonHighlight)
                {
                    case 0:
                        uiInputDistributor.LookUp();
                        break;
                    case 1:
                        uiInputDistributor.LookDown();
                        break;
                    case 2:
                        uiInputDistributor.LookReset();
                        break;
                }
                break;

            // Activate corresponding move function
            case UiState.Move:
                switch (currentButtonHighlight)
                {
                    case 0:
                        uiInputDistributor.MoveForward();
                        break;
                    case 1:
                        uiInputDistributor.TurnLeft();
                        break;
                    case 2:
                        uiInputDistributor.TurnRight();
                        break;
                }
                break;

            // Activate corresponding interact function
            case UiState.Interact:
                switch (currentButtonHighlight)
                {
                    case 0:
                        uiInputDistributor.HighlightAll();
                        interactionsMenuManager.HighlightedAll();
                        break;
                    case 1:
                        uiInputDistributor.SelectNext();
                        interactionsMenuManager.SelectedNext();
                        break;
                    case 2:
                        if (interactionsMenuManager.CanViewInteractions())
                        {
                            currentButtonHighlight = 0;
                            interactTabberManager.EndTabbing();
                            interactionsMenuManager.SetupInteractableSub(interactSubTabberManager);
                            interactSubTabberManager.BeginTabbing();
                            currentUiState = UiState.InteractSub;
                        }
                        break;
                }
                break;

            case UiState.InteractSub:
                interactionsMenuManager.TriggerSubInteraction(interactSubTabberManager, currentButtonHighlight);
                break;

            case UiState.Settings:
                // TODO
                break;
        }

    } // END Button3


    // Button 4 (back)
    //--------------------------------------//
    private void Button4(InputAction.CallbackContext ctx)
    //--------------------------------------//
    {
        switch (currentUiState)
        {
            case UiState.Header:
                // At head of UI system, no "back" available
                break;

            case UiState.Look:
                lookTabberManager.EndTabbing();
                folderTabsManager.BeginTabbing();
                currentUiState = UiState.Header;
                break;

            case UiState.Move:
                moveTabberManager.EndTabbing();
                folderTabsManager.BeginTabbing();
                currentUiState = UiState.Header;
                break;

            case UiState.Interact:
                interactTabberManager.EndTabbing();
                folderTabsManager.BeginTabbing();
                currentUiState = UiState.Header;
                break;

            case UiState.InteractSub:
                interactSubTabberManager.EndTabbing();
                interactionsMenuManager.DestroyInteractableSub(interactSubTabberManager);
                interactTabberManager.ResetTabbing();
                currentButtonHighlight = 0;
                interactTabberManager.BeginTabbing();
                currentUiState = UiState.Interact;
                break;

            case UiState.Settings:
                // TODO
                lookTabberManager.EndTabbing();
                folderTabsManager.BeginTabbing();
                currentUiState = UiState.Header;
                break;
        }

    } // END Button4


    #endregion


    #region OTHER


    // Called when interact is out of range
    //--------------------------------------//
    public void InteractOutOfRange()
    //--------------------------------------//
    {
        if (currentUiState == UiState.InteractSub)
        {
            interactSubTabberManager.EndTabbing();
            interactionsMenuManager.DestroyInteractableSub(interactSubTabberManager);
            interactionsMenuManager.ResetText();
            interactTabberManager.BeginTabbing();
            currentUiState = UiState.Interact;
        }
        else
        {
            interactionsMenuManager.ResetText();
        }

    } // END InteractOutOfRange


    #endregion


} // END NewMenuNavigation.cs
