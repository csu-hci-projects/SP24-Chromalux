using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public static ExperimentController Instance { get; private set; }
    public bool setupComplete { get; private set; }

    private string firstRoom;
    private string subjectName;
    private string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static string subjectFilePath;
    private static string subjectEmotionSurveyFilePath;
    private static string subjectStroopTestFilePath;
    private int envNumber;

    enum State
    {
        INIT,
        TUTORIAL1,
        TUTORIAL2,
        TUTORIAL3,
        FIRST_SURVEY,
        SURVEY,
        FIRST_SCENELOAD,
        SECOND_SCENELOAD,
        PRACTICE_QUESTION_INTRO,
        PRACTICE_QUESTIONS,
        FIRST_QUESTION_INTRO,
        QUESTION_INTRO,
        QUESTIONS,
        END,
    }
    private State currentState;

    private void Init()
    {
        currentState = State.INIT;
        setupComplete = false;
        envNumber = 0;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    public void FinishSurvey((string, int)[] results)
    {
        RecordSurveyResponses(results);
        ++envNumber;
    }
    public void RecordTaskData(float completionTime, bool passed, bool congruent) {
        string writeData =
            subjectName + ";" +
            firstRoom + ";" +
            envNumber + ";" +
            completionTime + ";" +
            (passed ? "PASS" : "FAIL") + ";" +
            (congruent ? "Congruent" : "Incongruent");
        Debug.Log(writeData);
        //try {
        //    using (StreamWriter writer = new StreamWriter(subjectStroopTestFilePath, true)) {
        //        writer.WriteLine(writeData);
        //    }
        //} catch (Exception e) {
        //    Debug.LogError("Error writing to file: " + e.Message);
        //}
    }

    // advance to the next state
    // each case is the outgoing state, so rules for setting up EG: TUTORIAL2 should go under case TUTORIAL1
    public void SetUIState(ActivityUI activityUI)
    {
        UIPanel uiPanel;
        switch (currentState)
        {
            case State.INIT:
                currentState = State.TUTORIAL1;
                activityUI.SwitchPanel("Tutorial1");
                break;
            case State.TUTORIAL1:
                currentState = State.TUTORIAL2;
                activityUI.SwitchPanel("Tutorial2");
                break;
            case State.TUTORIAL2:
                currentState = State.FIRST_SURVEY;
                uiPanel = activityUI.SwitchPanel("Survey");
                uiPanel.Init();
                break;
            case State.FIRST_SURVEY:
                currentState = State.TUTORIAL3;
                activityUI.SwitchPanel("Tutorial3");
                break;
            case State.TUTORIAL3:
                currentState = State.FIRST_SCENELOAD;
                activityUI.Disable(); // prevent double advance during transition
                sceneChanger.ChangeScene(firstRoom);
                break;
            case State.FIRST_SCENELOAD: // called on first scene load to init new ui instance
                currentState = State.PRACTICE_QUESTION_INTRO;
                activityUI.QuestionIntro(0);
                break;
            case State.PRACTICE_QUESTION_INTRO:
                currentState = State.PRACTICE_QUESTIONS;
                uiPanel = activityUI.SwitchPanel("PracticeQuestion");
                uiPanel.Init();
                break;
            case State.PRACTICE_QUESTIONS:
                currentState = State.FIRST_QUESTION_INTRO;
                activityUI.QuestionIntro(-1);
                break;
            case State.FIRST_QUESTION_INTRO:
                currentState = State.QUESTIONS;
                activityUI.SwitchPanel("Question");
                break;
            case State.QUESTIONS:
                currentState = State.SURVEY;
                activityUI.SwitchPanel("Survey");
                break;
            case State.SURVEY: // survey completed
                if (envNumber > 6) {
                    // EXPERIMENT OVER!
                    currentState = State.END;
                    activityUI.SwitchPanel("End");
                    return;
                } else if (envNumber == 4) {
                    currentState = State.SECOND_SCENELOAD;
                    activityUI.Disable(); // prevent double advance during transition
                    if (firstRoom == "Office") sceneChanger.ChangeScene("Forest");
                    else sceneChanger.ChangeScene("Office");
                    return;
                }

                // trigger environment change here

                currentState = State.QUESTION_INTRO;
                activityUI.QuestionIntro(envNumber);
                break;
            case State.QUESTION_INTRO:
                currentState = State.QUESTIONS;
                uiPanel = activityUI.SwitchPanel("Question");
                uiPanel.Init();
                break;
            case State.SECOND_SCENELOAD: // called on second scene load to init new ui instance
                currentState = State.QUESTION_INTRO;
                activityUI.QuestionIntro(envNumber);
                break;
        }
    }

    public void FinishSetup(string name, string room)
    {
        firstRoom = room;
        setupComplete = true;
        subjectName = name;
        FileSetup(subjectName);
    }

    private void FileSetup(string name)
    {
        string folderPath = Path.Combine(desktopPath, "Chromalux - Subject Records");
        string currentDateAndTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string stroopFilePath = Path.Combine(folderPath, name + "_STROOP_" + currentDateAndTime + ".txt");
        string surveyFilePath = Path.Combine(folderPath, name + "_SURVEY_" + currentDateAndTime + ".txt");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        subjectEmotionSurveyFilePath = surveyFilePath;
        subjectStroopTestFilePath = stroopFilePath;

        using (StreamWriter writerOne = new StreamWriter(subjectEmotionSurveyFilePath, true))
        {
            writerOne.WriteLine("Subject; Group; Environment Number; Emotion; Rating");
        }

        using (StreamWriter writerTwo = new StreamWriter(subjectStroopTestFilePath, true))
        {
            writerTwo.WriteLine("Subject; Group; Environment Number; Completion Time; Correctness; Congruency");
        }
    }

    private void RecordSurveyResponses((string, int)[] responses)
    // Subject; Room; Emotion; Response
    {
        foreach (var response in responses)
        {
            Debug.Log(response);
            string line = 
                subjectName + ";" + 
                firstRoom + ";" +
                envNumber + ";" + 
                response.Item1 + ";" + 
                response.Item2;
            using (StreamWriter writer = new StreamWriter(subjectEmotionSurveyFilePath, true))
            {
                writer.WriteLine(line);
            }
        }
    }
}
