
using System.IO.Compression;
using JetBrains.Rider.Unity.Editor;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor.Rendering;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementController : MonoBehaviour
{


    #region VARIABLES

    CharacterController _characterController = null;
    [Header("XR Rig")]
    [Tooltip("Reference to the XR Rig's Transform")]
    [SerializeField] Transform Rig;
    [Tooltip("Reference to the Camera's Transform")]
    [SerializeField] Transform Camera;
    
    Vector3 _userMoveInput = Vector3.zero;
    Vector3 _userLookInput = Vector3.zero;

    [Header("Movement")]
    [Tooltip("Reference to the GameObject where the Input Action Manager and/or Inputs are defined")]
    [SerializeField] MovementInput _input;

    [SerializeField] public float speed = 1f;
    [SerializeField] public float rotationValue = 15f;
    [SerializeField] public float rotationValueVertical = 10f;
    // variables for analog turning (level 2)
    [SerializeField] public float turnCooldown = 1.0f;
    float lastPressTime = 0f;
    // determines which level of accessibity the user is on
    [Tooltip("Determines which level of accessibility the user is on")]
    [SerializeField] public int currentLvl = 1;
    #endregion

    #region START
    void Start()
    {
        if (!Rig)
            Debug.LogError("MovementController does not have reference for Rig <XR Origin Hands (XR Rig)>");
        if (!Camera)
            Debug.LogError("MovementController does not have reference for Camera");
        if (!_input)
            Debug.LogError("MovementController does not have reference for the Input Actions");
        
        _characterController = GetComponent<CharacterController>();
    }
    #endregion

    #region FIXED UPDATE


    private void FixedUpdate(){
        // These check if the switch has been pressed
        bool switchDown1 = _input.buttonPress1 > 0.1f;
        bool switchDown2 = _input.buttonPress2 > 0.1f;
        bool switchDown3 =  _input.buttonPress3 > 0.1f;
        bool switchDown4 = _input.buttonPress4 > 0.1f;
        bool statePressed = _input.state > 0.1f;

        // If the user is in the air then gravity will bring them down until grounded
        if(_characterController.isGrounded == false)
        {
            _userMoveInput += Physics.gravity;
            _characterController.Move(_userMoveInput * speed * Time.deltaTime);
        }

        // Level 1 Accessibity controls 
        if(currentLvl == 1){
            if(switchDown1){
                Debug.Log(_input.buttonPress1 + " Turn L");
                _userLookInput = GetTurnLInput();
                UserLook();
                _input.buttonPress1 = 0;
            }

            if(switchDown2){
                Debug.Log(_input.buttonPress2 + " Forward");
                _userMoveInput = GetMoveInput();
                UserMove();
                _input.buttonPress2 = 0;
            }

            if(switchDown3)
            {
                Debug.Log(_input.buttonPress3 + " Turn R");
                _userLookInput = GetTurnRInput();
                UserLook();
                _input.buttonPress3 = 0;
            }
            if(switchDown4)
            {
                Debug.Log("Back to tree");
                _input.buttonPress4 = 0;
            }
            if(statePressed)
            {
                currentLvl = 2;
                _input.state = 0;
                Debug.Log("Level: " + currentLvl);
            }
        } else if(currentLvl == 2){

            _userMoveInput = GetMoveInputAnalog();
            UserMoveAnalog();

            if(switchDown1){
                Debug.Log(_input.buttonPress1 + " Turn U");
                _userLookInput = GetLookUpInput();
                UserLookVertical();
                _input.buttonPress1 = 0;
            }

            if(switchDown2){
                Debug.Log(_input.buttonPress2 + " Turn D");
                _userLookInput = GetLookDownInput();
                UserLookVertical();
                _input.buttonPress2 = 0;
            }

            if(switchDown3)
            {
                Debug.Log("Interact change goes here");
                _input.buttonPress3 = 0;
            }
            if(switchDown4)
            {
                Debug.Log("Menu change goes here");
                _input.buttonPress4 = 0;
            }
            if(statePressed)
            {
                currentLvl = 1;
                Debug.Log("Lvl" + currentLvl);
                _input.state = 0;
            }
        }
    }


    #endregion


    #region MOVEMENT FUNCTIONS

    // Returns a value to be used to calculate the distance traveled for level 1 movement
    private Vector3 GetMoveInput(){
        return Rig.transform.forward * speed;
    }
    // Returns a value to be used to turn the rig a set amount of degrees
    private Vector3 GetTurnLInput(){
        return new Vector3 (0, -rotationValue, 0);
    }
    private Vector3 GetTurnRInput(){
        return new Vector3 (0, rotationValue, 0);
    }
    // Controls forward movement for level 1
    private void UserMove(){
        _userMoveInput = new Vector3(_userMoveInput.x, _userMoveInput.y, _userMoveInput.z);
        // Moves the rig
        _characterController.Move(_userMoveInput * speed * Time.deltaTime  * 1.5f);
    }
    // Rotates the rig based on if the rotation is left or right.
    private void UserLook(){
        Rig.transform.eulerAngles = Rig.transform.eulerAngles + _userLookInput;
    }   
    // References the camera instead of the rig itself since vertical adjustsments caused the rig to 
    //      rotate instead of just the camera.
    private void UserLookVertical()
    {
        Camera.transform.eulerAngles = Camera.transform.eulerAngles + _userLookInput;
    }

    // Translates user stick input into a vector value for movement
    private Vector3 GetMoveInputAnalog()
    {
        return new Vector3(_input.stickInput.x, 0.0f, _input.stickInput.y);
    }
    // Controls movement through the stick
    //      Direct forward and back on the stick does locomotion in the respective directions
    //      Left and right causes the rig to turn left and right which gets put on a cooldown
    private void UserMoveAnalog()
    {   
        // Checks input for the movement while limiting left and right movement
        bool movementCheck = (_userMoveInput.z > 0.9f && _userMoveInput.x < 0.2f && _userMoveInput.x > -0.2f) || 
                             (_userMoveInput.z < -0.9f && _userMoveInput.x < 0.2f && _userMoveInput.x > -0.2f);
        bool turnCheck = (_userMoveInput.x > 0.8f || _userMoveInput.x < -0.8f) && _userMoveInput.z < 0.9f && _userMoveInput.z > -0.9f;
        // Turning
        if(turnCheck)
        {   
            float currentTime = Time.time;
            float diffSecs = currentTime - lastPressTime;
            if(diffSecs >= turnCooldown)
            {
                lastPressTime = currentTime;
                _userLookInput = new Vector3(0, rotationValue * Mathf.Round(_userMoveInput.x),0);
                UserLook();
            }
            else
            {
                Debug.Log("Turning on cooldown");
            }
        }
        else if(movementCheck)
        {
            _userMoveInput = transform.right * _userMoveInput.x + transform.forward * _userMoveInput.z;
            // Continous forward movement
            if(_characterController.isGrounded == false)
            {
                _userMoveInput += Physics.gravity;
            }
            Debug.Log(_userMoveInput);
            _characterController.Move(_userMoveInput * speed * Time.deltaTime);
        } 
    }
    // Generates a rotation value to be used later
    private Vector3 GetLookUpInput()
    {
        return new Vector3 (-rotationValueVertical, 0, 0);
    }
    private Vector3 GetLookDownInput()
    {
        return new Vector3 (rotationValueVertical, 0, 0);
    }


    #endregion
}

