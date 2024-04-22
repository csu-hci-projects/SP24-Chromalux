using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

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

    public void Tutorial() {
        SwitchPanel("Tutorial1");
    }
    public void Tutorial2() {
        SwitchPanel("Tutorial2");
    }
    public void Tutorial3() {
        SwitchPanel("Tutorial3");
    }
    public void Survey() {
        SwitchPanel("Survey");
    }
    public void QuestionIntro(int envNumber) {
        if (envNumber == 0) {
            SwitchPanel("FirstQuestionIntro");
        } else {
            var panel = SwitchPanel("QuestionIntro");
            if (envNumber > 0) {
                string[] labels = { "second", "third", "fourth", "fifth", "sixth" };
                string welcome = "Welcome to the " + labels[envNumber-1] + " testing environment!";
                panel.Find("Welcome").GetComponent<TMP_Text>().text = welcome;
            } else {
                panel.Find("Welcome").GetComponent<TMP_Text>().text = "";
            }
        }
    }
    public void PracticeQuestion() {
        var panel = SwitchPanel("PracticeQuestion");
        panel.GetComponent<PracticeQuestionController>().Init();
    }
}
