using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class UIOptions : MonoBehaviour
{
   public bool twoInputs;
   public bool tree;
   public Color primaryColor;
   public Color secondaryColor;
   public Color twoInputHighlight;
   public GameObject tree2;
   public GameObject tree4;
   public GameObject tab2;
   public GameObject tab4;
   public  InputActionReference key1;
   public InputActionReference key2;
   public InputActionReference key3;
   public InputActionReference key4;


    void Awake()
    {
        // tree2.SetActive(false);
        // tree4.SetActive(false);
        // tab2.SetActive(false);
        // tab4.SetActive(false);
        // if(tree){
        //     if(twoInputs){
        //         tree2.SetActive(true);
        //     }else{
        //         tree4.SetActive(true);
        //     }
        // }
        // else{
        //     if(twoInputs){
        //         tab2.SetActive(true);
        //     }else{
        //         tab4.SetActive(true);
        //     }
        // }
        
    }

    
}
