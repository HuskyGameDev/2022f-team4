using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public enum ControlStyle { OneStep, TwoStep }

public class ControlsManager : MonoBehaviour {

    public static ControlsManager Instance;

    private InputActionAsset inputActionAsset;

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
    }
    
    public void changeBinding(string actionName, InputBinding binding) {

        inputActionAsset.FindAction(actionName).ChangeBinding(binding);
    }

    public ControlStyle getStyle() { return style; }

    public void setStyle(ControlStyle style) { this.style = style; }

    public InputAction get(string actionName) {

        return inputActionAsset.FindAction(actionName);
    }
}
