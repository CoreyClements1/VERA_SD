using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool pauseEnabled;
    public GameObject pauseMenu;
    public GameObject settings;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseEnabled = false;
        Debug.Log("testing function");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseEnabled = !pauseEnabled;
            pauseMenu.gameObject.SetActive(pauseEnabled);

            if (!pauseEnabled)
            {
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
