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
    public List<InputActionReference> keyActions; // Array of InputActionReferences
    public GameObject options;
    private UIOptions settings;
    bool twoInputs;
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
        keyActions = new List<InputActionReference>(); // Assuming max of 4 buttons
        twoInputs = settings.twoInputs;
        twoInputHighlight = settings.twoInputHighlight;
        // Assign actions from UIOptions (modify as needed)
        keyActions.Add(settings.key1);
        keyActions.Add(settings.key2);
        if(!twoInputs){
                keyActions.Add(settings.key3);
                keyActions.Add(settings.key4);
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
        if(context.performed){
        resetColors();
        buttons[0].onClick.Invoke();
        FadeToColor(buttons[0].colors.pressedColor, buttons[0]);
        }
    }
    private void two(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        if(context.performed){
        resetColors();
        buttons[1].onClick.Invoke();
        FadeToColor(buttons[1].colors.pressedColor, buttons[1]);
        }
    }
    private void three(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        if(context.performed){
        resetColors();
        buttons[2].onClick.Invoke();
        FadeToColor(buttons[2].colors.pressedColor, buttons[2]);
        }
    }
    private void four(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        if(context.performed){
        resetColors();
        buttons[3].onClick.Invoke();
        FadeToColor(buttons[3].colors.pressedColor, buttons[3]);
        }
    }

    // 2 input system
    private void down(InputAction.CallbackContext context){
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
        keyActions =keyActions.Take(buttons.Count).ToList();
        // Enable all actions
        for (int i = 0; i < keyActions.Count; i++)
        {
            keyActions[i].action.Enable();
        }

        // Subscribe to each action event
        for (int i = 0; i < keyActions.Count; i++)
        {
            if(i == 0){
                keyActions[i].action.performed += context => one(context);
            }
            else if(i == 1){
                 keyActions[i].action.performed += context => two(context);
            }
            else if(i == 2){
                 keyActions[i].action.performed += context => three(context);
            }
            else  if(i == 3){
                keyActions[i].action.performed += context => four(context);
            }
            
        }
    }
    void fourInputDisable(){
         for (int i = 0; i < keyActions.Count; i++)
        {
                if(i == 0){
                    keyActions[i].action.performed -= one;
                }
                else if(i == 1){
                        keyActions[i].action.performed -=two;
                }
                else if(i == 2){
                        keyActions[i].action.performed -= three;
                }
                else  if(i == 3){
                    keyActions[i].action.performed -=four;
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

    
}
