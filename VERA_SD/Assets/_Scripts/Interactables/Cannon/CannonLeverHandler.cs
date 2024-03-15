using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CannonLeverHandler : MonoBehaviour
{

    // CannonLeverHandler handles the lever of the cannon interactable


    #region VARIABLES


    [SerializeField] private CannonInteractable cannonInteractable;
    private bool grabActive = false;
    [SerializeField] private XRRayInteractor rayInteractorRight, rayInteractorLeft;
    private Transform targetTransform;


    #endregion


    #region TRIGGER STAY


    // OnTriggerStay, notify cannon
    //--------------------------------------//
    public void Update()
    //--------------------------------------//
    {
        if (grabActive)
            cannonInteractable.OnRotatingCannon(targetTransform);

    } // END OnTriggerStay


    // OnSelect
    //--------------------------------------//
    public void OnSelect()
    //--------------------------------------//
    {
        if (rayInteractorLeft.selectTarget != null && rayInteractorLeft.selectTarget.CompareTag("Button"))
        {
            targetTransform = rayInteractorLeft.transform;
            grabActive = true;
        }
        else if (rayInteractorRight.selectTarget != null && rayInteractorRight.selectTarget.CompareTag("Button"))
        {
            targetTransform = rayInteractorRight.transform;
            grabActive = true;
        }
        else
        {
            Debug.LogError("Something grabbed cannon which is not a hand   -(o.o)-   scary");
        }

    } // END OnSelect


    // OnEndSelect
    //--------------------------------------//
    public void OnEndSelect()
    //--------------------------------------//
    {
        grabActive = false;

    } // END OnEndSelect


    #endregion


} // END CannonLeverHandler.cs
