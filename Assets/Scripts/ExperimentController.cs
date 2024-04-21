using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public static ExperimentController Instance { get; private set; }
    private bool setupComplete;
    private string firstRoom;
    private string currentRoom;
    private string subjectName;
    private string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static string subjectEmotionSurveyFilePath;
    private static string subjectStroopTestFilePath;

    enum State
    {
        TUTORIAL,
        FIRST_SURVEY,
        SURVEY,
        QUESTION,
    }
    private State currentState;

    private void Init()
    {
        currentState = State.TUTORIAL;
        setupComplete = false;
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
        currentState = State.FIRST_SURVEY;
    }

    public void SetUIState(ActivityUI activityUI)
    {
        switch (currentState)
        {
            case State.TUTORIAL:
                activityUI.Tutorial();
                break;
            case State.FIRST_SURVEY:
                activityUI.Tutorial3();
                break;
            case State.QUESTION:
                activityUI.Question();
                break;
        }
    }

    public void FinishSetup(string name, string room)
    {
        firstRoom = room;
        currentRoom = room;
        setupComplete = true;
        subjectName = name;
        FileSetup(subjectName);
    }

    public void BeginExperiment()
    {
        currentState = State.QUESTION;
        sceneChanger.ChangeScene(firstRoom);
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
            writerOne.WriteLine("Subject; Room; Emotion; Rating");
        }

        using (StreamWriter writerTwo = new StreamWriter(subjectStroopTestFilePath, true))
        {
            writerTwo.WriteLine("Subject; Room; Completion Time; Correctness");
        }
    }

    public void RecordTaskData(float completionTime, bool passed)
    {
        /*
        TimeSpan totalTime = TimeSpan.FromMilliseconds(end - start);
        DateTime startTime = DateTimeOffset.FromUnixTimeMilliseconds(start).LocalDateTime;
        DateTime endTime = DateTimeOffset.FromUnixTimeMilliseconds(end).LocalDateTime;

        string formattedStartTime = startTime.ToString("HH:mm:ss");
        string formattedEndTime = endTime.ToString("HH:mm:ss");
        string formattedTotalTime = totalTime.ToString(@"hh\:mm\:ss");

        string writeData =
            subjectName + ";" +
            currentRoom + ";" +
            formattedStartTime + ";" +
            formattedEndTime + ";" +
            formattedTotalTime + ";" +
            (passed ? "PASS" : "FAIL");
        */

        string writeData =
            subjectName + ";" +
            currentRoom + ";" +
            completionTime + ";" +
            (passed ? "PASS" : "FAIL");
        try
        {
            using (StreamWriter writer = new StreamWriter(subjectStroopTestFilePath, true))
            {
                writer.WriteLine(writeData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }

    private void RecordSurveyResponses((string, int)[] responses)
    // Subject; Room; Emotion; Response
    {
        foreach (var response in responses)
        {
            Debug.Log(response);
            string line = subjectName + ";" + currentRoom + ";" + response.Item1 + ";" + response.Item2;
            using (StreamWriter writer = new StreamWriter(subjectEmotionSurveyFilePath, true))
            {
                writer.WriteLine(line);
            }
        }
    }

    void Update()
    {
        if (currentState != State.TUTORIAL)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Swapping scenes...");
                sceneChanger.ChangeScene(SceneManager.GetActiveScene().name == "Office" ? "Forest" : "Office");
            }
        }

        // Test stroop data recording function
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentRoom = "Test Room";
            Debug.Log("Recording Stroop Data.....");
            RecordTaskData(4.20f, false);
            currentRoom = null;
        }
    }
}
