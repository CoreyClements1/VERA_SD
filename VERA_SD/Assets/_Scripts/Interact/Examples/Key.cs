using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{

    // Key handles the key object and lock object


    #region VARIABLES


    [SerializeField] private Transform lockTransform;
    [SerializeField] private XRGrabInteractable doorGrabInteractable1, doorGrabInteractable2;
    [SerializeField] private Rigidbody doorRb1, doorRb2;
    [SerializeField] private HingeJoint doorHinge1, doorHinge2;
    [SerializeField] private ParticleSystem keyUnlockParticles;
    private bool canUnlock = true;
    private GrabTracker grabHandler;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        grabHandler = FindObjectOfType<GrabTracker>();
        LockDoors();

    } // END Start


    // Update
    //--------------------------------------//
    private void Update()
    //--------------------------------------//
    {
        if (canUnlock)
            TryUnlock();

    } // END Update


    #endregion


    #region LOCK / UNLOCK


    // Locks the doors by disabling joints
    //--------------------------------------//
    private void LockDoors()
    //--------------------------------------//
    {
        doorGrabInteractable1.enabled = false;
        doorGrabInteractable2.enabled = false;
        doorRb1.isKinematic = true;
        doorRb2.isKinematic = true;

    } // END LockDoors


    // Tries to unlock door if we are close enough
    //--------------------------------------//
    private void TryUnlock()
    //--------------------------------------//
    {
        if ((lockTransform.position - transform.position).magnitude < 0.5f)
        {
            if (grabHandler.grabbedObject == null)
            {
                Unlock();
                canUnlock = false;
            }
        }

    } // END TryUnlock


    // Unlocks door by allowing joint movement
    //--------------------------------------//
    public void Unlock()
    //--------------------------------------//
    {
        doorGrabInteractable1.enabled = true;
        doorGrabInteractable2.enabled = true;
        doorRb1.isKinematic = false;
        doorRb2.isKinematic = false;

        lockTransform.LeanScale(Vector3.zero, .5f).setEaseInExpo();
        transform.LeanScale(Vector3.zero, .5f).setEaseInExpo();
        GameObject.Destroy(GameObject.Instantiate(keyUnlockParticles, lockTransform.position, Quaternion.identity), 2f);
        GameObject.Destroy(GameObject.Instantiate(keyUnlockParticles, transform.position, Quaternion.identity), 2f);
        GameObject.Destroy(gameObject, 2f);

    } // END Unlock


    #endregion


} // END Key.cs
