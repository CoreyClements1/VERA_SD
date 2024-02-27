using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchKey : MonoBehaviour
{
    [SerializeField] GrabTracker tracker;
    [SerializeField] GameObject door;
    public void Open()
    {
        if (tracker.grabbedObject != null && tracker.grabbedObject.name == "Torch Key")
        {
            Debug.Log("SUCCESS!");
            door.SetActive(false);
        } else
        {
            Debug.Log("NO KEY!!!!! GET OUT!!!");
        }

    }

}
