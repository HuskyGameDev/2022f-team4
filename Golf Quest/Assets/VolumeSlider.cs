using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private string parameterName;

    private Slider slider;

    //private float masterVolume = 0.5f, musicVolume = 0.5f, sfxVolume = 0.5f;
    //private float masterVolume, musicVolume, sfxVolume;
    private float volume;

    void Start() {
        Debug.Log("Start Volume Slider");
       /* mixer.GetFloat("MasterVolume", out masterVolume);
        mixer.GetFloat("MusicVolume", out  musicVolume);
        mixer.GetFloat("SFXVolume", out  sfxVolume);
        mixer.SetFloat("MasterVolume", masterVolume);
        mixer.SetFloat("MusicVolume",musicVolume);
        mixer.SetFloat("SFXVolume", sfxVolume);
*/
        slider = GetComponent<Slider>();
        mixer.GetFloat(parameterName, out volume);
        Debug.Log(volume);
        slider.value = Mathf.Exp(volume/20);
    }

    void Update() {

        //Debug.Log(mixer);
    }

    public void changeSlider(float value) {

        mixer.SetFloat(parameterName, Mathf.Log(value) * 20);
    }
}
