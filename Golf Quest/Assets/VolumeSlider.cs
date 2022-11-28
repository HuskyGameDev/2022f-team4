using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour {

    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private string parameterName;

    void Start() {


    }

    void Update() {

        Debug.Log(mixer);
    }

    public void changeSlider(float value) {

        mixer.SetFloat(parameterName, Mathf.Log(value) * 20);
    }
}
