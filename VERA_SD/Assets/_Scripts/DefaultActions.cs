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
            
            Quaternion grabRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            grabInteractable.attachTransform.SetParent(Camera.main.transform, true);
            transform.SetParent(grabInteractable.attachTransform, true);
            
            grabInteractable.attachTransform.position = targetPosition;
            grabInteractable.attachTransform.rotation = grabRotation;

            
        }
        else
        {
            Debug.LogWarning("XR Grab Interactable component or Attach Transform not found. Using default rotation.");
            transform.SetParent(Camera.main.transform, true);
            // If XR Grab Interactable or Attach Transform is not available, use camera rotation
            obj.transform.position = targetPosition;
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
