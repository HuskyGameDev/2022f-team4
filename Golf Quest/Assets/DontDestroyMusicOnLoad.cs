using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusicOnLoad : MonoBehaviour
{
    private AudioSource oldAudioSource;
    private AudioSource newAudioSource;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyMusicOnLoad[] music = FindObjectsOfType<DontDestroyMusicOnLoad>();

        if (music.Length > 1) {

            oldAudioSource = music[0].GetComponent<AudioSource>();
            newAudioSource = music[1].GetComponent<AudioSource>();

            if(oldAudioSource.clip.name != newAudioSource.clip.name) {
                Destroy(music[0].gameObject);
                music[0] = music[1];
                music[1] = null;
            }
            else 
                Destroy(music[1].gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
