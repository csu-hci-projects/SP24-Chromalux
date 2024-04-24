using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover = false;
    private float down = 0;
    private const float HOLDTIME = 0.0f;
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
        barRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        hover = false;
        down = 0;
    }
}
