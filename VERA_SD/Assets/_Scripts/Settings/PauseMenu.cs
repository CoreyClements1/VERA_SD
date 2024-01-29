using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour
{
    private bool pauseEnabled;
    public GameObject pauseMenu;
    public GameObject settings;
    public GameObject snapTurningImages;
    [SerializeField] private GameObject player;
    private int index;
    private string currentName;
    // Start is called before the first frame update
    void Start()
    {
        pauseEnabled = false;
        Debug.Log("testing function");
        currentName = "free";
        index = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseEnabled = !pauseEnabled;
            pauseMenu.gameObject.SetActive(pauseEnabled);
            if(pauseEnabled)
                Time.timeScale = 0;
            if (!pauseEnabled)
            {
                Time.timeScale = 1;
                for (int i = 0; i < settings.transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.gameObject.SetActive(false);
                    

                }
            }
            
            
        }
    }

    public void snapTurning()
    {
        
        for(int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name != "Snap Turning")
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);
           
        }
    }

    public void changeTurning()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        if(name != currentName)
        {
            currentName = name;
            if (name == "90")
            {
                snapTurningImages.transform.GetChild(0).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 90f;
                index = 0;             
            }
            else if (name == "45")
            {
                snapTurningImages.transform.GetChild(1).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 45f;
                index = 1;
            }
            else if (name == "free")
            {
                snapTurningImages.transform.GetChild(2).gameObject.SetActive(true);
                snapTurningImages.transform.GetChild(index).gameObject.SetActive(false);
                player.GetComponent<MovementController>().rotationValue = 15f;
                index = 2;
            }
        }
        
    }
    public void movementSpeed()
    {
        for (int i = 0; i < settings.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name != "Movement Speed")
                child.gameObject.SetActive(false);
            else
                child.gameObject.SetActive(true);

        }
    }
}
