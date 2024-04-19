using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyController : MonoBehaviour
{
    SurveyFieldController[] surveyFields;
    HoldButton submitButton;
    int initializedButtons = 0;

    void Start()
    {
        submitButton = transform.Find("SubmitButton").GetComponent<HoldButton>();

        Transform Fields = transform.Find("Fields");
        surveyFields = new SurveyFieldController[Fields.childCount];
        int i = 0;
        foreach (Transform field in Fields)
            surveyFields[i++] = field.GetComponent<SurveyFieldController>();

        Init();
    }

    public void Init() {
        submitButton.interactable = false;
        foreach (SurveyFieldController field in surveyFields)
            field.Init();
    }

    public void IncrementButtonStateCount() {
        ++initializedButtons;
        if (initializedButtons == surveyFields.Length) {
            submitButton.interactable = true;
        }
    }

    public void PublishState() {
        if(surveyFields == null || surveyFields.Length == 0) {
            Debug.LogWarning("Survey fields empty or null!");
            return;
        }

        (string, int)[] states = new (string, int)[surveyFields.Length];
        int i = 0;
        foreach (SurveyFieldController field in surveyFields)
            states[i++] = field.GetState();

        foreach (var state in states) // Survey choices printed here
            
            Debug.Log(state);
        
        ExperimentController.RecordSurveyResponses(states); // Record survey responses to file using this
        ExperimentController.Instance.PublishSurvey(states);

        ActivityUI activityUI = transform.parent.parent.GetComponent<ActivityUI>();
        ExperimentController.Instance.SetUIState(activityUI);
    }
}
