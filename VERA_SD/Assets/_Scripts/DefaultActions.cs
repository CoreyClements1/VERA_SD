using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class DefaultActions : MonoBehaviour
{

    // ExampleInteractions provides a simple sample interaction which can be triggered by interactables
    //DefaultActions uses ExampleInteractions as a base structure but uses More Relevent Default Functionality// Atleast that is the goal.
    #region VARIABLES
    

    [SerializeField] private GrabTracker grabHandler;
    private bool isGrabbing = false;
    private Renderer rend;
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 offsetFromAttachTransform;
    // private GameObject grabbedObject;

    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Awake()
    //--------------------------------------//
    {
        grabHandler = FindObjectOfType<GrabTracker>();
        if (grabHandler == null)
        {
            Debug.LogError("GrabTracker is not assigned or not found.");
        }
        else
        {
            Debug.Log("GrabTracker found.");
        }
        rend = GetComponent<Renderer>();
        Debug.Log("localPosition"+transform.localPosition);
        Debug.Log("GlobalPosition"+transform.position);
    } // END Start

    private void Update()
    {
        // grabHandler = GetComponent<GrabTracker>();
        // if (grabHandler == null)
        // {
        //     Debug.LogError("GrabTracker is not assigned or not found.");
        // }
        // else
        // {
        //     Debug.Log("GrabTracker found.");
        // }
    }

    #endregion


    #region SAMPLE INTERACTIONS
    //
    public void GrabOrRelease(){
        if(GetComponent<XRGrabInteractable>() != null){
            if (isGrabbing == false)
            {
                if (grabHandler.GetGrabbedObject() == null)
                {
                    GrabObject(gameObject);
                }
                else
                {
                    ReleaseObject();
                }
            }
            else
            {
                ReleaseObject();
            }
        }
        else{
            Debug.LogError("Not A Grab Interactable");
        }
    }
    //

    void GrabObject(GameObject obj){
        isGrabbing = true;
        grabHandler.SetGrabbedObject(obj);
        // grabbedObject = obj;

        // Disable gravity during grab
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null){
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.isKinematic = true;
        }
        // use camera rotation
        
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        //use camera rotationXRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        Quaternion cameraRotation = Camera.main.transform.rotation;
        Vector3 offset = new Vector3(0.75f, 0.25f, 0.25f); // Adjust the offset as needed
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);

        if (grabInteractable != null && grabInteractable.attachTransform != null)
        {
            originalParent = transform.parent;
            // originalLocalPosition = transform.localPosition;
            // originalLocalRotation = transform.localRotation;
            offsetFromAttachTransform = transform.position - grabInteractable.attachTransform.localPosition;
            transform.SetParent(Camera.main.transform, true);
            // transform.localPosition = offset + grabInteractable.attachTransform.InverseTransformPoint(grabInteractable.attachTransform.localPosition);
            transform.localRotation = Quaternion.Inverse(grabInteractable.attachTransform.localRotation);
            transform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning("XR Grab Interactable component or Attach Transform not found. Using default rotation.");

            // If XR Grab Interactable or Attach Transform is not available, use camera rotation
            obj.transform.rotation = cameraRotation;
            obj.transform.position = targetPosition;
        }
        // Snap the grabbed object to the bottom right of the player's camera
        // }
    }

    // ReleaseObject method
    void ReleaseObject(){
        isGrabbing = false;
        GameObject obj = grabHandler.GetGrabbedObject();
        // Enable gravity during release
        transform.parent = originalParent;
        // transform.localPosition = originalLocalPosition;
        // transform.localRotation = originalLocalRotation;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
            rb.isKinematic = false;
        }
        // Place the grabbed object in front of the player with smooth interpolation
        Vector3 offset = new Vector3(0.5f, 0.5f, 1.0f); // Adjust the offset as needed// adjust for distance before drop
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);

        StartCoroutine(MoveObjectSmoothly(obj.transform, targetPosition, 0.75f)); // Adjust the duration as needed + makes it slower - makes it faster
        
        grabHandler.SetGrabbedObject(null);
    }

    IEnumerator MoveObjectSmoothly(Transform objectTransform, Vector3 targetPosition, float duration){
        float elapsedTime = 0f;
        Vector3 initialPosition = objectTransform.position;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // objectTransform.position = targetPosition; // Ensure it reaches the exact target position
    }
    
    #endregion


} // END ExampleInteractions.cs
