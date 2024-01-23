using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntiallyInactive : MonoBehaviour
{
    // private name = gameObject.name;
    // GameObject current = ;
    void Start()
    {
        gameObject.SetActive(false);
    }

}
