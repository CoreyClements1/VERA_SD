using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTracker : MonoBehaviour
{
    private GameObject grabbedObject;
    private Transform objParent;
    void Start()
    {

    }

    void Update()
    {
    }

    public void SetGrabParent(Transform obj)
    {
        objParent = obj;
    }

    public Transform GetGrabParent()
    {
        return objParent;
    }
    public void SetGrabbedObject(GameObject obj)
    {
        grabbedObject = obj;
    }

    public GameObject GetGrabbedObject()
    {
        return grabbedObject;
    }
}
