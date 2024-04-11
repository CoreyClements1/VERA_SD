using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInteractions : MonoBehaviour
{

    // ExampleInteractions provides a simple sample interaction which can be triggered by interactables


    #region VARIABLES


    private Renderer rend;


    #endregion


    #region MONOBEHAVIOUR


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        rend = GetComponent<Renderer>();

    } // END Start


    #endregion


    #region SAMPLE INTERACTIONS


    // Sets the color of the interactable's mesh
    //--------------------------------------//
    public void ChangeColor(string color)
    //--------------------------------------//
    {
        switch(color.ToLower())
        {
            case "r":
            case "red":
                rend.material.SetColor("_Color", Color.red);
                break;
            case "g":
            case "green":
                rend.material.SetColor("_Color", Color.green);
                break;
            case "b":
            case "blue":
                rend.material.SetColor("_Color", Color.blue);
                break;
            case "y":
            case "yellow":
                rend.material.SetColor("_Color", Color.yellow);
                break;
            case "p":
            case "purple":
                rend.material.SetColor("_Color", Color.magenta);
                break;
            default:
                rend.material.SetColor("_Color", Color.black);
                break;
        }

    } // END SetColor


    // Destroys the interactable
    //--------------------------------------//
    public void DestroyInteractable()
    //--------------------------------------//
    {
        Destroy(gameObject);

    } // END DestroyInteractable


    // Grows the interactable
    //--------------------------------------//
    public void Grow()
    //--------------------------------------//
    {
        transform.localScale = transform.localScale * 1.5f;

    } // END Grow


    // Shrinks the interactable
    //--------------------------------------//
    public void Shrink()
    //--------------------------------------//
    {
        transform.localScale = transform.localScale * .666f;

    } // END Shrink

    #endregion


} // END ExampleInteractions.cs
