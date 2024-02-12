using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MovementControllerT2 : MonoBehaviour
{
#region VARIABLES

    Rigidbody _rigidbody = null;
    [SerializeField] Transform Rig;
    [SerializeField] Transform Camera;
    Vector3 _userMoveInput = Vector3.zero;
    Vector3 _userLookInput = Vector3.zero;

    [Header("Movement")]
    [SerializeField] MovementInput _input;

    [SerializeField] public float speed = 1f;
    [SerializeField] public float rotationValue = 15f;
    [SerializeField] public float rotationValueVertical = 10f;

    #endregion


    #region FIXED UPDATE

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate(){
        // Switch input checks
        bool lookUpPressed = _input.upPress > 0.1f;
        bool lookDownPressed = _input.downPress > 0.1f;
        bool interactPressed = _input.interactPress > 0.1f;
        bool menuPressed = _input.menuPress > 0.1f;
        bool statePressed = _input.state > 0.1f;

        // Movement
        _userMoveInput = GetMoveInput();
        UserMove();

        // Continous forward movement
        _rigidbody.AddRelativeForce(_userMoveInput, ForceMode.Force);

        if(_userMoveInput != Vector3.zero)
        {
            Debug.Log(_userMoveInput);
        }
        // Switch Inputs 
        if(lookUpPressed)
        {
            Debug.Log(_input.upPress + " Turn U");
            _userLookInput = GetLookUpInput();
            UserLook();
            _input.upPress = 0;
        }

        if(lookDownPressed)
        {
            Debug.Log(_input.downPress + " Turn D");
            _userLookInput = GetLookDownInput();
            UserLook();
            _input.downPress = 0;
        }

        if(interactPressed)
        {
            Debug.Log("Interact change goes here");
            _input.interactPress = 0;
        }

        if(menuPressed)
        {
            Debug.Log("Menu change goes here");
            _input.menuPress = 0;
        }

        if(statePressed)
        {
            Debug.Log("State changed to 1");
            // _input.state = 0;
            
        }
    }


    #endregion


    #region MOVEMENT FUNCTIONS
    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.stickInput.x, 0.0f, _input.stickInput.y);
    }
    private void UserMove()
    {   
        // Checks input for the movement while limiting left and right movement
        bool movementCheck = (_userMoveInput.z > 0.9f && _userMoveInput.x < 0.2f && _userMoveInput.x > -0.2f) || 
                             (_userMoveInput.z < -0.9f && _userMoveInput.x < 0.2f && _userMoveInput.x > -0.2f);
        
        if(movementCheck)
        {
            _userMoveInput = new Vector3(_userMoveInput.x,
                                        _userMoveInput.y,
                                        _userMoveInput.z * speed * _rigidbody.mass);
            Debug.Log("Moving");
        } else {
            _userMoveInput = new Vector3(0,
                                        0,
                                        0);
            Debug.Log("Not Moving");
        }
    }
    private Vector3 GetLookUpInput()
    {
        return new Vector3 (-rotationValueVertical, 0, 0);
    }
    private Vector3 GetLookDownInput()
    {
        return new Vector3 (rotationValueVertical, 0, 0);
    }
    // References the camera instead of the rig itself since vertical adjustsments caused the rig to 
    //      rotate instead of just the camera.
    private void UserLook()
    {
        Camera.transform.eulerAngles = Camera.transform.eulerAngles + _userLookInput;
    }
    #endregion


}
