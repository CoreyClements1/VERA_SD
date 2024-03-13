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
    int buttonIndex;

    public void OnEnable()
    {
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

    public void OnDisable()
    {
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

    void Awake()
    {
        settings = options.GetComponent<UIOptions>();
        keyActions = new List<InputActionReference>(); // Assuming max of 4 buttons

        // Assign actions from UIOptions (modify as needed)
        keyActions.Add(settings.key1);
        keyActions.Add(settings.key2);
        keyActions.Add(settings.key3);
        keyActions.Add(settings.key4);

        buttons = new List<Button>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            buttons.Add(childTransform.GetComponent<Button>());
        }
    }

    private void one(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        buttons[0].onClick.Invoke();
        FadeToColor(buttons[0].colors.pressedColor, buttons[0]);
    }
    private void two(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        buttons[1].onClick.Invoke();
        FadeToColor(buttons[1].colors.pressedColor, buttons[1]);
    }
    private void three(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        buttons[2].onClick.Invoke();
        FadeToColor(buttons[2].colors.pressedColor, buttons[2]);
    }
    private void four(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
        {return;}
        buttons[3].onClick.Invoke();
        FadeToColor(buttons[3].colors.pressedColor, buttons[3]);
    }


    void FadeToColor(Color color, Button btn)
    {
        Graphic graphic = btn.GetComponent<Graphic>();
        graphic.CrossFadeColor(color, btn.colors.fadeDuration, true, true);
    }
}
