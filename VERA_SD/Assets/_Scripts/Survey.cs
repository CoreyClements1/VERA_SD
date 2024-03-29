using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Survey : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void GetSurveyValue()
    {
        Debug.Log(slider.value);
    }
   
}
