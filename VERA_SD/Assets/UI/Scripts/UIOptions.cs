using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class UIOptions : MonoBehaviour
{
   public bool twoInputs;
   public bool isTree;
   public Color primaryColor;
   public Color secondaryColor;
   public Color twoInputHighlight;
   public GameObject tree;
//    public GameObject tree4;
   public GameObject tab;
   public  InputActionReference key1;
   public InputActionReference key2;
   public InputActionReference key3;
   public InputActionReference key4;


    void Awake()
    {
        // tree2.SetActive(false);
        tree.SetActive(false);
        tab.SetActive(false);
        if(isTree){
            tree.SetActive(true);
            // if(twoInputs){
            //     tree2.SetActive(true);
            // }else{
            //     tree4.SetActive(true);
            // }
        }
        else{
            tab.SetActive(true);
            // if(twoInputs){
            //     tab2.SetActive(true);
            // }else{
            //     tab4.SetActive(true);
            // }
        }
        
    }

    
}
