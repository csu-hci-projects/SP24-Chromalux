using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetupUIController : MonoBehaviour
{
    public Button StartButton;
    public ExperimentController controller;

    private string username = null;
    private string room = null;

    private static string[] rooms = { "Office", "Forest" };
    public void SetRoom (int room) {
        this.room = rooms[room];
        UpdateButton();
    }

    public void SetName (string name) {
        this.username = name.Trim(' ').NullIfEmpty();
        UpdateButton();
    }

    public void StartExperiment () {
        controller.StartExperiment(username, room);
        gameObject.SetActive(false);
    }
    private void UpdateButton() {
        if (username != null && room != null) {
            StartButton.interactable = true;
        } else {
            StartButton.interactable = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateButton();
    }
}
