using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLeverHandler : MonoBehaviour
{

    // CannonLeverHandler handles the lever of the cannon interactable


    #region VARIABLES


    [SerializeField] private CannonInteractable cannonInteractable;


    #endregion


    #region TRIGGER STAY


    // OnTriggerStay, notify cannon
    //--------------------------------------//
    public void OnTriggerStay(Collider other)
    //--------------------------------------//
    {
        if (other.CompareTag("Player"))
            cannonInteractable.OnRotatingCannon(other.transform);

    } // END OnTriggerStay


    #endregion


} // END CannonLeverHandler.cs
