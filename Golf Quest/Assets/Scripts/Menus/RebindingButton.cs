using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum BindingCol { Mouse, Keyboard, Gamepad }

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class RebindingButton : MonoBehaviour {

    [SerializeField]
    private InputActionReference action;
    private InputBindingCompositeContext binding;

    [SerializeField]
    private BindingCol col;

    private const string waiting = "Awaiting Input...";
    private Button btn;
    private Image btnBg;
    private TextMeshProUGUI label;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    void Start() {

        btn = GetComponent<Button>();
        btnBg = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        btn.onClick.AddListener(startRebinding);
    }

    private void startRebinding() {

        btnBg.enabled = false;
        label.SetText(waiting);

        rebindingOperation = action.action.PerformInteractiveRebinding()
            .WithControlsHavingToMatchPath(col.ToString())
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => rebindComplete())
            .Start();
    }

    private void rebindComplete() {

        rebindingOperation.Dispose();
        btnBg.enabled = true;

        int bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);

        label.SetText(InputControlPath.ToHumanReadableString(
            action.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice));
    }
}
