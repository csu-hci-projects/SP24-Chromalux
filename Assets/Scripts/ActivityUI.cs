using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityUI : MonoBehaviour
{
    private Transform playercamera;
    private Transform basePanel;
    private Button StartButton;
    private TMP_Text StartButtonText;

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

    private void Clear() {
        foreach (Transform child in basePanel)
            child.gameObject.SetActive(false);
    }

    public void Tutorial() {
        Clear();
        Transform tutorial1 = basePanel.Find("Tutorial1");
        StartButton = tutorial1.Find("StartButton").GetComponent<Button>();
        StartButtonText = StartButton.gameObject.GetComponentInChildren<TMP_Text>();
        TMP_Text TutText = tutorial1.Find("Text").GetComponent<TMP_Text>();

        tutorial1.gameObject.SetActive(true);

        TutNotReady();
    }

    public void TutNotReady() {
        StartButton.interactable = false;
        StartButtonText.text = "Please Wait...";
    }
    public void TutReadyStart() {
        StartButton.interactable = true;
        StartButtonText.text = "Begin Experiment";
    }
}
