using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System;
using UnityEngine.InputSystem.LowLevel;

public enum ControlStyle { OneStep, TwoStep }

public class ControlsManager : MonoBehaviour {

    public static ControlsManager Instance;

    [HideInInspector]
    public InputActionAsset inputActionAsset;
    
    [HideInInspector]
    public List<RebindingButton> rebindingBtns = new List<RebindingButton>();

    public const string controlsPrefKey = "controls", defaultControlsPrefKey = "defaultControls";
    private Dictionary<string, string> xboxSymbols = new Dictionary<string, string>() {{"Button North","Y"}, {"Button East", "B"}, {"Button South", "A"}, {"Button West", "X"}};
    private Dictionary<string, string> psSymbols = new Dictionary<string, string>() {{"Button North","Triangle"}, {"Button East", "Circle"}, {"Button South", "X"}, {"Button West", "Square"}};
    public Dictionary<string, string> currentGamepadSymbols;

    [SerializeField]
    private ControlStyle style;

    void Awake() {

        if(Instance != null) {

            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {

        inputActionAsset = EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset;
        inputActionAsset.Enable();

        string controlsPref = PlayerPrefs.GetString(controlsPrefKey, PlayerPrefs.GetString(defaultControlsPrefKey, ""));

        if (string.IsNullOrEmpty(controlsPref))
            PlayerPrefs.SetString(defaultControlsPrefKey, inputActionAsset.ToJson());
        else
            inputActionAsset.LoadBindingOverridesFromJson(controlsPref);

        foreach (RebindingButton btn in rebindingBtns)
            btn.updateLabel();
    }

    void Update() {

        if (UnityEngine.InputSystem.Gamepad.current is UnityEngine.InputSystem.XInput.XInputController) {

            if(currentGamepadSymbols != xboxSymbols) {

                currentGamepadSymbols = xboxSymbols;
                foreach (RebindingButton btn in rebindingBtns)
                    btn.updateLabel();
            }

        }
        else if (UnityEngine.InputSystem.Gamepad.current is UnityEngine.InputSystem.DualShock.DualShockGamepad) {

            if(currentGamepadSymbols != psSymbols) {

                currentGamepadSymbols = psSymbols;
                foreach (RebindingButton btn in rebindingBtns)
                    btn.updateLabel();
            }

        }
    }

    public ControlStyle getStyle() { return style; }

    public void setStyle(ControlStyle style) { this.style = style; }
}
