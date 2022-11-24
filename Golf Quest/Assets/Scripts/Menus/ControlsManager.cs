using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using TMPro;

public enum ControlStyle { OneStep, TwoStep }

public class ControlsManager : MonoBehaviour {

    public static ControlsManager Instance;

    [HideInInspector]
    public InputActionAsset inputActionAsset;
    
    [HideInInspector]
    public List<RebindingButton> rebindingBtns = new List<RebindingButton>();

    public const string controlsPrefKey = "controls", defaultControlsPrefKey = "defaultControls";

    public TMP_FontAsset defaultFont, symbolsFont;

    public Dictionary<string, char> genericSymbols = new Dictionary<string, char>() {
        {"Left Stick", '8'},
        {"Left Stick/Down", '9'},
        {"Left Stick/Right", '0'},
        {"Left Stick/Up", '-'},
        {"Left Stick/Left", '='},
        {"Left Stick/Y", 'q'},
        {"Left Stick/X", 'w'},
        {"Left Stick Press", 'e'},
        {"Right Stick", 'y'},
        {"Right Stick/Down", 'u'},
        {"Right Stick/Right", 'i'},
        {"Right Stick/Up", 'o'},
        {"Right Stick/Left", 'p'},
        {"Right Stick/Y", '['},
        {"Right Stick/X", ']'},
        {"Right Stick Press", '\\'},
        {"Left Shoulder", 'd'},
        {"Left Trigger", 'j'},
        {"Right Shoulder", '\''},
        {"Right Trigger", 'v'},
        {"Button South", ','},
        {"Button East", '.'},
        {"Button North", '/'},
        {"Button West", 'Q'},
        {"D-Pad", 'Y'},
        {"D-Pad/Down", 'U'},
        {"D-Pad/Right", 'I'},
        {"D-Pad/Up", 'O'},
        {"D-Pad/Left", 'P'},
        {"D-Pad/Y", 'A'},
        {"D-Pad/X", 'S'},
        {"Start", 'V'},
        {"Select", 'B'},

        {"Left Button", 'J'},
        {"Middle Button", 'K'},
        {"Right Button", 'L'},
        {"Scroll/Up", 'Z'},
        {"Scroll/Y", 'X'},
        {"Scroll/Down", 'C'}
    };
    private Dictionary<string, char> xboxSymbols = new Dictionary<string, char>() {
        {"Left Shoulder", 'f'},
        {"Left Trigger", 'k'},
        {"Right Shoulder", 'z'},
        {"Right Trigger", 'b'}
    };
    private Dictionary<string, char> psSymbols = new Dictionary<string, char>() {
        {"Left Shoulder", 'g'},
        {"Left Trigger", 'l'},
        {"Right Shoulder", 'x'},
        {"Right Trigger", 'n'}
    };  
    public Dictionary<string, char> currentGamepadSymbols = new Dictionary<string, char>();

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
