using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTracker : MonoBehaviour
{
    private GameObject grabbedObject;
    private Transform objParent;
    private bool useGravity;
    private bool freezeRotation;
    private bool isKinematic;
    private RigidbodyInterpolation rigidbodyInterp;
    void Start()
    {

    }

    void Update()
    {
    }
    //===================================
    public void SetRB(Rigidbody rb)
    {
        useGravity = rb.useGravity;
        freezeRotation = rb.freezeRotation;
        isKinematic = rb.isKinematic;
        rigidbodyInterp = rb.interpolation;
    }
    public bool GetGravity()
    {
        return useGravity;
    }
    public bool GetfreezeRotation()
    {
        return freezeRotation;
    }
    public bool GetKinematic()
    {
        return isKinematic;
    }
    public RigidbodyInterpolation GetInterpolation()
    {
        return rigidbodyInterp;
    }
    //===================================

    public void SetGrabParent(Transform obj)
    {
        objParent = obj;
    }

    public Transform GetGrabParent()
    {
        return objParent;
    }

    //===================================
    public void SetGrabbedObject(GameObject obj)
    {
        grabbedObject = obj;
    }

    public GameObject GetGrabbedObject()
    {
        return grabbedObject;
    }
}
