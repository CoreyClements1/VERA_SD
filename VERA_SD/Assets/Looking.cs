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
    void Start()
    {

    }
    public void LookUp()
    {
        if (transform.rotation.eulerAngles.x % 360 <= 90 || transform.rotation.eulerAngles.x % 360 > 270)
        {
            transform.Rotate(-speed, 0f, 0f, Space.Self);
        }
    }
    public void LookDown()
    {
        if (transform.rotation.eulerAngles.x % 360 < 90 || transform.rotation.eulerAngles.x % 360 >= 270)
        {
            transform.Rotate(speed, 0f, 0f, Space.Self);
        }
    }
    public void ResetAngle()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
