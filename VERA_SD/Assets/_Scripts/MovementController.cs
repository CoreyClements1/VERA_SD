
using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Rigidbody _rigidbody = null;
    Camera vrCamera;

    [Header("Movement")]
    [SerializeField] MovementInput _input;


    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate(){

    }

    private float GetMoveInput(){
        return _input.turnPress;
    }
}
