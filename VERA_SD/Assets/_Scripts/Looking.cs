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
    private float newVertChange;
    private float verticleAngle;
    void Start()
    {
        mainCam = Camera.main.transform.parent;
        adjustedSpeed = speed;
        upAngle = 360 - upAngle;
    }
    //if max or min is set greater than 90 then i believe it will break
    void Update()
    {
        float radians = (Camera.main.transform.localRotation.eulerAngles.y * Mathf.PI) / 180;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        Vector3 newVector = new Vector3(sin, 0f, cos);
        Debug.Log("vector: " + newVector);
        Debug.Log("cameraForward: " + Camera.main.transform.forward);
        Vector3 rightVect = Vector3.Cross(newVector, Vector3.up).normalized;
        Debug.Log("rightVect: " + rightVect);
        Debug.Log("cameraRight: " + Camera.main.transform.right);
        mainCam.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainCam.RotateAround(Camera.main.transform.position, rightVect, verticleAngle);
    }
    public void LookUp()
    {
        if (verticleAngle < 90)
        {
            if (verticleAngle == -90)
            {
                verticleAngle += newVertChange;
            }
            else
            {
                if (verticleAngle + speed > 90)
                {
                    newVertChange = 90 - verticleAngle;
                    verticleAngle = 90;
                }
                else
                {
                    verticleAngle += speed;
                    newVertChange = speed;
                }
            }
        }
        float radians = (Camera.main.transform.localRotation.eulerAngles.y * Mathf.PI) / 180;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        Vector3 newVector = new Vector3(sin, 0f, cos);
        Debug.Log("vector: " + newVector);
        Debug.Log("cameraForward: " + Camera.main.transform.forward);
        Vector3 rightVect = Vector3.Cross(newVector, Vector3.up).normalized;
        Debug.Log("rightVect: " + rightVect);
        Debug.Log("cameraRight: " + Camera.main.transform.right);
        mainCam.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainCam.RotateAround(mainCam.position, rightVect, verticleAngle);
    }
    public void LookDown()
    {
        //Comments below are for logic of the throwing rotation stuff
        //CURRENT>-90
        if (verticleAngle > -90)
        {
            if (verticleAngle == 90)
            {
                verticleAngle -= newVertChange;
            }
            else
            {
                if (verticleAngle - speed < -90)
                {
                    newVertChange = 90 + verticleAngle;
                    verticleAngle = -90;
                }
                else
                {
                    verticleAngle -= speed;
                    newVertChange = speed;
                }
            }

        }
        float radians = (Camera.main.transform.localRotation.eulerAngles.y * Mathf.PI) / 180;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        Vector3 newVector = new Vector3(sin, 0f, cos);
        Debug.Log("vector: " + newVector);
        Debug.Log("cameraForward: " + Camera.main.transform.forward);
        Vector3 rightVect = Vector3.Cross(newVector, Vector3.up).normalized;
        Debug.Log("rightVect: " + rightVect);
        Debug.Log("cameraRight: " + Camera.main.transform.right);
        mainCam.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainCam.RotateAround(mainCam.position, rightVect, verticleAngle);
    }
    public void ResetAngle()
    {
        verticleAngle = 0;
        newVertChange = speed;
        mainCam.rotation = Quaternion.Euler(0, 0, 0);
    }
}