using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    
    [SerializeField]AudioSource buttonSound;

    public void Play() {
        buttonSound.Play();
    }


}
