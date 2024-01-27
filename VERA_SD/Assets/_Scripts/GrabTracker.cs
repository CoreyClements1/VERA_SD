using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 offset = new Vector3(0.75f, 0f, 0.25f); // Adjust the offset as needed

        //use camera rotation
        Quaternion cameraRotation = Camera.main.transform.rotation;
        // Apply the camera rotation to the grabbed object
        grabbedObject.transform.rotation = cameraRotation;
        grabbedObject.transform.position = Camera.main.ViewportToWorldPoint(offset);
    }
}