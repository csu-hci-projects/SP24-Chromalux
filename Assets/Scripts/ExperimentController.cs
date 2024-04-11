using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    public SceneChanger sceneChanger;
    private static ExperimentController Instance;
    private bool experimentRunning = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartExperiment(string name, string room) {
        experimentRunning = true;
        sceneChanger.ChangeScene(room);
    }

    // Update is called once per frame
    void Update() {
        if (experimentRunning) {
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
