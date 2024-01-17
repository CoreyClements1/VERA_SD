using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    
    public GameObject[] buttons;
    public KeyCode[] keys;
    public GameObject group;
    private Button[] btns;


    void Awake(){
        btns = new Button[buttons.Length];
        for(int i = 0; i < keys.Length; i++ ){
            btns[i] = buttons[i].GetComponent<Button>(); 
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if(group.activeInHierarchy){
             for(int i = 0; i < btns.Length; i++ ){
                    if(Input.GetKeyDown(keys[i])){
                    btns[i].onClick.Invoke();
                    FadeToColor(btns[i].colors.pressedColor, btns[i]);
                } else if(Input.GetKeyUp(keys[i])){
                    FadeToColor(btns[i].colors.normalColor, btns[i]);
                }
             }
            
            // if(button1){
            //     if(Input.GetKeyDown(keyInput1)){
            //         btn1.onClick.Invoke();
            //         FadeToColor(btn1.colors.pressedColor, btn1);
            //     } else if(Input.GetKeyUp(keyInput1)){
            //         FadeToColor(btn1.colors.normalColor, btn1);
            //     }
            // }
            // if(button2){
            //     if(Input.GetKeyDown(keyInput2)){
            //         btn2.onClick.Invoke();
            //         FadeToColor(btn2.colors.pressedColor, btn2);
            //     } else if(Input.GetKeyUp(keyInput2)){
            //         FadeToColor(btn2.colors.normalColor, btn2);
            //     }
            // }
            //  if(button3){
            //     if(Input.GetKeyDown(keyInput3)){
            //         btn3.onClick.Invoke();
            //         FadeToColor(btn3.colors.pressedColor, btn3);
            //     } else if(Input.GetKeyUp(keyInput3)){
            //         FadeToColor(btn3.colors.normalColor, btn3);
            //     }
            // }
            //  if(button4){
            //     if(Input.GetKeyDown(keyInput4)){
            //         btn4.onClick.Invoke();
            //         FadeToColor(btn4.colors.pressedColor, btn4);
            //     } else if(Input.GetKeyUp(keyInput4)){
            //         FadeToColor(btn4.colors.normalColor, btn4);
            //     }
            }
        }
    
    void FadeToColor(Color color, Button btn){
            Graphic graphic = GetComponent<Graphic>();
            graphic.CrossFadeColor(color, btn.colors.fadeDuration, true, true);
        }
}
