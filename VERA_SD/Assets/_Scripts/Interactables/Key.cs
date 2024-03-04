using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{

    // Key handles the key object and lock object


    #region VARIABLES


    [SerializeField] private Transform lockTransform;
    [SerializeField] private XRGrabInteractable doorGrabInteractable1, doorGrabInteractable2;
    [SerializeField] private ParticleSystem keyUnlockParticles;
    private bool canUnlock = true;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
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

    } // END LockDoors


    // Tries to unlock door if we are close enough
    //--------------------------------------//
    private void TryUnlock()
    //--------------------------------------//
    {
        if ((lockTransform.position - transform.position).magnitude < 0.5f)
        {
            Unlock();
            canUnlock = false;
        }

    } // END TryUnlock


    // Unlocks door by allowing joint movement
    //--------------------------------------//
    private void Unlock()
    //--------------------------------------//
    {
        doorGrabInteractable1.enabled = true;
        doorGrabInteractable2.enabled = true;
        lockTransform.LeanScale(Vector3.zero, .5f).setEaseInExpo();
        transform.LeanScale(Vector3.zero, .5f).setEaseInExpo();
        GameObject.Destroy(GameObject.Instantiate(keyUnlockParticles, lockTransform.position, Quaternion.identity), 2f);
        GameObject.Destroy(GameObject.Instantiate(keyUnlockParticles, transform.position, Quaternion.identity), 2f);
        GameObject.Destroy(gameObject, 2f);

    } // END Unlock


    #endregion


} // END Key.cs
