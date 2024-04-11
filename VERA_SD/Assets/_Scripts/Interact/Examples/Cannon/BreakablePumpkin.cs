using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePumpkin : MonoBehaviour
{

    #region VARIABLES


    [SerializeField] private Rigidbody[] breakableRbs;
    private bool broken = false;


    #endregion


    #region BREAK


    // Breaks the pumpkin
    //--------------------------------------//
    public void BreakPumpkin()
    //--------------------------------------//
    {
        if (!broken)
        {
            broken = true;

            foreach (Rigidbody rb in breakableRbs)
            {
                rb.isKinematic = false;
                rb.gameObject.GetComponent<Collider>().enabled = true;
            }
        }

    } // END BreakPumpkin


    // OnTriggerEnter, break the pumpkin
    //--------------------------------------//
    private void OnCollisionEnter(Collision collision)
    //--------------------------------------//
    {
        if (collision.gameObject.CompareTag("Projectile"))
            BreakPumpkin();        

    }


    #endregion


} // END BreakablePumpkin.cs
