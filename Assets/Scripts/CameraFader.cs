using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFader : MonoBehaviour
{
    private GameObject FadeCanvas;

    private bool active;
    private bool fadeIn;

    private float fade;
    private CanvasGroup cg;
    private Action callback;

    public void FadeIn() {
        fadeIn = true;
        active = true;
    }

    public void FadeOut(Action callback) {
        this.callback = callback;
        fadeIn = false;
        active = true;
    }

    void Awake() {
        fadeIn = true;
        active = true;

        fade = 1;

        FadeCanvas = gameObject.transform.GetChild(0).gameObject;
        FadeCanvas.gameObject.SetActive(true);
        cg = FadeCanvas.GetComponent<CanvasGroup>();
        cg.alpha = fade;
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            if (fadeIn) {
                if (fade > 0) {
                    fade -= .5f * Time.deltaTime;
                    cg.alpha = fade;
                } else {
                    active = false;
                }
            } else {
                if (fade < 1) {
                    fade += .5f * Time.deltaTime;
                    cg.alpha = fade;
                } else {
                    active = false;
                    callback();
                }
            }
        }
    }
}
