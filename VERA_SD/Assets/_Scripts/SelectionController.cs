using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour
{

    #region VARIABLES


    private List<GameObject> interactables = new List<GameObject>();
    private int counter = 0;
    [SerializeField] HandleInteractables treeBase;
    private GameObject previousObj;
    private Outline outline;
    private GameObject lookTarget;
    public string currentObj;
    private bool manualHighlightCancel = false;

    [SerializeField] float selectRadius;
    [SerializeField] Camera playerCam; // The point where all distance calculations are made (may change later)
    [SerializeField] Color outlineColor;
    [SerializeField] float outlineWidth = 5f;
    [SerializeField] float highlightDuration = 3f;
    [SerializeField] GameObject Arrow;
    [SerializeField] TextMeshPro Text;

    private GameObject interactSub;


    #endregion


    #region MONOBEHAVIOUR
    void Awake()
    {
        interactSub = GameObject.Find("Interact Sub");
        Debug.Log(interactSub);

    }


    // Update
    //--------------------------------------//
    private void Update()
    //--------------------------------------//
    {
        if (lookTarget != null)
        {
            Arrow.transform.LookAt(lookTarget.transform);
        }
        SelectedInRange();
    } // END Update


    #endregion


    #region SELECTION
    public void SelectedInRange()
    {
        UpdateSelectables();
        if (previousObj != null && !interactables.Contains(previousObj))
        {
            // Previous current object is out of range, deselect it
            previousObj.GetComponent<Outline>().enabled = false;
            previousObj = null;
            lookTarget = null;
            treeBase.RemoveListeners();
            treeBase.back(interactSub, GameObject.Find(currentObj + "1"));
            currentObj = null;
        }

    }

    // SelectionCycle
    //--------------------------------------//
    public void SelectionCycle()
    //--------------------------------------//
    {
        // Cancel highlighting if all highlighted
        PulseCancelHighlights();

        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now

        if (counter >= interactables.Count) counter = 0;
        currentObj = interactables[counter].name;

        // De-highlight previous object
        if (previousObj != null)
        {
            previousObj.GetComponent<Outline>().enabled = false;
        }

        // Get current object and get / add outline component
        if (interactables[counter].GetComponent<Outline>() != null)
        {
            outline = interactables[counter].GetComponent<Outline>();
        }
        else
        {
            outline = interactables[counter].AddComponent<Outline>();
        }

        // Enable outline
        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;

        // Logic for determining target's object position relative to player
        ObjectInView();

        previousObj = interactables[counter];
        // Loop back around once end of list
        if (counter == interactables.Count - 1)
            counter = 0;
        else counter++;

    } // END SelectionCycle


    // Updates highlightable selectables; returns true if there are selectables, false if there are none
    //--------------------------------------//
    bool UpdateSelectables()
    //--------------------------------------//
    {
        // Honestly a suspicious way of doing this: Get all objs in radius > Loop through and find all objs with IInteractable
        interactables.Clear(); // Clear the list first to avoid dupes

        // Get all colliders nearby
        Collider[] colliders = Physics.OverlapSphere(playerCam.transform.position, selectRadius);

        // On each collider, check if there is an interactable component, add to interactables list if yes
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<IInteractable>() != null)
                interactables.Add(collider.gameObject);
        }

        // Return whether there are interactables nearby or not
        // Debug.Log("I see " + interactables.Count);
        return (interactables.Count > 0) ? true : false;

    } // END UpdateSelectables

    // Grabs all the selectables in the scene, is used to create associating buttons in tree UI
    //--------------------------------------//
    public List<GameObject> grabAllSelectables()
    //--------------------------------------//
    {
        // Honestly a suspicious way of doing this, not to mention expensive, do it once please
        List<GameObject> interactables = new List<GameObject>();

        // Get all GameObjects nearby
        //Collider[] colliders = Physics.OverlapSphere(playerCam.transform.position, selectRadius);
        GameObject[] objects = FindObjectsOfType<GameObject>();

        // On each object, check if there is an interactable component, add to interactables list if yes
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<IInteractable>() != null)
                interactables.Add(obj);
        }

        return interactables;
    } // END grabAllSelectables

    #endregion


    #region OBJECT IN VIEW


    void ObjectInView()
    {
        lookTarget = interactables[counter];
        Vector3 target = lookTarget.transform.position;

        // Normally this would have the player's position relative to camera but not yet!
        Vector3 playerScreenPos = playerCam.WorldToScreenPoint(playerCam.transform.position);
        Vector3 targetScreenPos = playerCam.WorldToScreenPoint(target);

        // Grab the vector to the target
        Vector3 targetPosition = new Vector3(target.x, target.y, target.z);

        // 10 hardcoded, goofy code
        bool isOffScreen = targetScreenPos.x <= 10 || targetScreenPos.x >= Screen.width || targetScreenPos.y <= 10
                            || targetScreenPos.y >= Screen.height;

        if (Arrow != null || Text != null) // REFACTOR
        {
            if (isOffScreen)
            {
                Arrow.SetActive(true);
                //Arrow.transform.LookAt(targetPosition);
                Text.text = "OFF SCREEN";
                Text.color = Color.red;

            }
            else
            {
                Arrow.SetActive(true);
                //Arrow.transform.LookAt(targetPosition);
                Text.text = "ON SCREEN";
                Text.color = Color.green;
            }
        }
    }


    #endregion


    #region HIGHLIGHTING


    // Highlights all nearby selectables
    //--------------------------------------//
    public void HighlightAll()
    //--------------------------------------//
    {
        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now
        StartCoroutine(highlight());

    } // END HighlightAll


    // Coroutine to highlight all interactables
    //--------------------------------------//
    IEnumerator highlight()
    //--------------------------------------//
    {
        manualHighlightCancel = false;

        foreach (GameObject obj in interactables)
        {
            if (obj.GetComponent<Outline>() != null)
            {
                outline = obj.GetComponent<Outline>();
                outline.enabled = true;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = outlineWidth;
            }
            else
            {
                outline = obj.AddComponent<Outline>();
                outline.enabled = true;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = outlineWidth;
            }
        }
        yield return new WaitForSeconds(highlightDuration);

        // If we manually cancelled highlighting during coroutine, don't cancel highlighting again
        if (!manualHighlightCancel)
        {
            foreach (GameObject obj in interactables)
            {
                obj.GetComponent<Outline>().enabled = false;
            }
        }

    } // END highlight


    // Pulse cancels all highlighted components
    //--------------------------------------//
    public void PulseCancelHighlights()
    //--------------------------------------//
    {
        manualHighlightCancel = true;

        foreach (GameObject obj in interactables)
        {
            if (obj.GetComponent<Outline>() != null)
            {
                outline = obj.GetComponent<Outline>();
                outline.enabled = false;
            }
        }

    } // END PulseCancelHighlights


    #endregion


    #region OTHER


    // OnDrawGizmosSelected
    //--------------------------------------//
    private void OnDrawGizmosSelected()
    //--------------------------------------//
    {
        // This assumes that the radius is drawn from player's camera, may not be true later!
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerCam.transform.position, selectRadius);

    } // END OnDrawGizmosSelected

    #endregion


} // END SelectionController.cs
