using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject camera;
    public float speed;
    public float smooth;
    void Start()
    {
        
    }
    public void LookUp(){
        // if( camera.transform.eulerAngles.x > -180 && camera.transform.eulerAngles.x < 180){
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x-speed, transform.rotation.y, transform.rotation.z),  smooth);
        // }
    }
    public void LookDown(){
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x+speed, transform.rotation.y, transform.rotation.z),smooth);

    }
    public void ResetAngle(){
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
