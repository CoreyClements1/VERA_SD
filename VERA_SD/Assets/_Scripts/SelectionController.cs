using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class SelectionController : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    private Material originalMaterial;
    private List<GameObject> interactables = new List<GameObject>();
    private int counter = 0;
    private string currentObj;

    [SerializeField] GameObject Arrow;
    [SerializeField] Camera playerCam; // The point where all distance calculations are made (may change later)
    [SerializeField] TextMeshPro Text;
    [SerializeField] float selectRadius;

    public void SelectionCycle()
    {
        if (!UpdateSelectables()) return; // If there is nothing to select, skip for now

        if (originalMaterial != null) // We need to revert the previous object's material
        {
            // Avoid going out of bounds, the item before [index 0] is [index last]
            if (counter == 0)
                interactables[interactables.Count - 1].GetComponent<Renderer>().material = originalMaterial;
            else
                interactables[counter - 1].GetComponent<Renderer>().material = originalMaterial;
        }

        // Save material and highlight it
        Renderer renderer = interactables[counter].GetComponent<Renderer>();
        originalMaterial = renderer.material;
        renderer.material = highlightMaterial;
        currentObj = interactables[counter].name;

        // Logic for determining target's object position relative to player
        //ObjectInView();

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

    public string getSelection()
    {
        return currentObj;
    }
}
