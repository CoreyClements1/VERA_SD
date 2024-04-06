using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class Survey : MonoBehaviour
{
    public Slider slider;
    public TextAsset surveyData;
    public TextAsset surveyResponse;
    public TextMeshProUGUI surveyQuestionDisplay;
    public TextMeshProUGUI surveyResponseDisplay;
    public Button submitButton;
    private int questionCounter = 0;
    public SurveyQuestionList questionList = new SurveyQuestionList();
    public string surveyValue;

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


    void Start()
    {
        questionList = JsonUtility.FromJson<SurveyQuestionList>(surveyData.text);

        if(questionList.questions.Length != 0)
        {
            surveyQuestionDisplay.text = questionList.questions[questionCounter].question;
        }
        for(int i = 0; i < questionList.questions.Length; i++)
        {
            Debug.Log(questionList.questions[i].question);
            Debug.Log(questionList.questions[i].response);
        }

        slider.onValueChanged.AddListener((v) => {
            switch(slider.value)
            {
                case 1f:
                    surveyResponseDisplay.text = "Strongly Disagree";
                    break;
                case 2f:
                    surveyResponseDisplay.text = "Disagree";
                    break;
                case 3f:
                    surveyResponseDisplay.text = "Slightly Disagree";
                    break;
                case 4f:
                    surveyResponseDisplay.text = "Neutral";
                    break;
                case 5f:
                    surveyResponseDisplay.text = "Slightly Agree";
                    break;
                case 6f:
                    surveyResponseDisplay.text = "Agree";
                    break;
                case 7f:
                    surveyResponseDisplay.text = "Strongly Agree";
                    break;
            }
            surveyValue = surveyResponseDisplay.text;
            Debug.Log(surveyValue);
        });

        submitButton.onClick.AddListener(() => {
            SubmitSurveyQuestion();
        });
    }

    public void SaveResponse(SurveyQuestionList responseList){
        string response = JsonUtility.ToJson(responseList);
        // File.WriteAllText(Application.dataPath + "/SurveyResponse.json", response);
        File.WriteAllText(Application.dataPath + "/_Scripts/SurveyResponse.json", response);
        Debug.Log("Called");
    }
       
    public void SubmitSurveyQuestion()
    {
        //
        if (questionCounter != questionList.questions.Length-1)
        {
            questionList.questions[questionCounter].response = surveyValue;
            questionCounter++;
            surveyQuestionDisplay.text = questionList.questions[questionCounter].question;
        } 
        else {
            questionList.questions[questionCounter].response = surveyValue;
            surveyQuestionDisplay.text = "Your responses have been recorded";
            SaveResponse(questionList);
        }
    }
}