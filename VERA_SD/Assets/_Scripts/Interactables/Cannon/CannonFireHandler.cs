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
    private void OnTriggerEnter(Collider other)
    //--------------------------------------//
    {
        if (other.CompareTag("Player"))
            cannonInteractable.FireCannonball();

    } // END OnTriggerEnter


    #endregion


} // END CannonFireHandler.cs
