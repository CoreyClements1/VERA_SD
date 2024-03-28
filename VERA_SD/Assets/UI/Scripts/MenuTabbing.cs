using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;


public class MenuTabbing : MonoBehaviour
{
    public GameObject menu;
     GameObject treeLevel1;
    List<GameObject> UIElements;
    List<GameObject> Submenus;
    int active;
    List<Transform> menuOptions;
    int activeMenuItem;
    EventSystem eventSystem;
    public GameObject controller;
    public GameObject mainUI;
    public GameObject mainMenu;
    GameObject homeMenu;

    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        setupMenu();
        homeMenu = menu;

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < UIElements.Count; i++){
            if(i == active){
                colorSwitch(Color.red, UIElements[i]);
                // Debug.Log(UIElements[i].transform.name);
            }
            if(i != active){
                colorSwitch(Color.white, UIElements[i]);
            }
        }
        if(UIElements[active].GetComponent<TMP_Dropdown>() == null){
            menuOptions = new List<Transform>();
        }
        if(UIElements[active].GetComponent<TMP_Dropdown>() != null){
            for(int i = 0; i < menuOptions.Count; i++){
                if(i == activeMenuItem){
                    colorSwitchDropDown(Color.red, menuOptions[i]);
                    // Debug.Log(UIElements[i].transform.name);
                }
                if(i != activeMenuItem){
                    colorSwitchDropDown(Color.white, menuOptions[i]);
                }
            }
        }

        
    }

    void colorSwitch(Color c, GameObject g){
        if(g.GetComponent<Toggle>() != null){
                    Toggle toggle = g.GetComponent<Toggle>();
                    ColorBlock colors = toggle.colors;
                    colors.normalColor =c; // Set normal state to yellow
                    toggle.colors = colors;

                }
        if(g.GetComponent<Slider>() != null){
            Slider slider = g.GetComponent<Slider>();
            ColorBlock colors = slider.colors;
            colors.normalColor =c; // Set normal state to yellow
            slider.colors = colors;
        }
        if(g.GetComponent<TMP_Dropdown>() != null){
            Image dropdown =g.GetComponent<Image>();
            dropdown.color =  c;
        }
         if(g.GetComponent<Button>() != null){
            Image dropdown =g.GetComponent<Image>();
            dropdown.color =  c;
        }

    }

    void colorSwitchDropDown(Color c, Transform g){
        if(g != null){
            GameObject background = g.gameObject;
            Toggle toggle =background.GetComponent<Toggle>();
            ColorBlock colors = toggle.colors;
            colors.normalColor =c; 
            colors.highlightedColor =c; 
            colors.pressedColor =c; 
            colors.selectedColor =c; 
            toggle.colors = colors;}
    }


    public void up(){
        active--;
        if(active < (0)){
            active = ( UIElements.Count-1);
        }
    }

    public void down(){
        active++;
        if(active == UIElements.Count){
            active = 0;
        }
    }

       


    public void menuSelector(){
        for(int i = 0; i< Submenus.Count; i++){
            if((UIElements[active].GetComponent<Toggle>() != null) && (Submenus[i].name == "Toggle")){
                    Submenus[i].SetActive(true);

            }
            if((UIElements[active].GetComponent<Slider>() != null) && (Submenus[i].name == "Slider")){
                    Submenus[i].SetActive(true);
            }
            if((UIElements[active].GetComponent<TMP_Dropdown>() != null) && (Submenus[i].name == "Dropdown")){
                menuOptions = new List<Transform>();
                    Submenus[i].SetActive(true);
                    UIElements[active].GetComponent<TMP_Dropdown>().Show();
                    Transform child = UIElements[active].transform.Find("Dropdown List");
                    Transform grandchild = child.gameObject.transform.Find("Viewport");
                    Transform greatGrandchild =grandchild.gameObject.transform.Find("Content");
                    // GameObject items = greatGrandchild.gameObject;
                    Debug.Log(greatGrandchild.name);
                    for(int j = 1; j < greatGrandchild.childCount; j++){
                        menuOptions.Add(greatGrandchild.GetChild(j));
                    }
                    Debug.Log(menuOptions.Count);
                    activeMenuItem = 0;
            }
            if((UIElements[active].GetComponent<Button>() != null) && (Submenus[i].name == "Button")){
                    Submenus[i].SetActive(true);
            }

        }

    }

    public void handleToggle(){
        if (menu.GetComponent<ToggleGroup>() == null){
            if (UIElements[active].GetComponent<Toggle>().isOn)
            {
                UIElements[active].GetComponent<Toggle>().isOn = false;
            }
            else
            {
                UIElements[active].GetComponent<Toggle>().isOn = true;
            }
        }
        else
        {
            ToggleGroup t = menu.GetComponent<ToggleGroup>();
            t.NotifyToggleOn(UIElements[active].GetComponent<Toggle>());
            UIElements[active].GetComponent<Toggle>().isOn = true;

        }
    }

    public void handleButton(){
       UIElements[active].GetComponent<Button>().onClick.Invoke();
    }
    

    public void increaseSlider(){
        if(UIElements[active].GetComponent<Slider>().value < UIElements[active].GetComponent<Slider>().maxValue){
            UIElements[active].GetComponent<Slider>().value ++;
        }
    }

    public void decreaseSlider(){
        if(UIElements[active].GetComponent<Slider>().value > UIElements[active].GetComponent<Slider>().minValue){
            UIElements[active].GetComponent<Slider>().value --;
        }
    }

    public void hideDropdown(){
        UIElements[active].GetComponent<TMP_Dropdown>().Hide();
    }

    public void upDropdown(){
        activeMenuItem--;
        if(activeMenuItem < (0)){
            activeMenuItem = ( menuOptions.Count-1);
        }
    }

    public void downDropdown(){
        activeMenuItem++;
        if(activeMenuItem == menuOptions.Count){
            activeMenuItem = 0;
        }
    
    }

    public void selectDropdown(){
        UIElements[active].GetComponent<TMP_Dropdown>().value = activeMenuItem;
    }

    public void buttonCheck(){
        Debug.Log("Pressed");
    }

    public string getActiveName()
    {

            if ((UIElements[active].GetComponent<Toggle>() != null))
            {
                return ("Toggle");

            }
            if ((UIElements[active].GetComponent<Slider>() != null))
            {
                return ("Slider");
            }
            if ((UIElements[active].GetComponent<TMP_Dropdown>() != null))
            {
                return ("Dropdown");
            }
            else
            {
                return ("Button");
            }

        
    }

    public void setupMenu()
    {
        active = 0;
        UIElements = new List<GameObject>();
        for (int i = 0; i < menu.transform.childCount; i++)
        {
            UIElements.Add(menu.transform.GetChild(i).gameObject);
        }

        UIElements = UIElements.OrderBy(go => go.GetComponent<Transform>().position.y).ToList();

        Submenus = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name != "Navigation")
            {
                Submenus.Add(gameObject.transform.GetChild(i).gameObject);
            }
            if (gameObject.transform.GetChild(i).name == "Navigation")
            {
                treeLevel1 = gameObject.transform.GetChild(i).gameObject;
            }
        }

    }


    public void changeMenu(GameObject newMenu)
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            colorSwitch(Color.white, UIElements[i]);
        }
        controller.GetComponent<TabbingUINavigation>().deselectUI();
        menu = newMenu;
        setupMenu();
    }

    public void bacckAMenu() {
        for (int i = 0; i < UIElements.Count; i++)
        {
            colorSwitch(Color.white, UIElements[i]);
        }
        menu = menu.GetComponent<MenuData>().backMenu;
        menu.SetActive(true);
        setupMenu();
        
    }
    public void resetMenus()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            colorSwitch(Color.white, UIElements[i]);
        }
        menu.SetActive(false);
        menu = homeMenu;
        menu.SetActive(true);
        setupMenu();

    }

    public GameObject getActiveGameObject()
    {
        return UIElements[active];
    }

    public void leaveMenu()
    {
        resetMenus();
        mainMenu.SetActive(false);
        controller.SetActive(false);
        mainUI.SetActive(true);   
    }
    public void openMenu() {
        Debug.Log("opening Menu "+ menu.transform.name);

        mainMenu.SetActive(true);
        menu.SetActive(true);  
    }





}
