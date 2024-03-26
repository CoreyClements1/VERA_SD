using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using System.Linq;
using TMPro;

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
    List <GameObject> interactableMenus;
    GameObject InteractableList;
    public GameObject select;
    Button selectBttn;
    public  GameObject buttonPrefab;
    public GameObject options;
    private UIOptions settings;

    void Awake(){
        settings = options.GetComponent<UIOptions>();
        panelGroup = gameObject.transform.Find("Panels").gameObject;
        interactionPanel = panelGroup.transform.Find("Interactables").gameObject;
        InteractableList= interactionPanel.transform.Find("InteractableList").gameObject;
        interactableMenus = new List<GameObject>();
        interactables = selectionController.grabAllSelectables();
         foreach (GameObject interactable in interactables)
        {
            GameObject emptyMenu = new GameObject(interactable.name);
            setupMenu(emptyMenu);
            interactableMenus.Add(emptyMenu);
            SetupButtons(interactable,  emptyMenu);
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

        panels = panels.OrderBy(go => go.GetComponent<Transform>().position.y).ToList();
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < panels.Count; i++){
            if(i == activePanel){
                colorSwitch(settings.primaryColor, panels[i]);
                // Debug.Log(UIElements[i].transform.name);
            }
            if(i != activePanel){
                colorSwitch(new Color(1f, 1f, 1f, 0.39f), panels[i]);
            }
        }
        for(int i = 0; i < buttons.Count; i++){
            if(i == active){
                colorSwitch(settings.secondaryColor, buttons[i]);
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

        foreach(GameObject menu in interactableMenus ){
            menu.SetActive(false);
        }
    }

    public void handleButton(){
       buttons[active].GetComponent<Button>().onClick.Invoke();
    }

    void runInteration(string interaction, VERA_Interactable interactor)
    {
        interactor.TriggerInteraction(interaction);

    }

    public void setupMenu(GameObject menu){
        // Setting up the parent
        menu.transform.parent = InteractableList.transform;
        Transform newTransform = menu.transform;
        // setting up visuals
        newTransform.localScale = new Vector3(1f, 1f, 1f);
        menu.SetActive(false);
        // Making the buttons fit the available panel
        GridLayoutGroup grid = newTransform.gameObject.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(100f, 25f );
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = UnityEngine.UI.GridLayoutGroup.Axis.Horizontal;
        grid.constraint =  UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
    }

    public void SetupButtons(GameObject interactable, GameObject parent){
        List<string> InteractInfo = interactable.GetComponent<VERA_Interactable>().GetInteractions();
        VERA_Interactable accessInteraction = interactable.GetComponent<VERA_Interactable>();
        int size = InteractInfo.Count;
       
        // Assets/UI/Tabbing UI/Button.prefab
        for(int i = 0; i < size; i++){
            GameObject button = Instantiate(buttonPrefab, parent.transform) as GameObject;
            Transform textTransform = button.transform.GetChild(0);
            TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
            buttonText.text = InteractInfo[i];
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(() => accessInteraction.TriggerInteraction(buttonText.text));
        }

    }

    public void ChangeSelection()
    {
        foreach(GameObject menu in interactableMenus ){
            menu.SetActive(false);
        }
        selectBttn.onClick.RemoveAllListeners();
       
    }

    public void selectObject(){
        string name = selectionController.currentObj;
       foreach(GameObject menu in interactableMenus ){
            if(menu.transform.name == name)
            {
                menu.SetActive(true);
            }
        }
        colorSwitch(Color.white, buttons[active]);
        active = 0;
         buttons = new List<GameObject>();
        for (int i = 0; i < InteractableList.transform.Find(name) .childCount; i++)
        {
            buttons.Add(InteractableList.transform.Find(name).GetChild(i).gameObject);
        }
    }

     public void deselectObject(){
        string name = selectionController.currentObj;
        colorSwitch(Color.white, buttons[active]);
        selectPanel();
       foreach(GameObject menu in interactableMenus ){
            menu.SetActive(false);
        }
       
    }
    
    






}
