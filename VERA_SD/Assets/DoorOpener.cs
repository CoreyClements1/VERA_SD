using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO
// ADD CHECK FOR WHEN LIMITS MAX OR MIN ARE 0
public class DoorOpener : MonoBehaviour
{
    private HingeJoint joint;
    private bool isOpen = false;
    void Start()
    {
        joint = GetComponent<HingeJoint>();
    }

    public void Open()
    {
        if (!isOpen && joint.limits.max != 0)
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.max);
            isOpen = true;
        } else
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, joint.limits.min);
            isOpen = true;
        }
    }

    public void Close()
    {
        if (isOpen && joint.limits.max != 0)
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, -joint.limits.max);
            isOpen = false;
        } else
        {
            this.transform.RotateAround(transform.TransformPoint(joint.anchor), joint.axis, -joint.limits.min);
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
