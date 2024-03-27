using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    [SerializeField][Range(0, 90)] private float upAngle = 90;
    [SerializeField][Range(0, 90)] private float downAngle = 90;
    private float adjustedSpeed;
    private Transform mainCam;
    void Start()
    {
        mainCam = Camera.main.transform.parent;
        adjustedSpeed = speed;
        upAngle = 360 - upAngle;
    }
    //if max or min is set greater than 90 then i believe it will break
    public void LookUp()
    {
        Debug.Log(mainCam.rotation.eulerAngles.x);
        float currentAngle;
        if (Mathf.Floor(mainCam.rotation.eulerAngles.x) == 0)
        {
            currentAngle = 360;
        }
        else
        {
            currentAngle = mainCam.rotation.eulerAngles.x;
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
            Debug.Log(mainCam.rotation.eulerAngles.x);
        }
    }
    public void LookDown()
    {
        //Comments below are for logic of the throwing rotation stuff
        //CURRENT>-90
        if ((mainCam.rotation.eulerAngles.x % 360) < downAngle || (mainCam.rotation.eulerAngles.x % 360) >= upAngle)
        {
            if (mainCam.rotation.eulerAngles.x == upAngle)//IF CURRENT == 90;
            {
                transform.Rotate(adjustedSpeed, 0f, 0f, Space.Self);//ROTATE DOWN FROM ADJUSTED WITH -RADIANS
            }
            else//IF CURRENT DOES NOT == 90
            {
                if ((mainCam.rotation.eulerAngles.x % 360 + speed) > downAngle && (mainCam.rotation.eulerAngles.x % 360 + speed) < upAngle)//IF CURRENT-20 <-90
                {
                    adjustedSpeed = downAngle - (mainCam.rotation.eulerAngles.x);//90+CURRENT(NEGATIVE);
                    //ANGLE = -90;
                }
                else//IF CURRENT-20!<-90
                {
                    adjustedSpeed = speed;//CURRENT = THROWANGLECHANGE
                    //ANGLE-=THROWCHANGE;
                }
                transform.Rotate(adjustedSpeed, 0f, 0f, Space.Self);//ROTATE REGULAR
            }
        }
    }
    public void ResetAngle()
    {
        mainCam.rotation = Quaternion.Euler(0, 0, 0);
    }
}
