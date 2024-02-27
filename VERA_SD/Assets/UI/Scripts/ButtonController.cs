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
            
            }
        }
    
    void FadeToColor(Color color, Button btn){
            Graphic graphic = btn.GetComponent<Graphic>();
            graphic.CrossFadeColor(color, btn.colors.fadeDuration, true, true);
        }
    
}
