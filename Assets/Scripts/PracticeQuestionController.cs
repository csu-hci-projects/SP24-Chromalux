using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PracticeQuestionController : UIPanel
{
    public enum COLOR {
        RED,
        BROWN,
        GREEN,
        BLUE,
        PURPLE
    }
    protected static Color[] colors = {
        new Color32(0xff, 0x00, 0x00, 0xff),
        new Color32(0xaa, 0x55, 0x00, 0xff),
        new Color32(0x00, 0x99, 0x00, 0xff),
        new Color32(0x00, 0x80, 0xff, 0xff),
        new Color32(0xa3, 0x49, 0xa4, 0xff),
    };
    protected static string[] colorNames = {
        "Red",
        "Brown",
        "Green",
        "Blue",
        "Purple"
    };

    protected ColorButton[] buttons = new ColorButton[5];
    protected TMP_Text question;
    protected HoldButton continueButton;

    protected int questionNumber = 0;

    private (COLOR, COLOR)[] questions = {
        (COLOR.PURPLE,COLOR.PURPLE),
        (COLOR.BLUE,COLOR.PURPLE),
        (COLOR.BLUE,COLOR.BLUE),
        (COLOR.BLUE,COLOR.BROWN),
    };

    private bool woke = false; // dirty hack

    void Awake()
    {
        if (woke) return;

        woke = true;

        continueButton = transform.Find("ContinueButton").GetComponent<HoldButton>();
        question = transform.Find("Question").GetComponent<TMP_Text>();
        foreach (int color in Enum.GetValues(typeof(COLOR))) {
            buttons[color] = transform.Find(colorNames[color]).GetComponent<ColorButton>();
        }
        continueButton.interactable = false;
    }

    private void EnableButtons() {
        foreach (var button in buttons) {
            button.interactable = true;
            button.bg.color = Color.white;
            button.Init();
        }
    }
    private void DisableButtons() {
        foreach (var button in buttons) { button.interactable = false; }
    }

    protected virtual (COLOR,COLOR) GetQuestion(int index) {
        return questions[index];
    }

    public override void Init() {
        if (!woke) Awake();

        questionNumber = 0;
        AskQuestion();
    }

    public static COLOR colorID(string color) { return (COLOR)Array.IndexOf(colorNames, color); }

    public virtual void AskQuestion() {
        if (questionNumber >= 1) {//questions.Length) {
            SetUIState();
            return;
        }
        continueButton.interactable = false;
        question.color = colors[(int)GetQuestion(questionNumber).Item1];
        question.text = colorNames[(int)GetQuestion(questionNumber).Item2];
        EnableButtons();
    }
        
    public virtual bool SubmitAnswer(COLOR color) {
        continueButton.interactable = true;
        DisableButtons();
        var correct = buttons[(int)GetQuestion(questionNumber).Item1];
        var chosen = buttons[(int)color];
        chosen.bg.color = colors[(int)COLOR.RED];
        correct.bg.color = colors[(int)COLOR.GREEN];
        ++questionNumber;
        return true;
    }
}
