using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : PracticeQuestionController
{
    private static System.Random rng;
    private DateTime start;
    private (COLOR, COLOR)[] questions;

    protected override (COLOR, COLOR) GetQuestion(int index) { return questions[index]; }
    protected override int nQuestions() { return questions.Length; }

    public override void Init() {
        if (rng == null) rng = new System.Random((int)System.DateTime.Now.Ticks);
        if (questions == null) questions = new (COLOR, COLOR)[16];

        for (int i = 0; i < 8; i++) {
            COLOR c = (COLOR)rng.Next(0, 5);
            questions[i] = new (c, c);
        }
        for (int i = 8; i < 16; i++) {
            COLOR c = (COLOR)rng.Next(0, 5);
            COLOR d = (COLOR)(rng.Next((int)c+1, (int)c+5) % 5);
            questions[i] = new (c, d);
        }
        //shuffle
        int n = 16;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            var value = questions[k];
            questions[k] = questions[n];
            questions[n] = value;
        }

        base.Init();
    }

    public override void AskQuestion() {
        start = DateTime.Now;
        base.AskQuestion();
    }

    public override bool SubmitAnswer(COLOR color) {
        var question = GetQuestion(questionNumber);

        float elapsed = (float)DateTime.Now.Subtract(start).TotalSeconds;
        bool congruent = question.Item1 == question.Item2;
        bool correct = base.SubmitAnswer(color);

        ExperimentController.Instance.RecordStroopData(elapsed, correct, congruent);

        return true;
    }
}
