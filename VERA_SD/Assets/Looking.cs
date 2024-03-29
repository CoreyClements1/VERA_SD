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

    public void LookUp()
    {
        if (Camera.main.transform.parent.rotation.eulerAngles.x % 360 <= 90 || Camera.main.transform.parent.rotation.eulerAngles.x % 360 > 270)
        {
            Camera.main.transform.parent.Rotate(-speed, 0f, 0f, Space.Self);
        }
    }

    public void LookDown()
    {
        if (Camera.main.transform.parent.rotation.eulerAngles.x % 360 < 90 || Camera.main.transform.parent.rotation.eulerAngles.x % 360 >= 270)
        {
            Camera.main.transform.parent.Rotate(speed, 0f, 0f, Space.Self);
        }
    }

    public void ResetAngle()
    {
        Camera.main.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
    }
}
