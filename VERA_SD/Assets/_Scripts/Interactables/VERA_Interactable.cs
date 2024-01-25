using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VERA_Interactable : MonoBehaviour, IInteractable
{

    // VERA_Interactable provides a base for all interactable objects


    #region VARIABLES


    public List<InteractionInfo> interactions;


    #endregion


    #region IINTERACTABLE FUNCTIONS


    // Gets all interactions this interactable can be used with
    //--------------------------------------//
    public List<string> GetInteractions()
    //--------------------------------------//
    {
        // Build a list of strings consisting of the names of interactables
        List<string> ret = new List<string>();

        foreach (InteractionInfo i in interactions)
        {
            ret.Add(i.interactionName);
        }

        // Return list
        return ret;

    } // END GetInteractions


    // Triggers a given interaction
    //--------------------------------------//
    public void TriggerInteraction(string interaction)
    //--------------------------------------//
    {
        // Find interaction in list by given name
        foreach(InteractionInfo i in interactions)
        {
            // If we find an interaction matching the given name...
            if (i.interactionName == interaction)
            {
                // Trigger interaction and exit function
                i.onTriggerInteraction.Invoke();
                return;
            }
        }

        // If no interaction was found by given name, log an error
        Debug.LogError("VERA_Interactable (" + gameObject.name + "): Attempted to invoke interaction \"" + 
            interaction + "\", but no such interaction was found.");

    } // END TriggerInteraction


    #endregion


    #region ADDITIONAL FUNCTIONALITY


    // Triggers an interaction of this interactable by index (if possible)
    //--------------------------------------//
    public void TriggerInteractionByIndex(int index)
    //--------------------------------------//
    {
        if (index < interactions.Count)
        {
            interactions[index].onTriggerInteraction.Invoke();
        }
        
    } // END TriggerInteractionByIndex


    #endregion


} // END VERA_Interactable.cs


[System.Serializable]
public class InteractionInfo
{

    // InteractionInfo holds information necessary to trigger and display an interaction


    #region VARIABLES


    public string interactionName;
    public UnityEvent onTriggerInteraction;


    #endregion

} // END InteractionInfo.cs
