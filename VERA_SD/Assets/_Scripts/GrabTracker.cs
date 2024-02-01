using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTracker : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject grabbedObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(grabbedObject!=null){
            UpdateGrabbedObjectPosition();
        }
    }

    public void SetGrabbedObject(GameObject obj){
        grabbedObject = obj;
        // Debug.LogError(grabbedObject);
    }

    public GameObject GetGrabbedObject(){
        return grabbedObject;
    }
    
    void UpdateGrabbedObjectPosition(){
        // Update the position of the grabbed object to stay at the bottom right of the player' camera
        Vector3 offset = new Vector3(0.75f, 0.25f, 0.25f); // Adjust the offset as needed
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(offset);
        //use camera rotation
        Vector3 cameraPosition = Camera.main.transform.position;
        Quaternion cameraRotation = Camera.main.transform.rotation;
        if(grabbedObject.GetComponent<XRGrabInteractable>().attachTransform!=null){
            
        }
        else{
            grabbedObject.transform.rotation = cameraRotation;
            grabbedObject.transform.position = targetPosition;
        }

                
    }
}
