using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class DefaultActions : MonoBehaviour
{

    // ExampleInteractions provides a simple sample interaction which can be triggered by interactables
    //DefaultActions uses ExampleInteractions as a base structure but uses More Relevent Default Functionality// Atleast that is the goal.
    #region VARIABLES

    private bool isGrabbing = false;
    private Renderer rend;
    private GameObject grabbedObject;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        rend = GetComponent<Renderer>();

    } // END Start

    private void Update()
    {
        if (isGrabbing)
        {
            UpdateGrabbedObjectPosition();
        }
    }

    #endregion


    #region SAMPLE INTERACTIONS
    //
    public void GrabOrRelease(){
        if(GetComponent<XRGrabInteractable>() != null){
            if(isGrabbing==false ){
                GrabObject(gameObject);
            }
            else{
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
        grabbedObject = obj;

        // Disable gravity during grab
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (rb != null){
            rb.useGravity = false;
            rb.freezeRotation = true;
        }
        // use camera rotation
        Quaternion cameraRotation = Camera.main.transform.rotation;
        // Apply the camera rotation to the grabbed object
        grabbedObject.transform.rotation = cameraRotation;
        // Snap the grabbed object to the bottom right of the player's camera
        Vector3 offset = new Vector3(0.75f, 0f, 0.25f); // Adjust the offset as needed
        grabbedObject.transform.position = Camera.main.ViewportToWorldPoint(offset);
        // }
    }

    // ReleaseObject method
    void ReleaseObject(){
        isGrabbing = false;

        // Enable gravity during release
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
        }
        // Place the grabbed object in front of the player with smooth interpolation
        Vector3 frontOffset = new Vector3(0f, 0f, 1.0f); // Adjust the offset as needed// adjust for distance before drop
        Vector3 targetPosition = transform.position + transform.forward * frontOffset.z;

        StartCoroutine(MoveObjectSmoothly(grabbedObject.transform, targetPosition, 0.75f)); // Adjust the duration as needed + makes it slower - makes it faster

        grabbedObject = null;
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

    void UpdateGrabbedObjectPosition(){
        // Update the position of the grabbed object to stay at the bottom right of the player' camera
        Vector3 offset = new Vector3(0.75f, 0f, 0.25f); // Adjust the offset as needed

        //use camera rotation
        Quaternion cameraRotation = Camera.main.transform.rotation;
        // Apply the camera rotation to the grabbed object
        grabbedObject.transform.rotation = cameraRotation;
        grabbedObject.transform.position = Camera.main.ViewportToWorldPoint(offset);
    }

    // Sets the color of the interactable's mesh
    //--------------------------------------//
    public void ChangeColor(string color)
    //--------------------------------------//
    {
        switch(color.ToLower())
        {
            case "r":
            case "red":
                rend.material.SetColor("_Color", Color.red);
                break;
            case "g":
            case "green":
                rend.material.SetColor("_Color", Color.green);
                break;
            case "b":
            case "blue":
                rend.material.SetColor("_Color", Color.blue);
                break;
            case "y":
            case "yellow":
                rend.material.SetColor("_Color", Color.yellow);
                break;
            case "p":
            case "purple":
                rend.material.SetColor("_Color", Color.magenta);
                break;
            default:
                rend.material.SetColor("_Color", Color.black);
                break;
        }

    } // END SetColor


    // Destroys the interactable
    //--------------------------------------//
    public void DestroyInteractable()
    //--------------------------------------//
    {
        Destroy(gameObject);

    } // END DestroyInteractable


    // Grows the interactable
    //--------------------------------------//
    public void Grow()
    //--------------------------------------//
    {
        transform.localScale = transform.localScale * 1.5f;

    } // END Grow


    // Shrinks the interactable
    //--------------------------------------//
    public void Shrink()
    //--------------------------------------//
    {
        transform.localScale = transform.localScale * .666f;

    } // END Shrink

    #endregion


} // END ExampleInteractions.cs
