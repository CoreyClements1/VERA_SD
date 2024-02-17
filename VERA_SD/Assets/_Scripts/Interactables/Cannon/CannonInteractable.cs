using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonInteractable : MonoBehaviour
{

    // CannonInteractable is an example interactable which controls the cannon


    #region VARIABLES


    [SerializeField] private Transform cannonBase;
    [SerializeField] private Transform cannonBody;
    [SerializeField] private Transform cannonballSpawnpoint;
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private float cannonFireSpeed = 10f;


    #endregion


    #region LEVER ROTATION


    // Called when player rotating cannon, matches rotation with hand
    //--------------------------------------//
    public void OnRotatingCannon(Transform lookTransform)
    //--------------------------------------//
    {
        // Rotate base
        Vector3 baseTarget = new Vector3(lookTransform.position.x, cannonBase.position.y, lookTransform.position.z);
        cannonBase.LookAt((cannonBase.position - baseTarget) + cannonBase.position);

        // Rotate body
        Vector3 bodyTarget = lookTransform.position;
        //cannonBody.LookAt((cannonBody.position - baseTarget) + cannonBody.position);
        cannonBody.LookAt(bodyTarget);
        //Vector3 lookRot = cannonBody.Quaternion.LookRotation(bodyTarget).eulerAngles;
        Vector3 lookRot = cannonBody.rotation.eulerAngles;
        lookRot.x = -lookRot.x;
        lookRot.y = lookRot.y + 180f;
        cannonBody.rotation = Quaternion.Euler(lookRot);
        //cannonBody.LookAt(cannonBody.position - bodyTarget);

    } // END OnRotatingCannon


    #endregion


    #region MANUAL AIM


    // Aims left
    //--------------------------------------//
    public void AimLeft()
    //--------------------------------------//
    {
        Vector3 lookRot = cannonBase.rotation.eulerAngles;
        lookRot.y += 2f;
        cannonBase.rotation = Quaternion.Euler(lookRot);
        cannonBody.rotation = Quaternion.Euler(cannonBody.rotation.eulerAngles.x, lookRot.y, cannonBody.rotation.eulerAngles.z);

    } // END AimLeft


    // Aims right
    //--------------------------------------//
    public void AimRight()
    //--------------------------------------//
    {
        Vector3 lookRot = cannonBase.rotation.eulerAngles;
        lookRot.y -= 2f;
        cannonBase.rotation = Quaternion.Euler(lookRot);
        cannonBody.rotation = Quaternion.Euler(cannonBody.rotation.eulerAngles.x, lookRot.y, cannonBody.rotation.eulerAngles.z);

    } // END AimRight


    // Aims up
    //--------------------------------------//
    public void AimUp()
    //--------------------------------------//
    {
        Vector3 lookRot = cannonBody.rotation.eulerAngles;
        lookRot.x += 2f;
        cannonBody.rotation = Quaternion.Euler(lookRot);

    } // END AimUp


    // Aims down
    //--------------------------------------//
    public void AimDown()
    //--------------------------------------//
    {
        Vector3 lookRot = cannonBody.rotation.eulerAngles;
        lookRot.x -= 2f;
        cannonBody.rotation = Quaternion.Euler(lookRot);

    } // END AimUp


    #endregion


    #region CANNONBALL FIRING


    // Fires cannonball
    //--------------------------------------//
    public void FireCannonball()
    //--------------------------------------//
    {
        GameObject cannonball = GameObject.Instantiate(cannonballPrefab);
        cannonball.transform.position = cannonballSpawnpoint.position;

        cannonball.GetComponent<Rigidbody>().AddForce(cannonBody.forward * cannonFireSpeed, ForceMode.Impulse);

    } // END FireCannonball


    #endregion


} // END CannonInteractable.cs
