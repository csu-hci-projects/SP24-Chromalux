using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    protected ActivityUI activityUI;
    public void Enable(ActivityUI activityUI) {
        this.activityUI = activityUI;
        transform.gameObject.SetActive(true);
        Init();
    }

    public virtual void Init() { }

    public void SetUIState() {
        Debug.Assert(activityUI != null, "UIPanel: activityUI cannot be null");
        ExperimentController.Instance.SetUIState(activityUI);
    }
}
