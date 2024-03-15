using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{


    #region VARIABLES


    [SerializeField] private Key linkedKey;
    [SerializeField] private DefaultActions leftDoor;
    [SerializeField] private DefaultActions rightDoor;
    private GrabTracker grabHandler;
    private bool locked = true;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        grabHandler = FindObjectOfType<GrabTracker>();

    } // END Start


    #endregion


    #region OPEN


    // Tries to open/close the locked door, depending on if key is held / unlocked
    //--------------------------------------//
    public void TryOpenCloseLockDoor()
    //--------------------------------------//
    {
        if (!locked)
        {
            leftDoor.OpenAndClose();
            rightDoor.OpenAndClose();
        }
        else
        {
            if (grabHandler.grabbedObject != null)
            {
                if (grabHandler.grabbedObject == linkedKey.gameObject)
                {
                    linkedKey.Unlock();
                    locked = false;
                }
            }
        }

    } // END TryOpenCloseLockDoor


    #endregion


} // END LockedDoor.cs
