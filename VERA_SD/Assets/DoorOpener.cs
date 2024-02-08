using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private HingeJoint joint;
    private bool isOpen = false;
    void Start()
    {
        joint = GetComponent<HingeJoint>();
    }

    private void Open()
    {
        if (!isOpen)
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.max);
            isOpen = true;
        }
    }

    private void Close()
    {
        if (isOpen)
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, -joint.limits.max);
            isOpen = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Open();
            Debug.Log("opening door");
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            Close();
            Debug.Log("closing door");
        }
    }
}
