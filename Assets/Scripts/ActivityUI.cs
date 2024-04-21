using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityUI : MonoBehaviour
{
    private Transform playercamera;
    private Transform basePanel;

    // Start is called before the first frame update
    void Start()
    {
        playercamera = transform.parent.Find("Main Camera");
        basePanel = transform.GetChild(0);
        ExperimentController.Instance.SetUIState(this);
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = transform.parent.InverseTransformPoint(playercamera.position) + new Vector3(0,0,1.3f);
    }

    private void ClearPanels() {
        foreach (Transform child in basePanel)
            child.gameObject.SetActive(false);
    }

    public Transform SwitchPanel(string panelName) {
        Transform panel = basePanel.Find(panelName);

        ClearPanels();
        if (panel != null) panel.gameObject.SetActive(true);

        return panel;
    }

    private Button tut_StartButton;
    private TMP_Text tut_StartButtonText;
    private bool tut_startButtonSet;
    public void Tutorial() {
        tut_startButtonSet = false;
        SwitchPanel("Tutorial1");
    }
    public void Tutorial2() {
        Transform panel = SwitchPanel("Tutorial2");
    }
    public void Tutorial3() {
        Transform panel = SwitchPanel("Tutorial3");
        tut_StartButton = panel.Find("StartButton").GetComponent<Button>();
        tut_StartButtonText = tut_StartButton.gameObject.GetComponentInChildren<TMP_Text>();
        if (tut_startButtonSet) TutReadyStart();
    }
    public void TutNotReady() {
        tut_StartButton.interactable = false;
        tut_StartButtonText.text = "Please Wait...";
    }
    public void TutReadyStart() {
        if (tut_StartButton == null) tut_startButtonSet = true;
        else {
            tut_StartButton.interactable = true;
            tut_StartButtonText.text = "Begin Experiment";
        }
    }
    public void Survey() {
        Transform panel = SwitchPanel("Survey");
    }
    public void Question() {
        SwitchPanel("Question");
    }
}
