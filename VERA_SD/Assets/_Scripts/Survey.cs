using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class Survey : MonoBehaviour
{
    public TextAsset surveyData;
    public TextAsset surveyResponse;
    public SurveyQuestionList questionList = new SurveyQuestionList();

    [System.Serializable]
    public class SurveyQuestions
    {
        public string question;
        public string response;
    }

    [System.Serializable]
    public class SurveyQuestionList
    {
        public SurveyQuestions[] questions;
    }
    // public Slider slider;
    // public float surveyValue;

    // {
    //     surveyValue = 1;

   
    // public void GetSurveyValue()
    // {
    //     surveyValue = slider.value;
    //     Debug.Log(surveyValue);
    // }
    // Start is called before the first frame update
    void Start()
    {
        questionList = JsonUtility.FromJson<SurveyQuestionList>(surveyData.text);
        
        for(int i = 0; i < questionList.questions.Length; i++)
        {
            Debug.Log(questionList.questions[i].question);
            Debug.Log(questionList.questions[i].response);
        }

        string userResponse = "2";
        questionList.questions[0].response = userResponse;
        userResponse = "1";
        questionList.questions[1].response = userResponse;
        userResponse = "5";
        questionList.questions[2].response = userResponse;
        userResponse = "3";
        questionList.questions[3].response = userResponse;
        userResponse = "4";
        questionList.questions[4].response = userResponse;
        
        SurveyQuestionList questionResponse = questionList;

        for(int i = 0; i < questionResponse.questions.Length; i++)
        {
            Debug.Log(questionResponse.questions[i].question);
            Debug.Log(questionResponse.questions[i].response);
        }
        SaveResponse(questionResponse);
    }

    public void SaveResponse(SurveyQuestionList responseList){
        string response = JsonUtility.ToJson(responseList);
        // File.WriteAllText(Application.dataPath + "/SurveyResponse.json", response);
        File.WriteAllText(Application.dataPath + "/_Scripts/SurveyResponse.json", response);
        Debug.Log("Called");
    }
}