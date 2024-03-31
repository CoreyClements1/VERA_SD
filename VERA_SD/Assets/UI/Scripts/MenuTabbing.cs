using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;


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
    public UIOptions settings;
    public GameObject flag;
    GameObject homeMenu;
    public GameObject outside;

    //public SelectionController selectionController;
    public Button selectButton;
    public GameObject insideInteractable;
    public GameObject inside;
    List<GameObject> flagChildren;
    void Start()
    {

        flagChildren = new List<GameObject>();
        foreach (Transform child in flag.transform)
        {
            flagChildren.Add(child.gameObject);
        }
        setupMenu();
        homeMenu = menu;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (UIElements[active].GetComponent<TMP_Dropdown>() == null)
        {
            menuOptions = new List<Transform>();
        }
        if (UIElements[active].GetComponent<TMP_Dropdown>() != null)
        {
            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == activeMenuItem)
                {
                    colorSwitchDropDown(settings.secondaryColor, menuOptions[i]);
                    // Debug.Log(UIElements[i].transform.name);
                }
                if (i != activeMenuItem)
                {
                    colorSwitchDropDown(Color.white, menuOptions[i]);
                }
            }
        }


    }

    void colorSwitch(Color c, GameObject g)
    {
        if (g.GetComponent<Toggle>() != null)
        {
            Toggle toggle = g.GetComponent<Toggle>();
            ColorBlock colors = toggle.colors;
            colors.normalColor = c; // Set normal state to yellow
            toggle.colors = colors;

        }
        if (g.GetComponent<Slider>() != null)
        {
            Slider slider = g.GetComponent<Slider>();
            ColorBlock colors = slider.colors;
            colors.normalColor = c; // Set normal state to yellow
            slider.colors = colors;
        }
        if (g.GetComponent<TMP_Dropdown>() != null)
        {
            Image dropdown = g.GetComponent<Image>();
            dropdown.color = c;
        }
        if (g.GetComponent<Button>() != null)
        {
            Image dropdown = g.GetComponent<Image>();
            dropdown.color = c;
        }

    }

    void colorSwitchDropDown(Color c, Transform g)
    {
        if (g != null)
        {
            GameObject background = g.gameObject;
            Toggle toggle = background.GetComponent<Toggle>();
            ColorBlock colors = toggle.colors;
            colors.normalColor = c;
            colors.highlightedColor = c;
            colors.pressedColor = c;
            colors.selectedColor = c;
            toggle.colors = colors;
        }
    }


    public void up()
    {
        
        active--;
        if (active < (0))
        {
            active = (UIElements.Count - 1);
        }
        moveFlag();
        handleSelectButton();
    }

    public void down()
    {
        
        active++;
        if (active == UIElements.Count)
        {
            active = 0;
        }
        moveFlag();
       
        handleSelectButton();
    }

    public void moveFlag()
    {
        
        Vector3[] v = new Vector3[4];
        UIElements[active].GetComponent<RectTransform>().GetWorldCorners(v);
        float flagY = (v[0].y + v[2].y) / 2;

        RectTransform sourceRect = UIElements[active].GetComponent<RectTransform>();
        RectTransform targetRect = flag.GetComponent<RectTransform>();



        targetRect.anchoredPosition = sourceRect.anchoredPosition;
        targetRect.sizeDelta = sourceRect.sizeDelta;
        targetRect.localScale = sourceRect.localScale;
        targetRect.pivot = sourceRect.pivot;

        targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sourceRect.rect.height);
        targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sourceRect.rect.height);
        targetRect.position = new Vector3(v[0].x, flagY, v[0].z);

        RectTransform top = flagChildren[0].GetComponent<RectTransform>();
        RectTransform left = flagChildren[1].GetComponent<RectTransform>();
        RectTransform bottom = flagChildren[2].GetComponent<RectTransform>();
        RectTransform right = flagChildren[3].GetComponent<RectTransform>();

        top.position = new Vector3(v[0].x, v[0].y, v[0].z);
        left.position = v[3];
        bottom.position = v[0];
        right.position = v[2];

        top.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sourceRect.rect.width);
        bottom.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sourceRect.rect.width);
        left.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sourceRect.rect.height);
        right.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sourceRect.rect.height);

    }




    public void menuSelector()
    {
        for (int i = 0; i < Submenus.Count; i++)
        {
            
            if ((UIElements[active].GetComponent<Slider>() != null) && (Submenus[i].name == "Slider"))
            {
                Submenus[i].SetActive(true);
            }
            if ((UIElements[active].GetComponent<TMP_Dropdown>() != null) && (Submenus[i].name == "Dropdown"))
            {
                menuOptions = new List<Transform>();
                Submenus[i].SetActive(true);
                UIElements[active].GetComponent<TMP_Dropdown>().Show();
                Transform child = UIElements[active].transform.Find("Dropdown List");
                Transform grandchild = child.gameObject.transform.Find("Viewport");
                Transform greatGrandchild = grandchild.gameObject.transform.Find("Content");
                // GameObject items = greatGrandchild.gameObject;
                //Debug.Log(greatGrandchild.name);
                for (int j = 1; j < greatGrandchild.childCount; j++)
                {
                    menuOptions.Add(greatGrandchild.GetChild(j));
                }
                //Debug.Log(menuOptions.Count);
                activeMenuItem = 0;
            }
            

        }

    }

    public void handleToggle()
    {
        if (menu.GetComponent<ToggleGroup>() == null)
        {
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

    public void handleButton()
    {
        UIElements[active].GetComponent<Button>().onClick.Invoke();
    }


    public void increaseSlider()
    {
        if (UIElements[active].GetComponent<Slider>().value < UIElements[active].GetComponent<Slider>().maxValue)
        {
            UIElements[active].GetComponent<Slider>().value++;
        }
    }

    public void decreaseSlider()
    {
        if (UIElements[active].GetComponent<Slider>().value > UIElements[active].GetComponent<Slider>().minValue)
        {
            UIElements[active].GetComponent<Slider>().value--;
        }
    }

    public void hideDropdown()
    {
        UIElements[active].GetComponent<TMP_Dropdown>().Hide();
    }

    public void upDropdown()
    {
        activeMenuItem--;
        if (activeMenuItem < (0))
        {
            activeMenuItem = (menuOptions.Count - 1);
        }
    }

    public void downDropdown()
    {
        activeMenuItem++;
        if (activeMenuItem == menuOptions.Count)
        {
            activeMenuItem = 0;
        }

    }

    public void selectDropdown()
    {
        UIElements[active].GetComponent<TMP_Dropdown>().value = activeMenuItem;
    }

    public void buttonCheck()
    {
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
            if ((menu.transform.GetChild(i).GetComponent<Button>() != null) || (menu.transform.GetChild(i).GetComponent<Toggle>() != null) || (menu.transform.GetChild(i).GetComponent<Button>() != null) || (menu.transform.GetChild(i).GetComponent<Slider>() != null ) || (menu.transform.GetChild(i).GetComponent<Dropdown>() != null)){
                UIElements.Add(menu.transform.GetChild(i).gameObject);
            }
            
        }
        //Debug.Log("UI elements: "+ UIElements.Count);
        moveFlag();
        handleSelectButton();
        //flag.GetComponent
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
        controller.GetComponent<TabbingUINavigation>().deselectUI();
        menu = newMenu;
        setupMenu();
        menu.SetActive(true);
    }

    public void bacckAMenu()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            colorSwitch(Color.white, UIElements[i]);
        }
        menu.SetActive(false);
        menu = menu.GetComponent<MenuData>().backMenu;
        menu.SetActive(true);
        active = 0;
        setupMenu();
        moveFlag();
        
    }
    public void resetMenus()
    {
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
        flag.SetActive(false);
       
        inside.SetActive(false);
        insideInteractable.SetActive(false);
        outside.SetActive(true);
        //mainMenu.SetActive(false ); 
        Debug.Log("leaving");
        active = 0;
        mainUI.GetComponent<TabbingUINavigation>().selectSpecificPanel(0);
        Debug.Log("leaving still");
        menu.SetActive(false);
    }
    public void openMenu()
    {
        //Debug.Log("opening Menu " + menu.transform.name);
        
        mainMenu.SetActive(true);
        menu.SetActive(true);
        inside.SetActive(true);
        moveFlag();
        flag.SetActive(true);
    }


    // button: n= 1 check: n=2 dropdown/slider: n = 3
    public void handleSelectButton()
    {

        string activeItem = getActiveName();
        if (activeItem == "Button")
        {
            Transform textTransform = selectButton.transform.GetChild(0);
            TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
            buttonText.text = "Press Button";
        }
        if(activeItem == "Toggle")
        {
            Transform textTransform = selectButton.transform.GetChild(0);
            TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
            
            if (UIElements[active].GetComponent<Toggle>().isOn)
            {
                buttonText.text = "Uncheck";
            }
            else
            {
                buttonText.text = "Check";
            }
        }
        if((activeItem == "Dropdown") || (activeItem == "Slider"))
        {
            Transform textTransform = selectButton.transform.GetChild(0);
            TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
            if (activeItem == "Dropdown")
            {
                buttonText.text = "Change Selection";
            }
            if(activeItem == "Slider")
            {
                buttonText.text = "Change Value";
            }
            

        }

    }

    public void handleSelectEvent()
    {
        string activeItem = getActiveName();
        if (activeItem == "Button")
        {
            handleButton();
            Debug.Log("it's a button");
        }
        if (activeItem == "Toggle")
        {
            selectButton.onClick.AddListener(() => handleToggle());
            Debug.Log("it's a toggle");
        }
        if ((activeItem == "Dropdown") || (activeItem == "Slider"))
        {
            menuSelector();

            mainUI.GetComponent<TabbingUINavigation>().selectUI();
            mainUI.GetComponent<TabbingUINavigation>().selectSpecificPanel(2);
            inside.SetActive(false);
            insideInteractable.SetActive(true);

        }

    }



}
