using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public static ExperimentController Instance { get; private set; }
    private bool setupComplete;
    private string firstRoom;
    private string subjectName;
    private string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private string subjectFilePath;

    enum State
    {
        TUTORIAL,
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
    }

    public void SetUIState(ActivityUI activityUI)
    {
        switch (currentState)
        {
            case State.TUTORIAL:
                activityUI.Tutorial();
                break;
        }
    }

    public void FinishSetup(string name, string room)
    {
        firstRoom = room;
        setupComplete = true;
        subjectName = name;
        RecordNameAndRoom(subjectName, room);
    }

    public void BeginExperiment()
    {
        currentState = State.SURVEY;
        sceneChanger.ChangeScene(firstRoom);
    }

    public void RecordNameAndRoom(string name, string room)
    {
        string folderPath = Path.Combine(desktopPath, "Chromalux - Subject Records");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string currentDateAndTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = Path.Combine(folderPath, name + "_" + currentDateAndTime + ".txt");
        string subjectInfo = name + ", " + room + "\n";

        subjectFilePath = filePath;

        using (StreamWriter writer = new StreamWriter(subjectFilePath, true))
        {
            writer.Write(subjectInfo);
        }
    }

    public void RecordTaskData(long start, long end, bool passed)
    {
        TimeSpan totalTime = TimeSpan.FromMilliseconds(end - start);
        DateTime startTime = DateTimeOffset.FromUnixTimeMilliseconds(start).LocalDateTime;
        DateTime endTime = DateTimeOffset.FromUnixTimeMilliseconds(end).LocalDateTime;

        string formattedStartTime = startTime.ToString("HH:mm:ss");
        string formattedEndTime = endTime.ToString("HH:mm:ss");
        string formattedTotalTime = totalTime.ToString(@"hh\:mm\:ss");

        string writeData = "S: " + formattedStartTime + "| F: " + formattedEndTime + "| Total: " + formattedTotalTime + " : " + (passed ? "PASS" : "FAIL");

        try
        {
            using (StreamWriter writer = new StreamWriter(subjectFilePath, true))
            {
                writer.WriteLine(writeData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing to file: " + e.Message);
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
    }
}
