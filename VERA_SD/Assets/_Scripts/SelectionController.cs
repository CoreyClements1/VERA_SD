using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class SelectionController : MonoBehaviour
{
    private List<GameObject> interactables = new List<GameObject>();
    private int counter = 0;
    private GameObject previousObj;
    private Outline outline;
    public string currentObj;

    [SerializeField] float selectRadius;
    [SerializeField] Camera playerCam; // The point where all distance calculations are made (may change later)
    [SerializeField] Color outlineColor;
    [SerializeField] float outlineWidth = 5f;
    [SerializeField] float highlightDuration = 3f;
    [SerializeField] GameObject Arrow;
    [SerializeField] TextMeshPro Text;

    public void SelectionCycle()
    {
        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now
        currentObj = interactables[counter].name;

        if (previousObj != null)
        {
            previousObj.GetComponent<Outline>().enabled = false;
        }

        if (interactables[counter].GetComponent<Outline>() != null)
        {
            outline = interactables[counter].GetComponent<Outline>();
        }
        else
        {
            outline = interactables[counter].AddComponent<Outline>();
        }

        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;

        // Logic for determining target's object position relative to player
        //ObjectInView();

        previousObj = interactables[counter];
        // Loop back around once end of list
        if (counter == interactables.Count - 1)
            counter = 0;
        else counter++;
    }

    bool UpdateSelectables()
    {
        // Honestly a suspicious way of doing this: Get all objs in radius > Loop through and find all objs with IInteractable
        interactables.Clear(); // Clear the list first to avoid dupes
        Collider[] colliders = Physics.OverlapSphere(playerCam.transform.position, selectRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<IInteractable>() != null)
                interactables.Add(collider.gameObject);
        }
        //Debug.Log("I see " + interactables.Count);
        return (interactables.Count > 0) ? true : false;
    }

    void ObjectInView()
    {
        Vector3 target = interactables[counter].transform.position;

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
                Arrow.transform.LookAt(targetPosition);
                Text.text = "OFF SCREEN";
                Text.color = Color.red;

            }
            else
            {
                Arrow.SetActive(true);
                Arrow.transform.LookAt(targetPosition);
                Text.text = "ON SCREEN";
                Text.color = Color.green;
            }
        }
    }

    public void HighlightAll()
    {
        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now
        StartCoroutine(highlight());
    }

    IEnumerator highlight()
    {
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
        foreach (GameObject obj in interactables)
        {
            obj.GetComponent<Outline>().enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // This assumes that the radius is drawn from player's camera, may not be true later!
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerCam.transform.position, selectRadius);
    }

    public List<GameObject> grabInteractables()
    {
        UpdateSelectables();
        return interactables;
    }
}
