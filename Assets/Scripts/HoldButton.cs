﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover = false;
    private float down = 0;
    private const float HOLDTIME = 1f;
    private float width;
    private RectTransform barRect;

    protected override void Awake() {
        base.Awake();
        width = transform.GetComponent<RectTransform>().rect.width;
        barRect = transform.Find("Bar").GetComponent<RectTransform>();
    }

    public override void OnPointerClick(PointerEventData eventData) { } // delete
    public override void OnPointerEnter(PointerEventData eventData) {
        if (!interactable) return;
        hover = true;
        DoStateTransition(SelectionState.Highlighted, false);
    }
    public override void OnPointerExit(PointerEventData eventData) {
        if (!interactable) return;
        hover = false;
        if (down == 0) DoStateTransition(SelectionState.Normal, false);
    }
    public override void OnPointerDown(PointerEventData eventData) {
        if (!interactable) return;
        down = Time.time;
        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(HoldAnim());
    }
    public override void OnPointerUp(PointerEventData eventData) {
        if (!interactable) return;
        down = 0;
        barRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        if (hover) DoStateTransition(SelectionState.Highlighted, false);
        else DoStateTransition(SelectionState.Normal, false);
    }

    public IEnumerator HoldAnim() {
        float delta;
        while (HOLDTIME > (delta = Time.time - down)) {
            barRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, delta / HOLDTIME * width);
            yield return null;
        }
        if (down != 0) onClick.Invoke();
    }
}

/*
namespace Editor {
    [CustomEditor(typeof(HoldButton))]
        public class MyButtonEditor : ButtonEditor {
        public override void OnInspectorGUI()
        {
            HoldButton component = (HoldButton)target;
        
 
            component.bar = (GameObject)EditorGUILayout.ObjectField("Bar Object", component.bar, typeof(GameObject), true);
            base.OnInspectorGUI();
        }
    }
}
*/
