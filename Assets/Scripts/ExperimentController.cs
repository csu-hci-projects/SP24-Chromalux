using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public static ExperimentController Instance { get; private set; }
    private bool setupComplete;
    private string firstRoom;

    enum State {
        TUTORIAL,
        FIRST_SURVEY,
        SURVEY,
        QUESTION,
    }
    private State currentState;

    private void Init() {
        currentState = State.TUTORIAL;
        setupComplete = false;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        } else {
            Destroy(gameObject);
        }
    }

    public void PublishSurvey((string, int)[] results) {
        currentState = State.FIRST_SURVEY;
    }

    public void SetUIState(ActivityUI activityUI) {
        switch (currentState) {
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

    public void FinishSetup(string name, string room) {
        firstRoom = room;
        setupComplete = true;
    }

    public void BeginExperiment() {
        currentState = State.QUESTION;
        sceneChanger.ChangeScene(firstRoom);
    }

    // Update is called once per frame
    void Update() {
        if (currentState != State.TUTORIAL) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Debug.Log("Swapping scenes...");
                if (SceneManager.GetActiveScene().name == "Office") {
                    sceneChanger.ChangeScene("Forest");
                } else {
                    sceneChanger.ChangeScene("Office");
                }
            }
        }
    }

}
