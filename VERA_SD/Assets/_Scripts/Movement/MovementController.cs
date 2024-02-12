
using JetBrains.Rider.Unity.Editor;
using Unity.XR.CoreUtils;
using UnityEditor.Rendering;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementController : MonoBehaviour
{


    #region VARIABLES


    [SerializeField] Transform Rig;
    
    Vector3 _userMoveInput = Vector3.zero;
    Vector3 _userLookInput = Vector3.zero;

    [Header("Movement")]
    [SerializeField] MovementInput _input;

    [SerializeField] public float speed = 1f;
    [SerializeField] public float rotationValue = 15f;


    #endregion


    #region FIXED UPDATE


    private void FixedUpdate(){
        // Debug.Log(movementActions.Movement.Forward.ReadValue<float>());
        bool forwardDown = _input.forwardPress > 0.1f;
        bool turnLDown = _input.turnLPress > 0.1f;
        bool rightRDown =  _input.turnRPress > 0.1f;

        if(forwardDown){
            Debug.Log(_input.forwardPress + " Forward");
            _userMoveInput = GetMoveInput();
            UserMove();
            _input.forwardPress = 0;
        }

        if(turnLDown){
            Debug.Log(_input.turnLPress + " Turn L");
            _userLookInput = GetTurnLInput();
            UserLook();
            _input.turnLPress = 0;
        }

        if(rightRDown){
            Debug.Log(_input.turnRPress + " Turn R");
            _userLookInput = GetTurnRInput();
            UserLook();
            _input.turnRPress = 0;
        }
        
    }


    #endregion


    #region MOVEMENT FUNCTIONS


    private Vector3 GetMoveInput(){
        return Rig.transform.forward * speed;
    }

    private Vector3 GetTurnLInput(){
        return new Vector3 (0, -rotationValue, 0);
    }
    private Vector3 GetTurnRInput(){
        return new Vector3 (0, rotationValue, 0);
    }
    private void UserMove(){
        // Rig.transform.forward
        _userMoveInput = new Vector3(_userMoveInput.x, _userMoveInput.y, _userMoveInput.z);
        Rig.position = Rig.position + _userMoveInput;
    }

    private void UserLook(){
        Rig.transform.eulerAngles = Rig.transform.eulerAngles + _userLookInput;
    }


    #endregion


}

