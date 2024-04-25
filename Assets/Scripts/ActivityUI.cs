using UnityEngine;
using TMPro;

public class ActivityUI : MonoBehaviour
{
    private Transform playercamera;
    private Transform basePanel;
    private Transform activePanel;

    // Start is called before the first frame update
    void Start()
    {
        playercamera = transform.parent.Find("Main Camera");
        basePanel = transform.GetChild(0);
        ClearPanels();
        ExperimentController.Instance.SetUIState(this);
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = transform.parent.InverseTransformPoint(playercamera.position) + new Vector3(0,0,1.4f);
        if (activePanel != null)
            if (!activePanel.gameObject.activeSelf)
                ExperimentController.Instance.SetUIState(this);
    }

    private void ClearPanels() {
        foreach (Transform child in basePanel)
            child.gameObject.SetActive(false);
    }

    public UIPanel SwitchPanel(string panelName) {
        Transform panel = basePanel.Find(panelName);
        UIPanel ret = null;

        if (activePanel != null)
            activePanel.gameObject.SetActive(false);

        activePanel = panel;

        if (panel != null)
            ret = panel.GetComponent<UIPanel>();

        if (ret != null)
            ret.Enable(this);

        return ret;
    }

    // used to lock out ui during scene transitions
    public void Disable() {
        basePanel.gameObject.SetActive(false);
    }

    // panel methods called by ExperimentController.SetUIState
    public void QuestionIntro(int envNumber) {
        if (envNumber == 0) {
            SwitchPanel("PracticeQuestionIntro");
        } else {
            var panel = SwitchPanel("QuestionIntro").transform;
            if (envNumber > 1) {
                string[] labels = { "second", "third", "fourth", "fifth", "final" };
                string welcome = "Welcome to the " + labels[envNumber-2] + " testing environment!";
                panel.Find("Welcome").GetComponent<TMP_Text>().text = welcome;
            } else {
                panel.Find("Welcome").GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
