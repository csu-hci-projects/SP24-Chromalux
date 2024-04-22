using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : HoldButton
{
    PracticeQuestionController.COLOR color;
    PracticeQuestionController controller;
    public Image bg;

    protected override void Awake() {
        controller = transform.parent.GetComponent<PracticeQuestionController>();
        color = PracticeQuestionController.colorID(transform.name);
        bg = transform.GetComponent<Image>();
        Debug.Assert(controller != null);

        base.Awake();
        onClick.RemoveAllListeners();
        onClick.AddListener(() => { controller.SubmitAnswer(color); });
    }

    public void Init() {
        InstantClearState();
    }
}
