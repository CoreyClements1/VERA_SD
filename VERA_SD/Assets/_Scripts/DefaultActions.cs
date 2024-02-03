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
        // Debug.Log("localPosition"+transform.localPosition);
        // Debug.Log("GlobalPosition"+transform.position);
    } // END Start

    private void Update()
    {
        
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
        grabHandler.SetGrabParent(transform.parent);
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
        Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f); // Adjust the offset as needed
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);

        if (grabInteractable != null && grabInteractable.attachTransform != null)
        {
            Quaternion grabRotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            
            grabInteractable.attachTransform.SetParent(Camera.main.transform, true);
            transform.SetParent(grabInteractable.attachTransform, true);
            
            grabInteractable.attachTransform.position = targetPosition;
            grabInteractable.attachTransform.rotation =  grabRotation;

            
        }
        else
        {
            Debug.LogWarning("XR Grab Interactable component or Attach Transform not found. Using default rotation.");
            Quaternion grabRotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            transform.SetParent(Camera.main.transform, true);
            // If XR Grab Interactable or Attach Transform is not available, use camera rotation
            // obj.transform.localPosition = targetPosition;
            // obj.transform.localRotation = grabRotation;
            obj.transform.position = targetPosition;
            obj.transform.rotation = grabRotation;
            
        }
        // Snap the grabbed object to the bottom right of the player's camera
        // }
    }

    // ReleaseObject method
    void ReleaseObject(){
        isGrabbing = false;
        GameObject obj = grabHandler.GetGrabbedObject();
        obj.transform.SetParent(grabHandler.GetGrabParent(),true);
        // Enable gravity during release
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if(grabInteractable != null && grabInteractable.attachTransform != null){
            // Debug.Log(grabInteractable.attachTransform);
            grabInteractable.attachTransform.SetParent(obj.transform, true);
        }
        
        // Place the grabbed object in front of the player with smooth interpolation
        // Vector3 center = new Vector3(0.5f, 0.5f, 0.0f);
        Vector3 zero = Camera.main.ViewportToWorldPoint(Vector3.zero);
        // obj.transform.localRotation = Quaternion.identity;
        Vector3 offset = new Vector3(0.5f, 0.5f, 3.0f); // Adjust the offset as needed// adjust for distance before drop
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);
        // Check for collisions along the path
        if (CheckCollisions(zero, targetPosition))
        {
            // If there is a collision, move to the point just before the collision
            Vector3 adjustedPosition = FindAdjustedPosition(zero, targetPosition, obj);
            obj.transform.position = adjustedPosition;
        }
        else{
            // If no collision, move to the target position
            Debug.Log("Entered");
            obj.transform.position = targetPosition;
        }
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
            rb.isKinematic = false;
        }
            grabHandler.SetGrabbedObject(null);
        }

    bool CheckCollisions(Vector3 start, Vector3 end)
    {
        // Check for collisions using Physics.Linecast
        // This function returns true if there is a collision along the line
        return Physics.Linecast(start, end);
    }

    Vector3 FindAdjustedPosition(Vector3 start, Vector3 end, GameObject obj)
    {
        RaycastHit hit;
        Vector3 adjustedPosition = end;

        // Perform a Raycast to find the first collision along the path
        // obj.SetActive(false);
        int ogLayer = obj.layer;
        // Debug.Log("ogLayer; "+ogLayer);
        obj.layer = LayerMask.NameToLayer("Ignore Raycast");
        // Debug.Log("NamerToLayer; "+LayerMask.NameToLayer("Ignore Raycast"));
        int layerMask = ~LayerMask.GetMask("Ignore Raycast");

        if (Physics.Raycast(start, (end - start), out hit, Vector3.Distance(start, end),layerMask))
        {
            // Move to the point just before the collision
            adjustedPosition = hit.point - (end - start).normalized * 0.1f; // Adjust the offset as needed
            
            // Debug.Log("RayCase"+Physics.Raycast(start, (end - start).normalized, out hit, Vector3.Distance(start, end)));
            // Debug.Log("hitpoint: "+hit.collider);
            // Debug.Log("end:"+end+" start:"+start+" end-start:"+(end - start)+" normalized:"+(end - start).normalized+" distance:"+Vector3.Distance(start, end));
            // Debug.Log("adjustedPosition"+adjustedPosition);
        }

        obj.layer = ogLayer;
        
        // obj.SetActive(true);

        return adjustedPosition;
    }
    #endregion


} // END ExampleInteractions.cs
