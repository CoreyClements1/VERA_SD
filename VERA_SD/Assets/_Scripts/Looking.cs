using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject camera;
    public float speed;
    public float smooth;
    public float minAngle = 90f;
    public float maxAngle = 90f;
    private Transform mainCam;

    void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void LateUpdate()
    {


    }

    public void LookUp()
    {
        if (Camera.main.transform.parent.parent.rotation.eulerAngles.x % 360 <= 90 || Camera.main.transform.parent.parent.rotation.eulerAngles.x % 360 > 270)
        {
            Vector3 oldPos = mainCam.parent.position;
            Camera.main.transform.parent.parent.Rotate(-speed, 0f, 0f, Space.Self);
            mainCam.parent.position = oldPos;
        }

    }

    public void LookDown()
    {
        if (Camera.main.transform.parent.parent.rotation.eulerAngles.x % 360 < 90 || Camera.main.transform.parent.parent.rotation.eulerAngles.x % 360 >= 270)
        {
            Vector3 oldPos = mainCam.parent.position;
            Camera.main.transform.parent.parent.Rotate(speed, 0f, 0f, Space.Self);
            mainCam.parent.position = oldPos;
        }
    }

    public void ResetAngle()
    {
        Vector3 oldPos = mainCam.parent.position;
        Camera.main.transform.parent.parent.rotation = Quaternion.Euler(0, mainCam.parent.rotation.eulerAngles.y, mainCam.parent.rotation.eulerAngles.z);
        mainCam.parent.position = oldPos;
    }
}
