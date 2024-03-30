using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class PauseMenu : MonoBehaviour
{

    //PauseMenu handles all interactions done when a user wants to adjust their settings.

    #region VARIABLES
    private bool pauseEnabled;
    private bool saveEnabled;
    public GameObject pauseMenu;
    public GameObject settings;
    public GameObject snapTurningImages;
    public GameObject saveProgressScreen;
    public GameObject options;
    public GameObject typeOptions;
    public GameObject typeImages;
    public Slider slider;
    [SerializeField] private GameObject player;
    private int index;
    private int typeIndex;
    private string typeCurrentName;
    private string currentName;
    private float prevSnap;
    private float prevSpeed;
    private int prevIndex;
    private int prevTypeIndex;
    private int currentLvl;
    private int prevLvl;
    private List<bool> typeArray;
    private List<bool> optionsArray;
    public TextMeshProUGUI sliderText;
    public GameObject MenuTab;

    #endregion
    // Start is called before the first frame update

    #region FUNCTIONS
    void Start()
    {
        pauseEnabled = false;
        saveEnabled = false;
        currentName = "free";
        typeCurrentName = "T1";
        index = 2;
        typeIndex = 0;
        prevIndex = index;
        prevTypeIndex = typeIndex;
        prevSnap = player.GetComponent<MovementController>().rotationValue;
        prevSpeed = player.GetComponent<MovementController>().speed;
        currentLvl = player.GetComponent<MovementController>().currentLvl;
        prevLvl = currentLvl;
        typeArray = new List<bool>();
        optionsArray = new List<bool>();
        for(int i = 0; i < typeOptions.transform.childCount; i++)
        {
            typeArray.Add(typeOptions.transform.GetChild(i).GetComponent<Toggle>().isOn);
        }

        for (int i = 0; i < options.transform.childCount; i++)
        {
            optionsArray.Add(typeOptions.transform.GetChild(i).GetComponent<Toggle>().isOn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseEnabled)
        {
            pauseEnabled = true;
            pauseMenu.gameObject.SetActive(pauseEnabled);
            if(pauseEnabled)
                Time.timeScale = 0;
            
            
            
        }
    }

    //this function switches to the snap turning panel
    public void snapTurning()
    {
        
        for(int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if ((child.name != "Snap Turning") && (child.name != "Panel"))
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);
           
        }
    }

    //this function changes the degree in which a user turns. Yes this is very ugly and I will fix it in the future when it bugs me too much
    public void changeTurning()
    {
        string name = MenuTab.GetComponent<MenuTabbing>().getActiveGameObject().transform.name;
        if(name != currentName)
        {
            currentName = name;
            if (name == "90")
            {
                snapTurningImages.transform.GetChild(0).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 90f;
                optionsArray[0] = true;
                optionsArray[index] = false;
                index = 0;             
            }
            else if (name == "45")
            {
                snapTurningImages.transform.GetChild(1).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 45f;
                optionsArray[1] = true;
                optionsArray[index] = false;
                index = 1;
            }
            else if (name == "free")
            {
                snapTurningImages.transform.GetChild(2).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 15f;
                optionsArray[2] = true;
                optionsArray[index] = false;
                index = 2;
            }

        }
        
    }

    //this function switches to the movement speed panel
    public void movementSpeed()
    {
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if ((child.name != "Movement Speed") && (child.name != "Panel"))
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);

        }
    }
    //this function changes the amount a user can move forward per press
    public void changeMovementSpeed()
    {
        sliderText.text = slider.value.ToString();
        player.GetComponent<MovementController>().speed = slider.value;
    }

    //this function activates the save screen window
    public void SaveScreenActive()
    {
        saveEnabled = !saveEnabled;
        saveProgressScreen.gameObject.SetActive(saveEnabled);
        

    }
    
    //saves all changes a user makes before returning to experiment
    public void SaveChanges()
    {
        prevSnap = player.GetComponent<MovementController>().rotationValue;
        prevSpeed = player.GetComponent<MovementController>().speed;

        prevIndex = index;
        prevTypeIndex = typeIndex;
        prevLvl = currentLvl;
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        pauseEnabled = false;
        Time.timeScale = 1;
    }

    //removes all changes a user makes
    public void RemoveChanges()
    {
        
        player.GetComponent<MovementController>().rotationValue = prevSnap;
        player.GetComponent<MovementController>().speed = prevSpeed;
        player.GetComponent<MovementController>().currentLvl = prevLvl;
        index = prevIndex;
        typeIndex = prevTypeIndex;
        slider.value = prevSpeed;
        
        //once type 3 is added, we'll remove -1
        for(int i = 0; i < typeOptions.transform.childCount - 1; i++)
        {
           
            Transform child = typeOptions.transform.GetChild(i);
           

            if (typeArray[i])
            {
                
                child.GetComponent<Toggle>().isOn = false;
                typeImages.transform.GetChild(i).gameObject.SetActive(false);
                
            }
           
            if (i == typeIndex)
            {
                child.GetComponent<Toggle>().isOn = true;
                typeImages.transform.GetChild(i).gameObject.SetActive(true);
            }
            
        }

        for(int i = 0; i < options.transform.childCount; i++)
        {
            Transform child = options.transform.GetChild(i);
            if (optionsArray[i])
            {
                child.GetComponent<Toggle>().isOn = false;
                snapTurningImages.transform.GetChild(i).gameObject.SetActive(false);
            }

            if(i == index)
            {
                child.GetComponent<Toggle>().isOn = true;
                snapTurningImages.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);


        }

        pauseEnabled = false;
        Time.timeScale = 1;
    }

    //menu activation for selection movement type
    public void TypeSelection()
    {
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if ((child.name != "Type Selection") && (child.name != "Panel"))
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);

        }
    }

    //changes type selection and changes image
    public void ChangeTypeSelection()
    {
        string name = MenuTab.GetComponent<MenuTabbing>().getActiveGameObject().transform.name;
        if (name != typeCurrentName)
        {
            typeCurrentName = name;
            //temporary measure so that errors arent thrown for level that doesnt exist yet
            for (int i = 0; i < typeOptions.transform.childCount - 1; i++)
            {

                if (typeOptions.transform.GetChild(i).name == name)
                {
                    typeImages.transform.GetChild(i).gameObject.SetActive(true);
                    typeImages.transform.GetChild(typeIndex).gameObject.SetActive(false);
                    
                    prevLvl = currentLvl;
                    currentLvl = i + 1;
                    player.GetComponent<MovementController>().currentLvl = currentLvl;

                    typeArray[i] = true;
                    typeArray[typeIndex] = false;
                    
                    typeIndex = i;
                    break;
                }

            }
        }
       
    }
    public void openMenu()
    {
        pauseEnabled = true;
    }
    #endregion
}
