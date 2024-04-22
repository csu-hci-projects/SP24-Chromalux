using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurveyController : MonoBehaviour
{
    SurveyFieldController[] surveyFields;
    HoldButton submitButton;
    TMP_Text submitButtonText;
    int initializedButtons = 0;
    bool readyProcedureComplete;

    void Start()
    {
        submitButton = transform.Find("SubmitButton").GetComponent<HoldButton>();
        submitButtonText = submitButton.transform.Find("Text").GetComponent<TMP_Text>();

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
        readyProcedureComplete = false;
        UpdateSubmitButtonState();
    }

    // handle setting the button interactable if the setup completes while survey is open
    public void Update() {
        if (!readyProcedureComplete && ExperimentController.Instance.setupComplete) {
            UpdateSubmitButtonState();
        }
    }

    public void UpdateSubmitButtonState() {
        submitButtonText.text = "Submit";
        if (initializedButtons >= surveyFields.Length) {
            if (ExperimentController.Instance.setupComplete == true) {
                submitButton.interactable = true;
                readyProcedureComplete = true;
            } else {
                submitButtonText.text = "Please Wait...";
                submitButton.interactable = false;
            }
        } else {
            submitButton.interactable = false;
        }
    }

    public void IncrementButtonStateCount() {
        ++initializedButtons;
        UpdateSubmitButtonState();
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

        ExperimentController.Instance.FinishSurvey(states);

        ActivityUI activityUI = transform.parent.parent.GetComponent<ActivityUI>();
        ExperimentController.Instance.SetUIState(activityUI);
    }
}
