using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using System.Linq;
using TMPro;
using System;
using System.Threading;

public class TabbingUINavigation : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject panelGroup;
    GameObject panelGroup;
    List<GameObject> panels;
    int activePanel;
    int active;
    List<GameObject> buttons;
    // bool inSub;

    // Interacatables stuff 
    [SerializeField] SelectionController selectionController;
    private List<GameObject> interactables;
    // public GameObject interactionPanel;
    GameObject interactionPanel;
    List<GameObject> interactableMenus;
    List<GameObject> UIMenus;
    GameObject InteractableList;
    GameObject UIList;
    public GameObject select;
    Button selectBttn;
    public GameObject buttonPrefab;
    public GameObject options;
    private UIOptions settings;
    public GameObject menuManager;
    int height;



    void Awake()
    {
        settings = options.GetComponent<UIOptions>();
        panelGroup = gameObject.transform.Find("Panels").gameObject;
        interactionPanel = panelGroup.transform.Find("Interactables Inside").gameObject;
        InteractableList = interactionPanel.transform.Find("InteractableList").gameObject;
        UIList = panelGroup.transform.Find("Settings Inside").gameObject;

        interactableMenus = new List<GameObject>();
        UIMenus = new List<GameObject>();
        interactables = selectionController.grabAllSelectables();
        
            foreach (GameObject interactable in interactables)
            {
                GameObject emptyMenu = new GameObject(interactable.name);
                setupMenu(emptyMenu);
                interactableMenus.Add(emptyMenu);
                SetupButtons(interactable, emptyMenu);
            }
        
            for (int i = 0; i < UIList.transform.childCount; i++)
            {
                UIMenus.Add(UIList.transform.GetChild(i).gameObject);
            }

        
        selectBttn = select.GetComponent<Button>();

    }

    void Start()
    {
        activePanel = 0;
        // inSub = false;
        panels = new List<GameObject>();
        buttons = new List<GameObject>();
        
            for (int i = 0; i < panelGroup.transform.childCount; i++)
            {
                panels.Add(panelGroup.transform.GetChild(i).gameObject);
                
            }
            
            //panels = panels.OrderBy(go => go.GetComponent<Transform>().position.y).ToList();
      
        selectPanel();





    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == active)
            {
                colorSwitch(settings.secondaryColor, buttons[i]);

            }
            if (i != active)
            {
                colorSwitch(Color.white, buttons[i]);
            }
        }
        if((activePanel != 0)  && (activePanel != 6))
        {
            panelGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);
        }
        else if((activePanel == 0))
        {
            panelGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
        }
        else
        {
            int size = ((height / 4) * 40) + 40;
            panelGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }
        

    }


    void colorSwitch(Color c, GameObject g)
    {
        Image panel = g.GetComponent<Image>();
        panel.color = c;

    }


    public void left()
    {
        activePanel--;
        if (activePanel < (0))
        {
            activePanel = (panels.Count - 1);
        }
    }

    public void right()
    {
        activePanel++;
        if (activePanel == panels.Count)
        {
            activePanel = 0;
        }
    }

    public void up()
    {
        active--;
        if (active < (0))
        {
            active = (buttons.Count - 1);
        }
    }


    public void down()
    {
        active++;
        if (active == buttons.Count)
        {
            active = 0;
        }
    }

    public void selectPanel()
    {
        //if(buttons != null)
        //{
        //    for (int i = 0; i < buttons.Count; i++)
        //    {
        //        colorSwitch(new Color(1f, 1f, 1f, 0.39f), buttons[i]);   
        //    }
        //}
        buttons = new List<GameObject>();
        active = 0;
      
        panels[activePanel].SetActive(true);
        if ((panels[activePanel].transform.name != "Interactables Inside") && (panels[activePanel].transform.name != "Settings Inside"))
        {
            for (int i = 0; i < panels[activePanel].transform.childCount; i++)
            {
                buttons.Add(panels[activePanel].transform.GetChild(i).gameObject);
                colorSwitch(new Color(1f, 1f, 1f, 1f), buttons[i]);
            }
        }
        else if(panels[activePanel].transform.name == "Settings Inside")
        {
            for (int i = 0; i < UIMenus.Count; i++)
            {
                if (UIMenus[i].transform.name == menuManager.GetComponent<MenuTabbing>().getActiveName() )
                {
                    
                    for (int j = 0; j < UIMenus[i].transform.childCount; j++)
                    {
                        buttons.Add(UIMenus[i].transform.GetChild(j).gameObject);
                    }
                    Debug.Log(buttons.Count);
                }
            }

        }
        else
        {
            for (int i = 0; i < InteractableList.transform.childCount; i++)
            {
                if (InteractableList.transform.GetChild(i).gameObject.activeSelf)
                {
                    for(int j = 0; j < InteractableList.transform.GetChild(i).childCount; j++)
                    {
                        buttons.Add(InteractableList.transform.GetChild(i).GetChild(j).gameObject);
                    }
                }
            }
            height = buttons.Count;

        }
            
        
        
        panels[activePanel].SetActive(true);

    }

    public void selectSpecificPanel(int newPanel)
    {
        if(activePanel != 0){
            panels[activePanel].SetActive(false);
        }
        //hidePanels(newPanel);
        //showPanel(newPanel);
        activePanel = newPanel;
        active = 0;
        selectPanel();

    }





    public void handleButton()
    {
        
        if (buttons[active] == null)
        {
            return;
        }
        // Debug.Log(panels[activePanel].transform.name);
        int oldActive = active;
        Thread.Sleep(30);
        buttons[oldActive].GetComponent<Button>().onClick.Invoke();
        
        //Debug.Log(panels[activePanel].transform.name);
    }

    void runInteration(string interaction, VERA_Interactable interactor)
    {
        interactor.TriggerInteraction(interaction);

    }

    public void setupMenu(GameObject menu)
    {
        // Setting up the parent
        menu.transform.parent = InteractableList.transform;
        Transform newTransform = menu.transform;
        // setting up visuals
        newTransform.localScale = new Vector3(1f, 1f, 1f);
        newTransform.localPosition = new Vector3(0f, 0f, 0f);
        newTransform.localRotation = Quaternion.Euler(0, 0, 0);
        menu.SetActive(false);
        // Making the buttons fit the available panel
        GridLayoutGroup grid = newTransform.gameObject.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(100f, 25f);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = UnityEngine.UI.GridLayoutGroup.Axis.Horizontal;
        grid.constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
    }

    public void SetupButtons(GameObject interactable, GameObject parent)
    {
        List<string> InteractInfo = interactable.GetComponent<VERA_Interactable>().GetInteractions();
        VERA_Interactable accessInteraction = interactable.GetComponent<VERA_Interactable>();
        int size = InteractInfo.Count;

        // Assets/UI/Tabbing UI/Button.prefab
        for (int i = 0; i < size; i++)
        {
            GameObject button = Instantiate(buttonPrefab, parent.transform) as GameObject;
            button.transform.localPosition = new Vector3(0f, 0f, 0f);
            Transform textTransform = button.transform.GetChild(0);
            TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
            buttonText.text = InteractInfo[i];
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(() => accessInteraction.TriggerInteraction(buttonText.text));
        }

    }

    public void ChangeSelection()
    {
        foreach (GameObject menu in interactableMenus)
        {
            menu.SetActive(false);
        }
        selectBttn.onClick.RemoveAllListeners();

    }

    public void selectObject()
    {
        string name = selectionController.currentObj.name;
        foreach (GameObject menu in interactableMenus)
        {
            if (menu.transform.name == name)
            {
                menu.SetActive(true);
            }
        }
        colorSwitch(Color.white, buttons[active]);
        active = 0;
        buttons = new List<GameObject>();
        for (int i = 0; i < InteractableList.transform.Find(name).childCount; i++)
        {
            buttons.Add(InteractableList.transform.Find(name).GetChild(i).gameObject);
        }
    }

    public void deselectObject()
    {
        string name = selectionController.currentObj.name;
        colorSwitch(Color.white, buttons[active]);
        selectPanel();
        foreach (GameObject menu in interactableMenus)
        {
            menu.SetActive(false);
        }

    }

    public void selectUI()
    {

        string name = menuManager.GetComponent<MenuTabbing>().getActiveName();
        if((name == "Slider") || (name == "Dropdown")){
            Debug.Log("check");
       //colorSwitch(Color.white, buttons[active]);
        active = 0;
        buttons = new List<GameObject>();
            UIList.transform.Find(name).gameObject.SetActive(true);
            for (int i = 0; i < UIList.transform.Find(name).childCount; i++)
            {
                buttons.Add(UIList.transform.Find(name).GetChild(i).gameObject);
            }
        }
    }

    public void deselectUI()
    {
        string name = menuManager.GetComponent<MenuTabbing>().getActiveName(); ;
        colorSwitch(Color.white, buttons[active]);
        //selectPanel();
        foreach (GameObject menu in UIMenus)
        {
            menu.SetActive(false);
        }

    }

    public void hidePanels(int n)
    {

        for (int i = 1; i < panels.Count; i++)
        {
            if (i != n)
            {
                colorSwitch(new Color(1f, 1f, 1f, 0.0f), panels[i]);
                for (int j = 0; j < panels[i].transform.childCount; j++)
                {
                    GameObject b = panels[i].transform.GetChild(j).gameObject;
                    if (b != InteractableList)
                    {
                        colorSwitch(new Color(1f, 1f, 1f, 0.0f), b);
                    }
                        for (int k = 0; k < b.transform.childCount; k++)
                        {
                            b.transform.GetChild(k).gameObject.SetActive(false);
                        }
                    
                }
            }
        }

    }

    public void showPanel(int n)
    {
        colorSwitch(new Color(1f, 1f, 1f, 0.39f), panels[n]);
        for (int j = 0; j < panels[n].transform.childCount; j++)
        {
            GameObject b = panels[n].transform.GetChild(j).gameObject;
            if (b != InteractableList)
            {
                colorSwitch(new Color(1f, 1f, 1f, 1f), b);

                for (int k = 0; k < b.transform.childCount; k++)
                {
                    b.transform.GetChild(k).gameObject.SetActive(true);
                }
            }
            
        }
    }

    public void avoidDouble()
    {
        active = -1;
    }






}
