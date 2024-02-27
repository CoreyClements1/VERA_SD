using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTracker : MonoBehaviour
{

    #region VARIABLES


    public GameObject grabbedObject;
    private Transform objParent;
    private bool useGravity;
    private bool freezeRotation;
    private bool isKinematic;
    private RigidbodyInterpolation rigidbodyInterp;


    #endregion


    #region MONOBEHAVIOUR

    // START
    //--------------------------------------//
    void Start()
    //--------------------------------------//
    {
    } //End Start

    // UPDATE
    //--------------------------------------//
    void Update()
    //--------------------------------------//
    {
    }//End Update


    #endregion


    #region GET/SET FUNCTIONS


    #region RIGIDBODY


    // SetRB // Store RigidBody settings //
    //--------------------------------------//
    public void SetRB(Rigidbody rb)
    //--------------------------------------//
    {
        useGravity = rb.useGravity;
        freezeRotation = rb.freezeRotation;
        isKinematic = rb.isKinematic;
        rigidbodyInterp = rb.interpolation;
    }//End SetRB

    // Get Gravity Setting
    //--------------------------------------//
    public bool GetGravity()
    //--------------------------------------//
    {
        return useGravity;
    }//End GetGravity

    // Get Freeze Rotation Setting
    //--------------------------------------//
    public bool GetfreezeRotation()
    //--------------------------------------//
    {
        return freezeRotation;
    }//End GetfreezeRotation

    // Get Kinematic Setting
    //--------------------------------------//
    public bool GetKinematic()
    //--------------------------------------//
    {
        return isKinematic;
    }//End GetKinematic

    // Get Interpolation Setting
    //--------------------------------------//
    public RigidbodyInterpolation GetInterpolation()
    //--------------------------------------//
    {
        return rigidbodyInterp;
    }//End GetInterpolation

    #endregion


    #region PARENT


    // SetGrabParent // Store Grabbed Object's Parent //
    //--------------------------------------//
    public void SetGrabParent(Transform obj)
    //--------------------------------------//
    {
        objParent = obj;
    }//End SetGrabParent

    // Get Grabbed Object Parent
    //--------------------------------------//
    public Transform GetGrabParent()
    //--------------------------------------//
    {
        return objParent;
    }//End GetGrabParent

    #endregion


    #region GRABBED OBJECT

    // SetGrabbedObject // Store Grabbed Object //
    //--------------------------------------//
    public void SetGrabbedObject(GameObject obj)
    //--------------------------------------//
    {
        grabbedObject = obj;
    }//End SetGrabbedObject

    // Get Grabbed Object
    //--------------------------------------//
    public GameObject GetGrabbedObject()
    //--------------------------------------//
    {
        return grabbedObject;
    }//End GetGrabbedObject


    #endregion


    #endregion
}
