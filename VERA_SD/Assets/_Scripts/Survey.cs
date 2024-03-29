using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Survey : MonoBehaviour
{
    public Slider slider;
    public float surveyValue;
    // Start is called before the first frame update
    void Start()
    {
        surveyValue = 1;
    }

    void Update()
    {

    }
    public void GetSurveyValue()
    {
        surveyValue = slider.value;
        Debug.Log(surveyValue);
    }

    
   
}
