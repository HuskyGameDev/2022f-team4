using UnityEngine;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class MimicText : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI label;

    private TextMeshProUGUI self;

    void Start() {

        self = GetComponent<TextMeshProUGUI>();
    }  

    void Update() {

        self.font = label.font;
        self.SetText(label.text);
    }
}
