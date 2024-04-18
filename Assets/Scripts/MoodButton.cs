using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class MoodButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int state { get; private set; }
    SurveyFieldController surveyFieldController;
    private bool hover = false;
    private bool down = false;

    protected override void Awake()
    {
        state = Int32.Parse(transform.GetChild(0).GetComponent<TMP_Text>().text);
        surveyFieldController = transform.parent.parent.GetComponent<SurveyFieldController>();
    }

    public void Init() {
        down = false;
        hover = false;
        DoStateTransition(SelectionState.Normal, true);
    }

    protected new SelectionState currentSelectionState { get { return VisualState(); } }

    private SelectionState VisualState() {
        if (!down) {
            if (hover) return SelectionState.Highlighted;
            else return SelectionState.Normal;
        } else {
            return SelectionState.Pressed;
        }
    }

    private void SetVisualState() {
        DoStateTransition(VisualState(), false);
    }

    public void SetState(bool down) {
        this.down = down;
        SetVisualState();
    }
    public override void OnPointerDown(PointerEventData eventData) { } // delete
    public override void OnPointerUp(PointerEventData eventData) { } // delete
    public override void OnPointerClick(PointerEventData eventData) {
        if (!interactable) return;
        surveyFieldController.SetState(this);
    }
    public override void OnPointerEnter(PointerEventData eventData) {
        if (!interactable) return;
        hover = true;
        SetVisualState();
    }
    public override void OnPointerExit(PointerEventData eventData) {
        if (!interactable) return;
        hover = false;
        SetVisualState();
    }
}
