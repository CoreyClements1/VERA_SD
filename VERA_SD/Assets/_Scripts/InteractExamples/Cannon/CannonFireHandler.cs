using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFireHandler : MonoBehaviour
{

    // CannonFireHandler handles the fire button being pressed


    #region VARIABLES


    [SerializeField] private CannonInteractable cannonInteractable;


    #endregion


    #region TRIGGER


    // OnTriggerEnter, fire a cannonball
    //--------------------------------------//
    public void OnSelect()
    //--------------------------------------//
    {
        cannonInteractable.FireCannonball();

    } // END OnTriggerEnter


    #endregion


} // END CannonFireHandler.cs
