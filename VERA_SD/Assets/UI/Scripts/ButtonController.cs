using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;

public class ButtonController : MonoBehaviour
{
    private List<Button> buttons;
     List<InputActionReference> keyActions; // Array of InputActionReferences
     List<InputActionReference> keyActionsAlt; 
    public GameObject options;
    private UIOptions settings;
    bool twoInputs;
    bool tree;
    int active;
    Color twoInputHighlight;

    public void OnEnable()
    {
        if(twoInputs){
            twoInputsEnable();
        }else{
           fourInputEnable();
        }
        
    }

    public void OnDisable()
    {
        if(twoInputs){
            twoInputDisable();
        }else{
            fourInputDisable();
        }
    }

    void Awake()
    {
        active = 0;
        settings = options.GetComponent<UIOptions>();
        keyActions = new List<InputActionReference>(); 
        keyActionsAlt = new List<InputActionReference>(); 
        twoInputs = settings.twoInputs;
        tree = settings.tree;
        twoInputHighlight = settings.twoInputHighlight;
        keyActions.Add(settings.key1);
        keyActions.Add(settings.key2);

        keyActionsAlt.Add(settings.key1Alt);
        keyActionsAlt.Add(settings.key2Alt);
        if(!twoInputs){
                keyActions.Add(settings.key3);
                keyActions.Add(settings.key4);

                keyActionsAlt.Add(settings.key3Alt);
                keyActionsAlt.Add(settings.key4Alt);
        }

        buttons = new List<Button>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            buttons.Add(childTransform.GetComponent<Button>());
        }
        // Debug.Log(buttons.Count);
    }

    // 4 inputs setup
    private void one(InputAction.CallbackContext context)
    {
        
        if (!gameObject.activeInHierarchy)
        {return;}
        Debug.Log("1" + buttons[0].transform.name);
        // if(tree){
        //     stall(buttons[0].transform.parent.gameObject);
        // }
        // if(context.performed){
        resetColors();
        buttons[0].onClick.Invoke();
        FadeToColor(buttons[0].colors.pressedColor, buttons[0]);
        // }
    }
    private void oneAlt(InputAction.CallbackContext context)
    {
        
        if (!gameObject.activeInHierarchy)
        {return;}
        Debug.Log("1 Alt " + buttons[0].transform.name);
        // if(tree){
        //     stall(buttons[0].transform.parent.gameObject);
        // }
        // if(context.performed){
        resetColors();
        buttons[0].onClick.Invoke();
        FadeToColor(buttons[0].colors.pressedColor, buttons[0]);
        // }
    }

    private void two(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        // if(context.performed){
        resetColors();
        Debug.Log("2" + buttons[1].transform.name);
        buttons[1].onClick.Invoke();
        FadeToColor(buttons[1].colors.pressedColor, buttons[1]);
        // }
    }
    private void twoAlt(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        // if(context.performed){
        resetColors();
        Debug.Log("2 Alt" + buttons[1].transform.name);
        buttons[1].onClick.Invoke();
        FadeToColor(buttons[1].colors.pressedColor, buttons[1]);
        // }
    }

    private void three(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        Debug.Log("3 "+ buttons[2].transform.name);
         Debug.Log(buttons[2].transform.gameObject.GetComponent<ButtonData>().levelSwitch + buttons[2].transform.parent.name);
        // if(context.performed){
            if(tree){
                stall(buttons[2].transform.parent.gameObject);
            }
        resetColors();
        buttons[2].onClick.Invoke();
        FadeToColor(buttons[2].colors.pressedColor, buttons[2]);
        // }
    }
     private void threeAlt(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        Debug.Log("3 Alt "+ buttons[2].transform.name);
       if(tree){
                stall(buttons[2].transform.parent.gameObject);
            }
        // if(context.performed){
        resetColors();
        buttons[2].onClick.Invoke();
        FadeToColor(buttons[2].colors.pressedColor, buttons[2]);
        // }
    }

    private void four(InputAction.CallbackContext context)
    {
        
        if (!gameObject.activeInHierarchy)
        {return;}
        // if(context.performed){
        Debug.Log("4 Alt "+ buttons[3].transform.name);
        resetColors();
        buttons[3].onClick.Invoke();
        FadeToColor(buttons[3].colors.pressedColor, buttons[3]);
        // }
    }
    private void fourAlt(InputAction.CallbackContext context)
    {
        
        if (!gameObject.activeInHierarchy)
        {return;}
        // if(context.performed){
        Debug.Log("4 Alt "+ buttons[3].transform.name);
        resetColors();
        buttons[3].onClick.Invoke();
        FadeToColor(buttons[3].colors.pressedColor, buttons[3]);
        // }
    }

    // 2 input system
    private void down(InputAction.CallbackContext context){
        if (!gameObject.activeInHierarchy)
        {return;}
        resetHighlight();
        active++;
        if(active == buttons.Count){
            active = 0;
        }
        colorSwitch(twoInputHighlight,buttons[active]);
    }
    private void enter(InputAction.CallbackContext context)
    {
        
        if (!gameObject.activeInHierarchy)
        {return;}
        Debug.Log("Enter"+ buttons[active].transform.name+ active);
        buttons[active].onClick.Invoke();
        FadeToColor(buttons[active].colors.normalColor, buttons[active]);
    }
    // color stuff
    void FadeToColor(Color color, Button btn)
    {
        Graphic graphic = btn.GetComponent<Graphic>();
        graphic.CrossFadeColor(color, btn.colors.fadeDuration, true, true);
    }
     void colorSwitch(Color c, Button g){
        Graphic dropdown =g.GetComponent<Graphic>();
        dropdown.color =  c;
     }
    void resetColors(){
        for(int i = 0; i < buttons.Count; i++){
              FadeToColor(buttons[i].colors.normalColor, buttons[i]);
        }
    }
    void resetHighlight(){
        for(int i = 0; i < buttons.Count; i++){
              colorSwitch(buttons[i].colors.normalColor, buttons[i]);
        }
    }
    
    void fourInputEnable(){
        Debug.Log(gameObject.transform.name+" enabled");
        keyActions =keyActions.Take(buttons.Count).ToList();
        keyActionsAlt =keyActionsAlt.Take(buttons.Count).ToList();
        // Enable all actions
        for (int i = 0; i < keyActions.Count; i++)
        {
            keyActions[i].action.Enable();
            keyActionsAlt[i].action.Enable();
        }

        // Subscribe to each action event
        for (int i = 0; i < keyActions.Count; i++)
        {
            if(i == 0){
                if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed += context => oneAlt(context);
                }
                else{
                    keyActions[i].action.performed += context => one(context);
                }
            }
            else if(i == 1){
                 if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed += context => twoAlt(context);
                }
                else{
                    keyActions[i].action.performed += context => two(context);
                }
            }
            else if(i == 2){
                 if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed += context => threeAlt(context);
                }
                else{
                    keyActions[i].action.performed += context => three(context);
                }
            }
            else  if(i == 3){
                if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed += context => fourAlt(context);
                }
                else{
                    keyActions[i].action.performed += context => four(context);
                }
            }
            
        }
    }
    void fourInputDisable(){
        // Debug.Log(gameObject.transform.name+" disabled");
        for (int i = 0; i < keyActions.Count; i++)
        {
            if(i == 0){
                if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed -= oneAlt;
                }
                else{
                    keyActions[i].action.performed -= one;
                }
            }
            else if(i == 1){
                 if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed -= twoAlt;
                }
                else{
                    keyActions[i].action.performed -= two;
                }
            }
            else if(i == 2){
                 if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed -= threeAlt;
                }
                else{
                    keyActions[i].action.performed -= three;
                }
            }
            else  if(i == 3){
                if(buttons[i].gameObject.GetComponent<ButtonData>().levelSwitch){
                    keyActionsAlt[i].action.performed -= fourAlt;
                }
                else{
                    keyActions[i].action.performed -= four;
                }
            }
            
        }
    }

    void twoInputsEnable(){
        for (int i = 0; i < keyActions.Count; i++)
        {
            keyActions[i].action.Enable();
        }
         for (int i = 0; i < keyActions.Count; i++)
        {
            if(i == 0){
                keyActions[i].action.performed += context => down(context);
            }
            else if(i == 1){
                 keyActions[i].action.performed += context => enter(context);
            }
        }

    }

    void twoInputDisable(){
         for (int i = 0; i < keyActions.Count; i++)
        {
                if(i == 0){
                    keyActions[i].action.performed -= down;
                }
                else if(i == 1){
                        keyActions[i].action.performed -=enter;
                }
                
        }
    }
    public static void stall(GameObject go)
    {
        // Debug.Log(go.transform.name+" Stall");
        // Check if the GameObject has a parent
        if (go.transform.parent == null)
        {
            Debug.LogWarning("GameObject has no parent, cannot check sibling count.");
            return;
        }

        // Loop until the GameObject is the only active child
        int activeSiblings = 0;
        foreach (Transform child in go.transform.parent)
        {
            if (child.gameObject != go && child.gameObject.activeInHierarchy)
            {
                activeSiblings++;
                break; // Exit loop if another active sibling is found
            }
        }

        if (activeSiblings > 0)
        {
            // Recheck active sibling count in each loop iteration
            activeSiblings = 0;
            foreach (Transform child in go.transform.parent)
            {
                if (child.gameObject != go && child.gameObject.activeInHierarchy)
                {
                    activeSiblings++;
                    break; // Exit loop if another active sibling is found
                }
            }
        }
    }


    
}
