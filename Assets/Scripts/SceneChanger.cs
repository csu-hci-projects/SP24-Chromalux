using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public CameraFader cf;

    private static SceneChanger Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Instance.cf = this.cf; // use cf linked in scene
            Destroy(gameObject);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Debug.Log("Swapping scenes...");
            ChangeScene("Forest");
        }
    }

    public void SetScene(String name) {
        SceneManager.LoadScene(name);
    }

    public void ChangeScene(String name) {
        cf.FadeOut(delegate () { SetScene(name); });
    }
}
