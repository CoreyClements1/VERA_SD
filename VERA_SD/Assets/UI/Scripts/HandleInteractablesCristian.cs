using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class HandleInteractablesCristian : MonoBehaviour
{
    [SerializeField] SelectionController selectionController;
    public GameObject InteractablesGroup; 
    private List<GameObject> interactables;

    public GameObject InteractionMainMenu;
    
    // For Instantiating UI 
    public GameObject original;
    public GameObject parent;
    Button selectBttn;

    void Awake(){
        Debug.Log("Currently rocking with + " + parent.name);
        // Setup list of interactables

        /*        interactables = new GameObject[InteractablesGroup.transform.childCount];
                for( int i = 0; i < interactables.Length; i++){
                    interactables[i] = InteractablesGroup.transform.GetChild(i).gameObject;
                }
                for(int i = 0; i< interactables.Length; i++){
                    List <GameObject> levels = SetupLevels(interactables[i]);
                    SetupButtons(levels, interactables[i]);

        }*/
        interactables = selectionController.grabInteractables();
        foreach (GameObject interactable in interactables)
        {
            List<GameObject> levels = SetupLevels(interactable);
            SetupButtons(levels, interactable);
        }

        selectBttn = InteractionMainMenu.transform.Find("Select").GetComponent<Button>();
        //selectBttn.onClick.AddListener(() =>nextLevel(InteractionMainMenu, parent.transform.Find("TestInteractable1").gameObject ));
        

    }

    List <GameObject> SetupLevels(GameObject Interactable){
        VERA_Interactable Interact = Interactable.GetComponent<VERA_Interactable>();
        List<string> PossibleInteractions = Interactable.GetComponent<VERA_Interactable>().GetInteractions();
        int Size = PossibleInteractions.Count;
        int NumLevels = Size/2;
        List <GameObject> Levels = new List<GameObject>();
        for(int i = 0; i < NumLevels; i++){
            GameObject obj = Instantiate(original, parent.GetComponent<Transform>());
            obj.name = Interactable.name + (i+1).ToString();
            Levels.Add(obj);

        }
        return Levels;
    }

    void SetupButtons(List <GameObject> levels, GameObject Interactable){
         List<string> InteractInfo = Interactable.GetComponent<VERA_Interactable>().GetInteractions();
         VERA_Interactable accessInteraction = Interactable.GetComponent<VERA_Interactable>();
         int Size = InteractInfo.Count;
         int numLevels = levels.Count;
         int counter = 0;
         //Debug.Log("Interact Info: "+ InteractInfo.Count.ToString());
         // Loop through every level and setup buttons 1 2 and 3
         for(int i = 0; i < numLevels; i++){
            GameObject current = levels[i];
            if(i < (numLevels-1)){
                GameObject next = levels[i+1];
            }
            if(i > 0){
                GameObject prev = levels[i-1];
            }
            // setup all 3 buttons on that level 
            for( int j = 0; j < 3; j++){
                GameObject obj = levels[i].transform.Find((j+1).ToString()).gameObject;
                Button btn = obj.GetComponent<Button>();
                TextMeshProUGUI txt = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                // Levels that need a more options button
                if(i < (numLevels-1)){
                    // interaction buttons
                     GameObject next = levels[i+1];
                    if(j < 2){
                        txt.text = InteractInfo[counter];
                        btn.onClick.AddListener(()=>  accessInteraction.TriggerInteraction(txt.text));
                        counter++;
                    }
                    // More options button
                    else{
                        txt.text = "More Actions";
                        btn.onClick.AddListener(()=>nextLevel(current, next));
                    }
                }
                // Final Level
                else{
                    txt.text = InteractInfo[counter];
                    btn.onClick.AddListener(()=>  accessInteraction.TriggerInteraction(txt.text));
                    counter++;
                }

            }

             GameObject backObj = levels[i].transform.Find("Back Button (Button 4)").gameObject;
            Button backBtn = backObj.GetComponent<Button>();
            if(i > 0){
                GameObject prev = levels[i-1];
                backBtn.onClick.AddListener(()=> back(prev, current));

            }
            else{
                backBtn.onClick.AddListener(()=> back(InteractionMainMenu, current));

            }
         }
         
    }


    void runInteration(string interaction, VERA_Interactable interactor){
        interactor.TriggerInteraction(interaction);
        
    }

    void back(GameObject previous, GameObject current){
        current.SetActive(false);
        previous.SetActive(true);
    }

    void nextLevel(GameObject current, GameObject  next){
        current.SetActive(false);
        next.SetActive(true);

    }

    public void ChangeSelection()
    {
        string tmp = "" + selectionController.getSelection() + "1";
        Debug.Log(tmp);
        selectBttn.onClick.RemoveAllListeners();
        selectBttn.onClick.AddListener(() => nextLevel(InteractionMainMenu, parent.transform.Find(tmp).gameObject));
    }


}


// Setup list of interactables
// get number of possible interactions
// make gameobject with floor(n/2) levels 