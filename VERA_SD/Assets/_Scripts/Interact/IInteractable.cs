using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    // IInteractable is an interface which provides general functionality for all interactables


    #region FUNCTIONS


    public List<string> GetInteractions();
    public void TriggerInteraction(string interaction);


    #endregion

} // END IInteractable.cs
