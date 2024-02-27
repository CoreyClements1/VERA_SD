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
    public float maxAngle = -90f;
    void Start()
    {
        
    }
    public void LookUp(){
        // if( camera.transform.eulerAngles.x > -180 && camera.transform.eulerAngles.x < 180){
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x-speed, transform.rotation.y, transform.rotation.z),  smooth);
        // }
        transform.Rotate(-speed, 0f, 0f, Space.Self);
    }
    public void LookDown(){
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x+speed, transform.rotation.y, transform.rotation.z),smooth);
        transform.Rotate(speed, 0f, 0f, Space.Self);
    }
    public void ResetAngle(){
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }
}
