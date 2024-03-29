using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannonball : MonoBehaviour
{


    #region VARIABLES


    [SerializeField] private ParticleSystem explosionParticlePrefab;


    #endregion


    #region COLLISION


    // On collision, destroy cannonball
    //--------------------------------------//
    private void OnCollisionEnter(Collision collision)
    //--------------------------------------//
    {
        Destroy(GameObject.Instantiate(explosionParticlePrefab, transform.position, explosionParticlePrefab.transform.rotation), 2f);
        Destroy(gameObject);

    } // END OnCollisionEnter


    #endregion


    public void DestroyCannonball()
    {

    }


} // END Cannonball.cs
