using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;


public class SelectionController : MonoBehaviour
{

    // SelectionController controls the selection of nearby interactable objects


    #region VARIABLES


    private List<VERA_Interactable> interactables = new List<VERA_Interactable>();
    private int counter = 0;
    private VERA_Interactable previousObj;
    private Outline outline;
    private GameObject lookTarget;
    [System.NonSerialized] public VERA_Interactable currentObj;
    private bool manualHighlightCancel = false;
    private GrabTracker grabTracker;

    [SerializeField] float selectRadius;
    private Camera playerCam; // The point where all distance calculations are made (may change later)
    [SerializeField] Color outlineColor;
    [SerializeField] float outlineWidth = 5f;
    [SerializeField] float highlightDuration = 3f;
    private Arrow arrow;
    private TextMeshPro Text;
    [SerializeField] bool useCameraSelect = false;

    private GameObject interactSub;


    #endregion


    #region MONOBEHAVIOUR


    // Awake
    //--------------------------------------//
    void Awake()
    //--------------------------------------//
    {
        interactSub = GameObject.Find("Interact Sub");
        playerCam = Camera.main;
        arrow = FindObjectOfType<Arrow>();

    } // END Awake


    // Start
    //--------------------------------------//
    private void Start()
    //--------------------------------------//
    {
        grabTracker = FindObjectOfType<GrabTracker>();
        if (grabTracker == null)
        {
            // Debug.LogError("GrabTracker is not assigned or not found.");
        }
        else
        {
            // Debug.Log("GrabTracker found.");
        }

    } // END Start

    // Update
    //--------------------------------------//
    private void Update()
    //--------------------------------------//
    {
        if (lookTarget != null && arrow != null)
        {
            arrow.transform.LookAt(lookTarget.transform);
        }
        SelectedOutOfRange();

    } // END Update


    #endregion


    #region SELECTION


    // SelectedOutOfRange
    // Ensures any object that somehow gets out of range is made non-selectable and deselects in necessary
    //--------------------------------------//
    public void SelectedOutOfRange()
    //--------------------------------------//
    {
        if (currentObj != null)
        {
            float outside = Vector3.Distance(playerCam.transform.position, currentObj.transform.position);
            if (outside > selectRadius)
            {
                currentObj.GetComponent<Outline>().enabled = false;
                currentObj = null;
                lookTarget = null;
                FindObjectOfType<NewMenuNavigation>().InteractOutOfRange();
            }
        }

    } // END SelectedOutOfRange


    // SelectionCycle
    // Probably good practice to separate the functions that happened to all cram inside this
    //--------------------------------------//
    public void SelectionCycle()
    //--------------------------------------//
    {
        // Cancel highlighting if all highlighted
        PulseCancelHighlights();

        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now

        if (counter >= interactables.Count) counter = 0;
        currentObj = interactables[counter];

        // De-highlight previous object
        if (previousObj != null)
        {
            previousObj.gameObject.GetComponent<Outline>().enabled = false;
        }

        // Get current object and get / add outline component
        if (interactables[counter].GetComponent<Outline>() != null)
        {
            outline = interactables[counter].GetComponent<Outline>();
        }
        else
        {
            outline = interactables[counter].gameObject.AddComponent<Outline>();
        }

        // Enable outline
        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;

        // Disable arrow if object is grabbed
        if (arrow != null)
        {
            if (grabTracker.GetGrabbedObject() == interactables[counter])
                arrow.gameObject.SetActive(false);
            else
                arrow.gameObject.SetActive(true);
        }

        // If camera select is enabled, snap view to the object
        if (useCameraSelect && (grabTracker.GetGrabbedObject() != interactables[counter]))
            CameraSnap(interactables[counter].gameObject);

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
            {
                // RaycastHit hit, lets not grab objects behind walls;
                Vector3 directionToInteractable = collider.gameObject.transform.position - playerCam.transform.position;
                Collider[] cameraColliders = playerCam.GetComponentsInChildren<Collider>();
                Collider[] objChildColliders = collider.gameObject.GetComponentsInChildren<Collider>();
                Collider[] combinedColliders = cameraColliders.Concat(objChildColliders).ToArray();
                RaycastHit[] hits = Physics.RaycastAll(playerCam.transform.position, directionToInteractable, Vector3.Distance(playerCam.transform.position, collider.gameObject.transform.position));
                if (hits.Length == 1)
                {
                    if (hits[0].collider == collider)
                    {
                        interactables.Add(collider.gameObject.GetComponent<VERA_Interactable>());
                    }
                }
                else
                {
                    System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                    foreach (RaycastHit hitCollider in hits)
                    {
                        //initializing value to false each iteration
                        bool isIn = false;
                        foreach (Collider c in combinedColliders)
                        {
                            //if the raycast object collider is one of the children colliders break loop no need to check anymore
                            if (hitCollider.collider == c)
                            {
                                isIn = true;
                                break;
                            }
                        }
                        if (isIn == false)
                        {
                            continue;
                        }
                        else if (hitCollider.collider != collider)
                        {
                            continue;
                        }
                        else
                        {
                            interactables.Add(collider.gameObject.GetComponent<VERA_Interactable>());
                        }
                    }
                }
                //if is child of camera or child of object
            }
        }
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


    // ObjectInView
    // Unused currently, but has potential
    //--------------------------------------//
    void ObjectInView()
    //--------------------------------------//
    {
        lookTarget = interactables[counter].gameObject;
        Vector3 target = lookTarget.transform.position;

        // Normally this would have the player's position relative to camera but not yet!
        Vector3 playerScreenPos = playerCam.WorldToScreenPoint(playerCam.transform.position);
        Vector3 targetScreenPos = playerCam.WorldToScreenPoint(target);

        // Grab the vector to the target
        Vector3 targetPosition = new Vector3(target.x, target.y, target.z);

        // 10 hardcoded based on the size of the player view, change this later
        bool isOffScreen = targetScreenPos.x <= 10 || targetScreenPos.x >= Screen.width || targetScreenPos.y <= 10
                            || targetScreenPos.y >= Screen.height;

        if (arrow != null || Text != null) // REFACTOR
        {
            if (isOffScreen)
            {
                //Text.text = "OFF SCREEN";
                //Text.color = Color.red;

            }
            else
            {
                //Text.text = "ON SCREEN";
                //Text.color = Color.green;
            }
        }

    } // END ObjectInView


    // CameraSnap
    //--------------------------------------//
    void CameraSnap(GameObject target)
    //--------------------------------------//
    {
        playerCam.transform.parent.LookAt(target.transform);

    } // END CameraSnap


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

        foreach (VERA_Interactable inter in interactables)
        {
            GameObject obj = inter.gameObject;
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
            foreach (VERA_Interactable inter in interactables)
            {
                inter.gameObject.GetComponent<Outline>().enabled = false;
            }
        }

    } // END highlight


    // Pulse cancels all highlighted components
    //--------------------------------------//
    public void PulseCancelHighlights()
    //--------------------------------------//
    {
        manualHighlightCancel = true;

        foreach (VERA_Interactable inter in interactables)
        {
            GameObject obj = inter.gameObject;
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
        if (playerCam != null)
        {
            Gizmos.DrawWireSphere(playerCam.transform.position, selectRadius);
        }

    } // END OnDrawGizmosSelected


    #endregion


} // END SelectionController.cs
