using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float upAngle = 90;
    public float downAngle = 90;
    private float adjustedSpeed;
    void Start()
    {
        adjustedSpeed = speed;
        upAngle = 360 - upAngle;
        if (downAngle > 90)
        {
            downAngle = 90;
        }
        else if (downAngle < 0)
        {
            downAngle = 0;
        }
        if (upAngle < 270)
        {
            upAngle = 270;
        }
        else if (upAngle > 360)
        {
            upAngle = 360;
        }
    }
    //if max or min is set greater than 90 then i believe it will break
    public void LookUp()
    {
        Debug.Log(transform.rotation.eulerAngles.x);
        float currentAngle;
        if (Mathf.Floor(transform.rotation.eulerAngles.x) == 0)
        {
            currentAngle = 360;
        }
        else
        {
            currentAngle = transform.rotation.eulerAngles.x;
        }

        if (currentAngle % 360 <= downAngle || currentAngle % 360 > upAngle)
        {
            if (currentAngle == downAngle)
            {
                transform.Rotate(-adjustedSpeed, 0f, 0f, Space.Self);
            }
            else
            {
                if ((currentAngle - speed) % 360 > downAngle && (currentAngle - speed) % 360 < upAngle)
                {
                    adjustedSpeed = (currentAngle) - upAngle;
                }
                else
                {
                    adjustedSpeed = speed;
                }
                transform.Rotate(-adjustedSpeed, 0f, 0f, Space.Self);
            }
            Debug.Log(transform.rotation.eulerAngles.x);
        }
    }
    public void LookDown()
    {
        if ((transform.rotation.eulerAngles.x % 360) < downAngle || (transform.rotation.eulerAngles.x % 360) >= upAngle)
        {
            if (transform.rotation.eulerAngles.x == upAngle)
            {
                transform.Rotate(adjustedSpeed, 0f, 0f, Space.Self);
            }
            else
            {
                if ((transform.rotation.eulerAngles.x % 360 + speed) > downAngle && (transform.rotation.eulerAngles.x % 360 + speed) < upAngle)
                {
                    adjustedSpeed = downAngle - (transform.rotation.eulerAngles.x);
                }
                else
                {
                    adjustedSpeed = speed;
                }
                transform.Rotate(adjustedSpeed, 0f, 0f, Space.Self);
            }
        }
    }
    public void ResetAngle()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
