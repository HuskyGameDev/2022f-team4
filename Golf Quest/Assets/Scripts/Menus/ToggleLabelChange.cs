using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle))]
public class ToggleLabelChange : MonoBehaviour {

    private Toggle toggle;
    private TextMeshProUGUI label;

    public string onLabel, offLabel;

    void Start() {

        toggle = GetComponent<Toggle>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        toggle.isOn = ControlsManager.Instance.getStyle() == ControlStyle.OneStep;

        valueChanged(toggle.isOn);
    }

    public void valueChanged(bool on) {

        if(on) {
            label.SetText(onLabel);
            ControlsManager.Instance.setStyle(ControlStyle.OneStep);
        } else {
            label.SetText(offLabel);
            ControlsManager.Instance.setStyle(ControlStyle.TwoStep);
        }
    }
}
