using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum Device { Mouse, Keyboard, Gamepad }
public enum ControlType { Axis, Button, Key, Vector2, Vector3, Quaternion, Integer, Stick, Dpad, Touch }

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class RebindingButton : MonoBehaviour {

    [SerializeField]
    private Device device;
    [SerializeField]
    private ControlType type;

    [SerializeField]
    private InputActionReference action;
    [SerializeField]
    private int bindingIndex;
    private const string waiting = "Awaiting Input...";
    private Button btn;
    private Image btnBg;
    private TextMeshProUGUI label;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private static InputActionAsset inputAsset;

    void Start() {

        ControlsManager.Instance.rebindingBtns.Add(this);

        if(RebindingButton.inputAsset == null)
            RebindingButton.inputAsset = EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset;

        btn = GetComponent<Button>();
        btnBg = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        btnBg.enabled = true;

        updateLabel();

        btn.onClick.AddListener(setupRebinding);
    }

    private void setupRebinding() {

        btnBg.enabled = false;
        label.SetText(waiting);

        ControlsManager.Instance.inputActionAsset.Disable();

        startRebinding();
    }

    private void startRebinding() {

        rebindingOperation = action.action.PerformInteractiveRebinding(bindingIndex)
            .WithMatchingEventsBeingSuppressed(true)
            .WithExpectedControlType(type.ToString())
            .WithControlsHavingToMatchPath("<" + device.ToString() + ">")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => postRebinding())
            .Start();
    }

    private void postRebinding() {

        rebindingOperation.Dispose();

        ControlsManager.Instance.inputActionAsset.Enable();

        btnBg.enabled = true;

        PlayerPrefs.SetString(ControlsManager.controlsPrefKey, inputAsset.SaveBindingOverridesAsJson());

        updateLabel();
    }

    public void resetDefaultControls() {

        ControlsManager.Instance.inputActionAsset.LoadBindingOverridesFromJson(PlayerPrefs.GetString(ControlsManager.defaultControlsPrefKey));
        PlayerPrefs.SetString(ControlsManager.controlsPrefKey, PlayerPrefs.GetString(ControlsManager.defaultControlsPrefKey));

        foreach (RebindingButton btn in ControlsManager.Instance.rebindingBtns)
            btn.updateLabel();
    }

    public void updateLabel() {

        string text = InputControlPath.ToHumanReadableString(
            action.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        if (ControlsManager.Instance.currentGamepadSymbols.ContainsKey(text))
            text = ControlsManager.Instance.currentGamepadSymbols[text];

        label.SetText(text);
    }
}
