using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Jobs;

public class SurveyFieldController : MonoBehaviour
{
    string mood;
    int state;
    MoodButton[] moodButtons;
    SurveyController surveyController;

    void Awake() {
        surveyController = transform.parent.parent.GetComponent<SurveyController>();
        mood = transform.Find("Label").GetComponent<TMP_Text>().text;
        Transform buttons = transform.Find("Buttons");
        moodButtons = new MoodButton[buttons.childCount];
        Debug.Assert(moodButtons.Length == 5);
        int i = 0;
        foreach (Transform button in buttons)
            moodButtons[i++] = button.GetComponent<MoodButton>();
    }

    public void Init() {
        state = -1;
        foreach (var button in moodButtons)
            button.Init();
    }

    public void SetState(MoodButton pressed) {
        if (state == -1) surveyController.incrementButtonStateCount();
        foreach (MoodButton moodButton in moodButtons)
            if (moodButton == pressed) moodButton.SetState(true);
            else moodButton.SetState(false);
        state = pressed.state;
    }

    public (string, int) GetState() {
        return (mood, state);
    }
}
