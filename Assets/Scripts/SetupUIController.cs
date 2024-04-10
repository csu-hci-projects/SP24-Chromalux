using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetupUIController : MonoBehaviour
{
    public Button StartButton;
    public ExperimentController controller;

    private string name = null;
    private string room = null;

    private Color gray = new Color(.5f,.5f,.5f, 1f);
    private Color green = new Color(.5f,1f,.75f, 1f);

    private static string[] rooms = { "Office", "Forest" };
    public void SetRoom (int room) {
        this.room = rooms[room];
        UpdateButton();
    }

    public void SetName (string name) {
        this.name = name.Trim(' ').NullIfEmpty();
        UpdateButton();
    }

    public void StartExperiment () {
        controller.StartExperiment(name, room);
    }
    private void UpdateButton() {
        if (name != null && room != null) {
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
