using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

public class TabbingUINavigation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panelGroup;
    List<GameObject> panels;
    int activePanel;
    int active;
    List<GameObject> buttons;
    bool inSub;
    void Start()
    {
        activePanel = 0;
        inSub = false;
        panels = new List<GameObject>();
        buttons = new List<GameObject>();
        for (int i = 0; i < panelGroup.transform.childCount; i++)
        {
            panels.Add(panelGroup.transform.GetChild(i).gameObject);
        }

        panels = panels.OrderBy(go => go.GetComponent<Transform>().position.y).ToList();
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < panels.Count; i++){
            if(i == activePanel){
                colorSwitch(Color.red, panels[i]);
                // Debug.Log(UIElements[i].transform.name);
            }
            if(i != activePanel){
                colorSwitch(Color.white, panels[i]);
            }
        }
        for(int i = 0; i < buttons.Count; i++){
            if(i == active){
                colorSwitch(Color.cyan, buttons[i]);
                // Debug.Log(UIElements[i].transform.name);
            }
            if(i != active){
                colorSwitch(Color.white, buttons[i]);
            }
        }
        
    }
    void colorSwitch(Color c, GameObject g){
       Image panel =g.GetComponent<Image>();
        panel.color =  c;

    }
    public void left(){
        activePanel--;
        if(activePanel < (0)){
            activePanel = ( panels.Count-1);
        }
    }
    public void right(){
        activePanel++;
        if(activePanel == panels.Count){
            activePanel = 0;
        }
    }

    public void up(){
        active--;
        if(active < (0)){
            active = ( buttons.Count-1);
        }
    }
    public void down(){
        active++;
        if(active == buttons.Count){
            active = 0;
        }
    }

    public void selectPanel(){
        buttons = new List<GameObject>();
        active = 0;
        if( panels[activePanel].transform.name != "Interactables"){
                for (int i = 0; i < panels[activePanel].transform.childCount; i++)
                {
                    buttons.Add(panels[activePanel].transform.GetChild(i).gameObject);
                }
        }
        else{
            for (int i = 0; i < panels[activePanel].transform.Find("Menu").childCount; i++)
                {
                    buttons.Add(panels[activePanel].transform.transform.Find("Menu").GetChild(i).gameObject);
                }

        }

    }
    public void deselectPanel(){
        active = -1;
        for(int i = 0; i< buttons.Count; i++){
          colorSwitch(Color.white, buttons[i]);
        }
        buttons = new List<GameObject>();
    }

    public void handleButton(){
       buttons[active].GetComponent<Button>().onClick.Invoke();
    }




}
