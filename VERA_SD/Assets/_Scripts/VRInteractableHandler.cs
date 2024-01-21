using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractableHandler : MonoBehaviour
{
    // public string interactableTag = "Interactable";

    public KeyCode interactionKey = KeyCode.Alpha1; // Change to KeyCode.Alpha1 for the "1" key

    private bool isGrabbing = false;
    private GameObject grabbedObject;

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (isGrabbing)
            {
                ReleaseObject();
            }
            else
            {
                TryInteract();
            }
        }

        if (isGrabbing)
        {
            UpdateGrabbedObjectPosition();
        }
    }
    void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Holdable"))
            {
                // Check the type of interaction and perform the corresponding action
                if (hit.collider.GetComponent<XRGrabInteractable>() != null)
                {
                    // It' a grabbable object, grab it
                    GrabObject(hit.collider.gameObject);
                }
                else
                {
                    // It' a different type of interactable object, implement interaction logic here
                    Debug.Log("Make it Grabable");
                }
            }
        }
    }

    void GrabObject(GameObject obj)
{
    isGrabbing = true;
    grabbedObject = obj;

    // Disable gravity during grab
    Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.useGravity = false;
        rb.freezeRotation = true;
    }
    // Get the rotation and position of the Attach Transform of XRGrabInteraction
        // XRGrabInteractable grabInteractable = grabbedObject.GetComponent<XRGrabInteractable>();
        // if (grabInteractable != null && grabInteractable.attachTransform != null)
        // {
        //     // Make the grabbed object a child of the attachTransform
        //     grabbedObject.transform.parent = grabInteractable.attachTransform;

        //     // Reset local position and rotation to maintain the object's world position
        //     grabbedObject.transform.localPosition = Vector3.zero;
        //     grabbedObject.transform.localRotation = Quaternion.identity;
        // }
        // else
        // {

            // If XRGrabInteraction or attachTransform is not present, use camera rotation
            Quaternion cameraRotation = Camera.main.transform.rotation;

            // Apply the camera rotation to the grabbed object
            grabbedObject.transform.rotation = cameraRotation;

            // Snap the grabbed object to the bottom right of the player's camera
            Vector3 offset = new Vector3(0.75f, 0f, 0.25f); // Adjust the offset as needed
            grabbedObject.transform.position = Camera.main.ViewportToWorldPoint(offset);
        // }
}

// ReleaseObject method
void ReleaseObject()
{
    isGrabbing = false;

    // Enable gravity during release
    Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.useGravity = true;
        rb.freezeRotation = false;
    }
    // //Attach transform remove parent
    // if (grabbedObject.transform.parent != null && grabbedObject.transform.parent.GetComponent<XRGrabInteractable>() != null)
    // {
    //     grabbedObject.transform.parent = null;
    // }
    // Place the grabbed object in front of the player with smooth interpolation
    Vector3 frontOffset = new Vector3(0f, 0f, 1.0f); // Adjust the offset as needed// adjust for distance before drop
    Vector3 targetPosition = transform.position + transform.forward * frontOffset.z;

    StartCoroutine(MoveObjectSmoothly(grabbedObject.transform, targetPosition, 0.75f)); // Adjust the duration as needed + makes it slower - makes it faster

    grabbedObject = null;
}

IEnumerator MoveObjectSmoothly(Transform objectTransform, Vector3 targetPosition, float duration)
{
    float elapsedTime = 0f;
    Vector3 initialPosition = objectTransform.position;

    while (elapsedTime < duration)
    {
        objectTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    objectTransform.position = targetPosition; // Ensure it reaches the exact target position
}

    void UpdateGrabbedObjectPosition()
    {
        // Update the position of the grabbed object to stay at the bottom right of the player' camera
        Vector3 offset = new Vector3(0.75f, 0f, 0.25f); // Adjust the offset as needed
        // grabbedObject.transform.LookAt(Camera.main.transform);
        // If XRGrabInteraction or attachTransform is not present, use camera rotation
        Quaternion cameraRotation = Camera.main.transform.rotation;
        // Apply the camera rotation to the grabbed object
        grabbedObject.transform.rotation = cameraRotation;
        grabbedObject.transform.position = Camera.main.ViewportToWorldPoint(offset);
    }
}
